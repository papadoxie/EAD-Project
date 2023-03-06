using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PUCCI.Areas.Identity.Data;
using PUCCI.Models.Audit;

namespace PUCCI.Data;

public partial class PUCCIIdentityContext : IdentityDbContext<User>
{
    public PUCCIIdentityContext(DbContextOptions<PUCCIIdentityContext> options, ICurrentUserService currentUserService)
        : base(options)
    {
        _currentUserService = currentUserService;
        Database.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
	}

	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Add auditing information before saving changes to DB
    /// </summary>
    public void Audit()
    {
        var entries = ChangeTracker.Entries();

        foreach (var entry in entries)
        {
            if (entry.Entity is AuditModel entity)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entity.CreationTime = DateTime.Now;
                        entity.ModifiedTime = entity.CreationTime;
                        entity.CreatedBy = _currentUserService.GetCurrentUsername();
                        break;
                    case EntityState.Modified:
                        entity.ModifiedTime = DateTime.Now;
                        break;
                }
            }
        }
    }

    // Needed to get the User who made changes
	private readonly ICurrentUserService _currentUserService;
}
