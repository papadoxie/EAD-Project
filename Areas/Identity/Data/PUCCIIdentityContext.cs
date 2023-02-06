using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PUCCI.Areas.Identity.Data;

namespace PUCCI.Data;

public partial class PUCCIIdentityContext : IdentityDbContext<User>
{
    public PUCCIIdentityContext(DbContextOptions<PUCCIIdentityContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        // Seed the DB with initial Entities
        // DbInitializer.Seed(builder);
    }
}
