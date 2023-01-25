namespace EAD_Project.Models
{
    public class User
    {
        // User identification attributes
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }

        // User resources on PUCCI
        public List<Image>? Images { get; set; }
        public List<Container>? Containers { get; set; }
    }
}
