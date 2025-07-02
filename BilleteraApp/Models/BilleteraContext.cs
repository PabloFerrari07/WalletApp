using Microsoft.EntityFrameworkCore;

namespace BilleteraApp.Models
{
    public class BilleteraContext : DbContext
    {

        public BilleteraContext(DbContextOptions<BilleteraContext> options) : base(options)
        {

        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Saldo> Saldos { get; set; }
        public DbSet<Gasto> Gastos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<HistorialSaldo> HistorialSaldos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Usuario — Saldo (1:1)
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Saldo)
                .WithOne(s => s.Usuario)
                .HasForeignKey<Saldo>(s => s.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade); 

            // Usuario — Gastos (1:N)
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Gastos)
                .WithOne(g => g.Usuario)
                .HasForeignKey(g => g.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Usuario — Categorias (1:N)
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Categorias)
                .WithOne(c => c.Usuario)
                .HasForeignKey(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Usuario — HistorialSaldo (1:N)
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.HistorialSaldos)
                .WithOne(h => h.Usuario)
                .HasForeignKey(h => h.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Categoria — Gastos (1:N)
            modelBuilder.Entity<Categoria>()
                .HasMany(c => c.Gastos)
                .WithOne(g => g.Categoria)
                .HasForeignKey(g => g.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict); 
        }



    }
}
