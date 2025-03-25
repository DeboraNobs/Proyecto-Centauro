using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace proyecto_centauro.Models
{
    [Table("GrupoAlquiler")]
    public class GrupoAlquiler
    {
        [Key]
        public int Id { get; set; }

        // claves foráneas
        public int AlquilerId { get; set; }
        public int GrupoId { get; set; }

        // atributos de navegacion
        public Alquiler? Alquiler { get; set; }
        public Grupo? Grupo { get; set; }

        // atributo adicional de la tabla pivot
        public decimal Precio { get; set; }
    }
}