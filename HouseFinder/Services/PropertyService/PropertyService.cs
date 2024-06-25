using AutoMapper;
using HouseFinderBackEnd.Data;
using HouseFinderBackEnd.Data.Buildings;
using HouseFinderBackEnd.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HouseFinderBackEnd.Services.PropertyService
{
    public class PropertyService : IPropertyService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public PropertyService(ApplicationDbContext context, UserManager<User> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task DeleteProperty(int id)
        {
            _context.Properties.Remove(await _context.Properties.FindAsync(id) 
                ?? throw new InvalidOperationException("Property not found"));
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PropertyModel>> GetProperties(int page, int pageSize)
        {
            int skip = (page - 1) * pageSize;

            var properties = await _context.Properties
                .Skip(skip)
                .Take(pageSize)
                .Include(p => p.City)
                .Select(x => _mapper.Map<PropertyModel>(x))
                .ToListAsync();

            return properties;
        }

        public async Task<PropertyModel> GetProperty(int id)
        {
            var property = _mapper.Map<PropertyModel>(await _context.Properties
                .Include(p => p.City)
                .FirstOrDefaultAsync(p => p.Id == id));

            if (property == null)
            {
                throw new InvalidDataException("Property not found");
            }

            return property;
        }

        public async Task<ICollection<PropertyModel>> GetUserPropertyForSale(string? userId)
        {
            var user = await _userManager.FindByIdAsync(userId ?? throw new InvalidOperationException("User not found")) ?? throw new ArgumentNullException(nameof(userId));
            var userPropertiesForSale = await _context.Properties
                .Where(w => w.User.Id == user.Id)
                .Include(p => p.City)
                .Select(x => _mapper.Map<PropertyModel>(x))
                .ToListAsync();
            return userPropertiesForSale;
        }

        public async Task<IEnumerable<PropertyModel>> GetUserWatchList(string? userId)
        {
            var user = await _userManager.FindByIdAsync(userId ?? throw new InvalidOperationException("User not found")) ?? throw new ArgumentNullException(nameof(userId));
            var watchList = await _context.Properties
                .Where(w => w.User.Id == user.Id)
                .Include(p => p.City)
                .Select(x => _mapper.Map<PropertyModel>(x))
                .ToListAsync();
            return watchList;
        }

        public async Task<PropertyModel> PostProperty(PropertyModel property, ClaimsPrincipal claimsPrincipal)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property), "Property cannot be null");
            }
            var propertyEntity = _mapper.Map<Property>(property);

            var user = await GetUser(claimsPrincipal);
            propertyEntity.User = user;

            var city = await _context.Cities.FirstOrDefaultAsync
                (c => c.Name.ToLower().Equals(property.City.Trim().ToLower()));

            city ??= new City { Name = property.City };

            propertyEntity.City = city;

            await _context.Properties.AddAsync(propertyEntity);
            await _context.SaveChangesAsync();

            return property;
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
