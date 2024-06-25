using HouseFinderBackEnd.Data.Buildings;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HouseFinderBackEnd.Data
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<Property> Properties { get; set; } = new HashSet<Property>();

        public ICollection<Property> WatchList { get; set; } = new HashSet<Property>();
    }
}
