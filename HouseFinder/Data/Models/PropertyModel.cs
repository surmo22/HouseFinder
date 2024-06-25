using HouseFinderBackEnd.Data.Buildings;

namespace HouseFinderBackEnd.Data.Models
{
    public class PropertyModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public decimal Price { get; set; }
        public List<string> ImageUrls { get; set; }
        public string ContactPhone { get; set; }
        public string City { get; set; }
    }
}
