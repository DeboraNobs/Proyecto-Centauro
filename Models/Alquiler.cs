using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace proyecto_centauro.Models
{
    [Table("Alquileres")]
    public class Alquiler
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime Fechainicio { get; set; }
        [Required]
        public DateTime FechaFin { get; set; }
        [Required]
        public string? LugarRecogida { get; set; } = string.Empty;
        [Required]
        public string? LugarDevolucion { get; set; } = string.Empty;
        [Required]
        public TimeSpan HorarioRecogida {get; set; }
        [Required]
        public TimeSpan HorarioDevolucion {get; set; }

        // FOREIGN KEY - USUARIOS
        public int? UsersId { get; set; }
        public Usuario? Usuario { get; set; } // Propiedad de navegación, para acceder a todos los atributos del usuario. Ej --> Usuario.Email
       
       // FOREIGN KEY - GRUPOS
       public int? GrupoId { get; set; }
       public Grupo? Grupo { get; set; }
       // refleja la relación M:M con Servicios a través de la tabla intermedia
       public List<ServicioAlquiler> ServicioAlquileres { get; set; } = [];

    }
}