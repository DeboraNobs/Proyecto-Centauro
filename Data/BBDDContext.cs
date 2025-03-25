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
            // relación 1:M entre Usuario y Alquiler
            modelBuilder.Entity<Alquiler>()
                .HasOne(a => a.Usuario)
                .WithMany(a => a.Alquileres)
                .HasForeignKey(a => a.UsersId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // relacion M:M Servicio - Alquiler con atributo precio
            modelBuilder.Entity<ServicioAlquiler>()
                .HasKey(e => e.Id); // definir clave primaria de la pivot
            modelBuilder.Entity<ServicioAlquiler>() // relación 1:M de Alquiler con ServicioAlquiler
                .HasOne(s => s.Alquiler)               // un ServicioAlquiler pertenece a un solo Alquiler
                .WithMany(a => a.ServicioAlquileres)     // un Alquiler puede tener muchos ServicioAlquileres
                .HasForeignKey(s => s.AlquilerId).OnDelete(DeleteBehavior.Cascade);  
            modelBuilder.Entity<ServicioAlquiler>() // relación 1:M de Servicio con ServicioAlquiler
                .HasOne(s => s.Servicio)               // un ServicioAlquiler pertenece a un solo Servicio
                .WithMany(s => s.ServicioAlquileres)     // un Servicio puede estar en muchos ServicioAlquileres
                .HasForeignKey(s => s.ServicioId).OnDelete(DeleteBehavior.Cascade);      

            // relacion M:M Grupo - Alquiler con atributo precio
            modelBuilder.Entity<GrupoAlquiler>()
                .HasKey(h => h.Id);
            modelBuilder.Entity<GrupoAlquiler>() // relacion 1:M Alquiler con GrupoAlquiler
                .HasOne(g => g.Alquiler).WithMany(g => g.GrupoAlquileres).HasForeignKey(g => g.AlquilerId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<GrupoAlquiler>() // relacion 1:M Grupo con GrupoAlquiler
                .HasOne(a => a.Grupo).WithMany(a => a.GrupoAlquileres).HasForeignKey(a => a.GrupoId).OnDelete(DeleteBehavior.Cascade);

            // relación 1:M entre Grupo y Coche
            modelBuilder.Entity<Coche>()
                .HasOne(a => a.Grupo)
                .WithMany(a => a.Coches)
                .HasForeignKey(a => a.GrupoId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder); 
        }

        // aqui se agregan los modelos para que se creen las tablas en SQLite y se definen los nombres de las tablas
        public DbSet<Usuario> Users { get; set; } 
        public DbSet<Alquiler> Alquileres { get; set; } 
        public DbSet<Servicio> Servicios { get; set; }
        public DbSet<ServicioAlquiler> ServicioAlquileres { get; set; }
        public DbSet<Grupo> Grupos { get; set; }
        public DbSet<GrupoAlquiler> GruposAlquileres { get; set;}
        public DbSet<Coche> Coches { get; set; }
    }