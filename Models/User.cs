using System.ComponentModel.DataAnnotations;
namespace EAD_Project.Models
{
    public class User
    {
    // User identification attributes
        [Required(ErrorMessage = "UserName is required")]
        public string? Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required(ErrorMessage = "Email is required")]
         public string? Email { get; set; }
        // User resources on PUCCI
        public List<Container>? Containers { get; set; }
    }
}
