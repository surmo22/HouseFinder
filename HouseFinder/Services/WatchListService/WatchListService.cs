using HouseFinderBackEnd.Data;
using HouseFinderBackEnd.Data.Buildings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HouseFinderBackEnd.Services.WatchListService
{
    public class WatchListService : IWatchListService
    {
        private readonly ApplicationDbContext _context;

        public WatchListService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddPropertyToUserWatchList(ClaimsPrincipal user, int propertyId)
        {
            var appUser = await GetUser(user);

            var propertyToAdd = await _context.Properties.FindAsync(propertyId);
            if (propertyToAdd == null)
            {
                throw new ArgumentException("Property not found", nameof(propertyId));
            }

            appUser.WatchList.Add(propertyToAdd);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Property>> GetUserWatchList(ClaimsPrincipal user)
        {
            var appUser = await GetUser(user);
            return appUser.WatchList;
        }

        public async Task RemovePropertyFromUserWatchList(ClaimsPrincipal user, int propertyId)
        {
            var appUser = await GetUser(user);

            var propertyToRemove = _context.Properties.FirstOrDefault(p => p.Id == propertyId);
            if (propertyToRemove != null)
            {
                appUser.WatchList.Remove(propertyToRemove);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("Property not found in watch list", nameof(propertyId));
            }
        }

        private async Task<User> GetUser(ClaimsPrincipal user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), $"{nameof(user)} cannot be null.");
            }
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                throw new InvalidOperationException($"{nameof(userId)} is null.");
            }

            var foundUser = await _context.Users.Where(u => u.Id == userId)
                .Include(u => u.WatchList)
                .Include(u => u.Properties)
                .FirstOrDefaultAsync();

            if (foundUser == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            return foundUser;
        }
    }
}
