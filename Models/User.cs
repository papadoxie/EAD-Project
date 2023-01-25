using System.ComponentModel.DataAnnotations;
namespace PUCCI.Models
{
    public class User
    {
        // User identification attributes
        public int ID { get; set; } // Needed for Entity Framework

        [Required(ErrorMessage = "Username is required")]
        public string? Username { get; set; }
        
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
        
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        // User Cloud Resources
        public List<Image>? Images { get; set; }
        public List<Container>? Containers { get; set; }
    }
}
