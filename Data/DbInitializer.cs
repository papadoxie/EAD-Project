//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;
//using PUCCI.Models;

//namespace PUCCI.Data
//{
//    public class PUCCIContext : IdentityDbContext
//    {
//        public PUCCIContext(DbContextOptions<PUCCIContext> options)
//            : base(options)
//        {
//        }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            optionsBuilder.UseSqlServer(
//                options => options.EnableRetryOnFailure()
//            );
//        }

//        public DbSet<PUCCI.Models.User> User { get; set; } = default!;
//        public DbSet<PUCCI.Models.Container> Container { get; set; } = default!;
//        public DbSet<PUCCI.Models.Image> Image { get; set; } = default!;

//    }
//}
