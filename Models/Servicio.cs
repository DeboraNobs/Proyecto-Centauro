using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace proyecto_centauro.Models
{
    [Table("Servicios")]
    public class Servicio
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public required string Nombre { get; set; }
        [Required]
        public decimal Precio { get; set; }

        public decimal? PorcentajeDescuento { get; set; }
    
        // la lista representa la relación M:M con Alquileres a través de la tabla intermedia
        public List<ServicioAlquiler> ServicioAlquileres { get; set; } = [];

    }
}