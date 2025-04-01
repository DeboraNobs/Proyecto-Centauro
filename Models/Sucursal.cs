using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace proyecto_centauro.Models
{
    public class Sucursal
    {
        [Key]
        public int Id { get; set;}
        [Required]
        public string? Nombre { get; set;}
        
        public ICollection<Coche>? Coches { get; set;}
    }
}