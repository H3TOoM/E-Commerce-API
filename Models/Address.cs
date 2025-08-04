namespace E_Commerce.Models
{
    public class Address
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }

        public User User { get; set; }
    }
}
