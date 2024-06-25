using HouseFinderBackEnd.Data.Buildings;
using HouseFinderBackEnd.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HouseFinderBackEnd.Services.PropertyService
{
    public interface IPropertyService
    {
        Task DeleteProperty(int id);
        Task<IEnumerable<PropertyModel>> GetProperties(int page, int pageSize);
        Task<PropertyModel> GetProperty(int id);
        Task<ICollection<PropertyModel>> GetUserPropertyForSale(string? userId);
        Task<IEnumerable<PropertyModel>> GetUserWatchList(string? userId);
        Task<PropertyModel> PostProperty(PropertyModel property, ClaimsPrincipal claimsPrincipal);
    }
}
