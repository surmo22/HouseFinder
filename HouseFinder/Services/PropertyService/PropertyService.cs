using HouseFinderBackEnd.Data;
using HouseFinderBackEnd.Data.Buildings;
using Microsoft.EntityFrameworkCore;

namespace HouseFinderBackEnd.Services.PropertyService
{
    public class PropertyService : IPropertyService
    {
        private readonly ApplicationDbContext _context;

        public PropertyService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task DeleteProperty(int id)
        {
            var property = await GetProperty(id) ?? throw new InvalidOperationException("Property not found");
            _context.Properties.Remove(property);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Property>> GetProperties(int page, int pageSize)
        {
            int skip = (page - 1) * pageSize;

            var properties = await _context.Properties
                .Skip(skip)
                .Take(pageSize)
                .Include(p => p.Location).ThenInclude(l => l.City)
                .Include(l => l.Location).ThenInclude(l => l.District)
                .ToListAsync();

            return properties;
        }

        public async Task<Property> GetProperty(int id)
        {
            var property = await _context.Properties
                .Include(p => p.Location).ThenInclude(l => l.City)
                .Include(l => l.Location).ThenInclude(l => l.District)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (property == null)
            {
                throw new InvalidDataException("Property not found");
            }

            return property;
        }

        public async Task<Property> PostProperty(Property property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property), "Property cannot be null");
            }

            await _context.Properties.AddAsync(property);
            await _context.SaveChangesAsync();

            return property;
        }
    }
}
