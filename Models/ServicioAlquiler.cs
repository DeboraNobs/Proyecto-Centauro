using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace proyecto_centauro.Models
{ 
    [Table("ServicioAlquiler")]
    public class ServicioAlquiler
    {
        [Key]
        public int Id { get; set; }

        // claves foráneas
        public int AlquilerId { get; set; }
        public int ServicioId { get; set; }

        // atributos de navegacion
        public Alquiler? Alquiler { get; set; }
        public Servicio? Servicio { get; set; }

        // atributo adicional de la tabla pivot
        public decimal Precio { get; set; }
    }
}