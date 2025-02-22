using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ProjetoIntegrador.Models;

namespace ProjetoIntegrador.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<UserNutri> Pedidos { get; set; }
        public DbSet<AdmCriou> AdmCriou { get; set; }
    }
}
