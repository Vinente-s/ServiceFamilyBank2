using Microsoft.EntityFrameworkCore;
using ServiceFamilyBank.Models;


namespace ServiceFamilyBank.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext (DbContextOptions dbContextOptions) : base (dbContextOptions)
        {
            this.Database.SetCommandTimeout(600);
        }

        public DbSet<Usuario> dusuarios { get; set; }
        public DbSet<Perfis> dperfis { get; set; }
    }
}