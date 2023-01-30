using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using PUCCI.Areas.Identity.Data;
using PUCCI.Models;

namespace PUCCI.Data
{
    public partial class PUCCIIdentityContext : IdentityDbContext<User>
    {
        public DbSet<Container>? Containers { get; set; }
        public DbSet<Image>? Images { get; set; }
    }
}
