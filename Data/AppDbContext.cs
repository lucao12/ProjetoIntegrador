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
        public DbSet<Alimento> Alimentos { get; set; }
        public DbSet<AlimentoUsuario> AlimentosUsuarios { get; set; }
        public DbSet<UserNutri> Pedidos { get; set; }
        public DbSet<AdmCriou> AdmCriou { get; set; }
        public DbSet<Mensagem> Mensagem { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurando o relacionamento muitos-para-muitos entre Usuario e Alimento
            modelBuilder.Entity<AlimentoUsuario>()
                .HasKey(x => new { x.UsuarioId, x.AlimentoId });

            modelBuilder.Entity<AlimentoUsuario>()
                .HasOne(x => x.Usuario)
                .WithMany(u => u.Alimento)
                .HasForeignKey(x => x.UsuarioId);

            modelBuilder.Entity<AlimentoUsuario>()
                .HasOne(x => x.Alimento)
                .WithMany()
                .HasForeignKey(x => x.AlimentoId);
        }
    }
}
