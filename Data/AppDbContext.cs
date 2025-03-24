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
        public DbSet<Dieta> Dieta { get; set; }
        public DbSet<DietaSemana> DietaSemana { get; set; }
        public DbSet<HistoricoPesoAltura> HistoricoPesoAltura { get; set; } // Adicionando o DbSet
        public DbSet<AlimentoQuantidade> AlimentoQuantidade { get; set; }

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
                
            modelBuilder.Entity<HistoricoPesoAltura>()
                .HasOne(h => h.Usuario)
                .WithMany() // Um usuário pode ter vários registros de histórico
                .HasForeignKey(h => h.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade); // Se o usuário for deletado, os históricos também serão deletados
        }
    }
}
