using HouseFinderBackEnd.Data.Buildings;

namespace HouseFinderBackEnd.Data
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext _context;

        public DataSeeder(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            if (!_context.Cities.Any())
            {
                await SeedCitiesAsync();
            }

            if (!_context.Properties.Any())
            {
                await SeedPropertiesAsync();
            }
        }

        private async Task SeedCitiesAsync()
        {
            var cities = new List<City>
        {
            new City { Name = "CityName1" },
            new City { Name = "CityName2" }
        };

            _context.Cities.AddRange(cities);
            await _context.SaveChangesAsync();
        }

        private async Task SeedPropertiesAsync()
        {
            var properties = new List<Property>
        {
            new Property { Title = "Title1",
                           Description = "Description1",
                           Address = "Address1",
                           Price = 1000,
                           ImageUrls = new List<string> { "ImageUrl1" },
                           CreatedAt = DateTime.Now,
                           City = _context.Cities.First(),
                           ContactPhone = "ContactPhone1",
                           User = _context.Users.First()
            },
            new Property { Title = "Title2",
                           Description = "Description2",
                           Address = "Address2",
                           Price = 2000,
                           ImageUrls = new List<string> { "ImageUrl2" },
                           CreatedAt = DateTime.Now,
                           ContactPhone = "ContactPhone2",
                           City = _context.Cities.First(),
                           User = _context.Users.First()
            }
        };

            _context.Properties.AddRange(properties);
            await _context.SaveChangesAsync();
        }
    }
}