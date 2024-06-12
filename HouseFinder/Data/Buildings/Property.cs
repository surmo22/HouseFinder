namespace HouseFinderBackEnd.Data.Buildings
{
    public class Property
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public decimal Price { get; set; }
        public List<string> ImageUrls { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ContactPhone { get; set; }

        public int LocationId { get; set; }
        public Location Location { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }

}
