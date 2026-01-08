using Microsoft.EntityFrameworkCore;
using HotelManagement.Models;

namespace HotelManagement.Data
{
    public class HotelContext : DbContext
    {
        public HotelContext(DbContextOptions<HotelContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Quarto> Quarto { get; set; }
        public DbSet<Reserva> Reserva { get; set; }
        public DbSet<Funcionario> Funcionario { get; set; }
        public DbSet<Setor> Setor { get; set; }
        public DbSet<AtribuicaoSetor> AtribuicaoSetor { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>().ToTable("Cliente");
            modelBuilder.Entity<Quarto>().ToTable("Quarto");
            modelBuilder.Entity<Reserva>().ToTable("Reserva");
            modelBuilder.Entity<Funcionario>().ToTable("Funcionario");
            modelBuilder.Entity<Setor>().ToTable("Setor");
            modelBuilder.Entity<AtribuicaoSetor>().ToTable("AtribuicaoSetor");

            // Configurar chave composta para AtribuicaoSetor
            modelBuilder.Entity<AtribuicaoSetor>()
                .HasKey(a => new { a.SetorID, a.FuncionarioID });
        }
    }
}