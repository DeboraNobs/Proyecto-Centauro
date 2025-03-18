using Microsoft.EntityFrameworkCore;
using proyecto_centauro.Models;

namespace proyecto_centauro.Data;

 public class UsuarioContext : DbContext
    {
        public UsuarioContext(DbContextOptions<UsuarioContext> options)
             : base(options) 
        {

        }

        public DbSet<Usuario> Users { get; set; } // La tabla se llama 'Users' en SQLite

    }