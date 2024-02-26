using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LinkAggregatorMVC.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Reflection.Emit;

namespace LinkAggregatorMVC.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Links> Links { get; set; } = default!;
       public DbSet<CategoriesLookup> CategoriesLookup { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //modelBuilder.Entity<Links>()
            //    .HasMany(c => c.LinkCategories)
            //    .WithOne(e => e.Links);
            // modelBuilder.Entity<CategoriesLookup>().ToTable("CategoriesLookup");

            //for some reason i needed to manually add in the category field, ef core could not automatically add it for some reason
            modelBuilder.Entity<CategoriesLookup>().Property(p => p.Category);
            modelBuilder.Entity<CategoriesLookup>().HasKey(k => k.ID);
            

            base.OnModelCreating(modelBuilder);
        }
    }
}
