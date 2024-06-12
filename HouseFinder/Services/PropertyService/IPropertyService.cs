using HouseFinderBackEnd.Data.Buildings;
using Microsoft.AspNetCore.Mvc;

namespace HouseFinderBackEnd.Services.PropertyService
{
    public interface IPropertyService
    {
        Task DeleteProperty(int id);
        Task<IEnumerable<Property>> GetProperties(int page, int pageSize);
        Task<Property> GetProperty(int id);
        Task<Property> PostProperty(Property property);
    }
}
