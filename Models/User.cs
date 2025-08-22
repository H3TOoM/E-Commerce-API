using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]

        public string FullName { get; set; }

        [Required, EmailAddress]
        [StringLength(100, MinimumLength = 5)]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string PasswordHash { get; set; }

        [Required]
        public string Role { get; set; }

        public ICollection<Order> Orders { get; set; }
        public Cart Cart { get; set; }
        public ICollection<Address> Addresses
        {
            get; set;
        }
    }
}
