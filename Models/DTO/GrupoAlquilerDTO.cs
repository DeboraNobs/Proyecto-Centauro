using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace proyecto_centauro.Models.DTO
{
    public class GrupoAlquilerDTO
    {
        public int AlquilerId { get; set; }
        public int GrupoId { get; set; }

        [RegularExpression(@"^\d+(\.\d+)?$",
           ErrorMessage = "El precio deben ser números positivos.")]
        public decimal Precio { get; set; }
    }
}