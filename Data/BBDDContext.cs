using Microsoft.EntityFrameworkCore;
using proyecto_centauro.Models;

namespace proyecto_centauro.Data;

 public class BBDDContext : DbContext
    {
        public BBDDContext(DbContextOptions<BBDDContext> options)
             : base(options) 
        {

        }

        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            // relación entre Usuario y Alquiler
            modelBuilder.Entity<Alquiler>()
                .HasOne(a => a.Usuario)
                .WithMany(a => a.Alquileres)
                .HasForeignKey(a => a.UsersId)
                .OnDelete(DeleteBehavior.Cascade);
            base.OnModelCreating(modelBuilder); // llamamos a la implementación base para aplicar configuraciones por defecto
        }

        public DbSet<Usuario> Users { get; set; } // la tabla se llama 'Users' en SQLite
        public DbSet<Alquiler> Alquileres { get; set; } 
    }