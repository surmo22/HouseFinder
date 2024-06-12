namespace HouseFinderBackEnd.Data
{
    public class Property
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ContactPhone { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
