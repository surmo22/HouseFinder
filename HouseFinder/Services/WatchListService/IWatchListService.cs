using HouseFinderBackEnd.Data.Buildings;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HouseFinderBackEnd.Services.WatchListService
{
    public interface IWatchListService
    {
        Task AddPropertyToUserWatchList(ClaimsPrincipal user, int propertyId);
        Task<IEnumerable<Property>> GetUserWatchList(ClaimsPrincipal user);
        Task RemovePropertyFromUserWatchList(ClaimsPrincipal user, int propertyId);
    }
}
