using Cafeteria.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cafeteria.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
            
        }

        public DbSet<Category> Category { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
    }
}
