using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieDefense.Domain.Entities;

namespace ZombieDefense.Infrasctructure.Data {
    public class AppDbContext : DbContext {

        public DbSet<TipoZombie> TiposZombie { get; set; }

        public DbSet<Zombie> Zombies { get; set; }

        public DbSet<Simulacion> Simulaciones { get; set; }

        public DbSet<ZombieEliminado> ZombiesEliminados { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<TipoZombie>()
                .ToTable("TipoZombie")
                .HasKey(t => t.TipoZombieId);

            modelBuilder.Entity<TipoZombie>()
                .Property(t => t.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Zombie>()
                .ToTable("Zombie")
                .HasKey(t => t.ZombieId);

            modelBuilder.Entity<Zombie>()
                .HasOne(z => z.TipoZombie)
                .WithMany()
                .HasForeignKey(z => z.TipoZombieId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Simulacion>()
                .ToTable("Simulacion")
                .HasKey(t => t.SimulacionId);

            modelBuilder.Entity<ZombieEliminado>()
                .ToTable("ZombieEliminado")
                .HasKey(z => new { z.ZombieId, z.SimulacionId });

            modelBuilder.Entity<ZombieEliminado>()
                .HasOne(z => z.Zombie)
                .WithMany()
                .HasForeignKey(z => z.ZombieId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ZombieEliminado>()
                .HasOne(z => z.Simulacion)
                .WithMany(s => s.ZombiesEliminados) // evitar el mapeo de otro SimulacionId
                .HasForeignKey(z => z.SimulacionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
