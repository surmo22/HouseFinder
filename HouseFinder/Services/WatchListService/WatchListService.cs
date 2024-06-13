using HouseFinderBackEnd.Data;
using HouseFinderBackEnd.Data.Buildings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HouseFinderBackEnd.Services.WatchListService
{
    public class WatchListService : IWatchListService
    {
        private UserManager<User> _userManager;
        private ApplicationDbContext _context;

        public WatchListService(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task AddPropertyToUserWatchList(ClaimsPrincipal user, int propertyId)
        {
            var userId = GetUserId(user);

            var watchList = await _context.WatchLists
                                          .Include(wl => wl.Properties)
                                          .Include(wl => wl.User)
                                          .FirstOrDefaultAsync(wl => wl.User.Id == userId);

            watchList ??= await CreateUserWatchList(userId, watchList);

            var propertyExists = watchList.Properties.Any(p => p.Id == propertyId);
            if (!propertyExists)
            {
                var propertyToAdd = await _context.Properties.FindAsync(propertyId) 
                    ?? throw new ArgumentException("Property not found", nameof(propertyId));
                watchList.Properties.Add(propertyToAdd);
            }

            await _context.SaveChangesAsync();
        }

        private async Task<WatchList> CreateUserWatchList(string userId, WatchList? watchList)
        {
            var foundUser = await _userManager.FindByIdAsync(userId);
            if (foundUser == null)
            {
                throw new InvalidOperationException("User not found");
            }
            watchList = new WatchList
            {
                User = foundUser,
                Properties = []
            };
            _context.WatchLists.Add(watchList);
            return watchList;
        }

        public async Task<IEnumerable<Property>> GetUserWatchList(ClaimsPrincipal user)
        {
            string userId = GetUserId(user);

            var properties = await _context.WatchLists
                 .Where(w => w.User.Id == userId)
                 .SelectMany(w => w.Properties) 
                 .Include(p => p.Location)
                 .ThenInclude(l => l.City)
                 .Include(p => p.Location)
                 .ThenInclude(l => l.District)
                 .ToListAsync();

            return properties ?? [];
        }

        private static string GetUserId(ClaimsPrincipal user)
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

            return userId;
        }

        public async Task RemovePropertyFromUserWatchList(ClaimsPrincipal user, int propertyId)
        {
            var userId = GetUserId(user);

            var watchList = await _context.WatchLists
                                          .Include(wl => wl.Properties)
                                          .Include(wl => wl.User)
                                          .FirstOrDefaultAsync(wl => wl.User.Id == userId);

            if (watchList == null)
            {
                throw new InvalidOperationException("User watch list not found.");
            }

            var propertyToRemove = watchList.Properties.FirstOrDefault(p => p.Id == propertyId);
            if (propertyToRemove != null)
            {
                watchList.Properties.Remove(propertyToRemove);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("Property not found in watch list", nameof(propertyId));
            }
        }
    }
}
