using PUCCI.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUCCI.Areas.Identity.Data;

// Add profile data for application users by adding properties to the User class
public class User : IdentityUser
{
    // User Cloud Resources
    public virtual ICollection<Image>? Images { get; set; }
    public virtual ICollection<Container>? Containers { get; set; }
}

