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
            
            // relacion M:M Servicio - Alquiler con atributo precio
            modelBuilder.Entity<ServicioAlquiler>()
                .HasKey(e => e.Id); // definir clave primaria de la pivot
            // relación 1:M de Alquiler a ServicioAlquiler
            modelBuilder.Entity<ServicioAlquiler>()
                .HasOne(sa => sa.Alquiler)               // un ServicioAlquiler pertenece a un solo Alquiler
                .WithMany(a => a.ServicioAlquileres)     // un Alquiler puede tener muchos ServicioAlquileres
                .HasForeignKey(sa => sa.AlquilerId)      // clave foránea en ServicioAlquiler
                .OnDelete(DeleteBehavior.Cascade);       // si se elimina un Alquiler, se eliminan sus relaciones
            // relación 1:M de Servicio a ServicioAlquiler
            modelBuilder.Entity<ServicioAlquiler>()
                .HasOne(sa => sa.Servicio)               // un ServicioAlquiler pertenece a un solo Servicio
                .WithMany(s => s.ServicioAlquileres)     // un Servicio puede estar en muchos ServicioAlquileres
                .HasForeignKey(sa => sa.ServicioId)      // clave foránea en ServicioAlquiler
                .OnDelete(DeleteBehavior.Cascade);       // si se elimina un Servicio, se eliminan sus relaciones


            base.OnModelCreating(modelBuilder); 
        }

        // aqui se agregan los modelos para que se creen las tablas en SQLite y se definen los nombres de las tablas
        public DbSet<Usuario> Users { get; set; } 
        public DbSet<Alquiler> Alquileres { get; set; } 
        public DbSet<Servicio> Servicios { get; set; }
        public DbSet<ServicioAlquiler> ServicioAlquileres { get; set; }

    }