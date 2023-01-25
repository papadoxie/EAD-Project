using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PUCCI.Models;

namespace PUCCI.Data
{
    public class PUCCIContext : DbContext
    {
        public PUCCIContext (DbContextOptions<PUCCIContext> options)
            : base(options)
        {
        }

        public DbSet<PUCCI.Models.User> User { get; set; } = default!;

        public DbSet<PUCCI.Models.Container> Container { get; set; } = default!;
    }
}
