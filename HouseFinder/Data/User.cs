using HouseFinderBackEnd.Data.Buildings;
using Microsoft.AspNetCore.Identity;

namespace HouseFinderBackEnd.Data
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public WatchList WatchList { get; set; } = new WatchList();
        public UserPropertiesForSale UserPropertiesForSale { get; set; } = new UserPropertiesForSale();
    }
}
