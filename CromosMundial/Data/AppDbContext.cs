using CromosMundial.Models;
using Microsoft.EntityFrameworkCore;

namespace CromosMundial.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Pais> Paises { get; set; }
        public DbSet<Equipo> Equipos { get; set; }
        public DbSet<Jugador> Jugadores { get; set; }
        public DbSet<Cromo> Cromos { get; set; }
        public DbSet<Album> Albumes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // sin esto SQL Server tira error por el doble cascade (cromo tiene 2 FK)
            modelBuilder.Entity<Cromo>()
                .HasOne(c => c.Jugador)
                .WithMany(j => j.Cromos)
                .HasForeignKey(c => c.JugadorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cromo>()
                .HasOne(c => c.Album)
                .WithMany(a => a.Cromos)
                .HasForeignKey(c => c.AlbumId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
