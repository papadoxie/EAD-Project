using System.ComponentModel.DataAnnotations;
namespace PUCCI.Models
{
    public class User
    {
        // User identification attributes
        public int ID { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public string? Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required(ErrorMessage = "Email is required")]
         public string? Email { get; set; }
        // User resources on PUCCI
        public List<Image>? Images { get; set; }
        public List<Container>? Containers { get; set; }
    }
}
