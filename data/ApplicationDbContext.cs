using Microsoft.EntityFrameworkCore;
using MyMicroservice.Models;

namespace MyMicroservice.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Articulo> Articulos { get; set; }
        public DbSet<Visita> Visitas { get; set; }  // Agrega el DbSet para las visitas
    }
}
