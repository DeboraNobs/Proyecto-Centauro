using Microsoft.EntityFrameworkCore;
using proyecto_centauro.Models;

namespace proyecto_centauro.Data;

 public class BBDDContext : DbContext
    {
        public BBDDContext(DbContextOptions<BBDDContext> options)
             : base(options) 
        {

        }

        /*override protected void OnModelCreating()
        {

        }
        */

        public DbSet<Usuario> Users { get; set; } // la tabla se llama 'Users' en SQLite

    }