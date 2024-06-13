using HouseFinderBackEnd.Data.Buildings;

namespace HouseFinderBackEnd.Data
{
    public class WatchList
    {
        public int Id { get; set; }
        public User User { get; set; }
        public ICollection<Property> Properties { get; set; }
    }
}
