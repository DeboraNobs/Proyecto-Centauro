
namespace proyecto_centauro.Models.DTO
{
   public class CocheDisponibilidadDTO {
     public int Id { get; set;}
     public string? Marca { get; set;}
     public string? Modelo { get; set;}
     public string? Descripcion { get; set;}
     public string? Patente { get; set;}
     public string? Tipo_coche { get; set;}
     public string? Tipo_cambio { get; set;}
     public int Num_plazas { get; set;}
     public int Num_maletas { get; set;}
     public int Num_puertas { get; set;}
     public bool Posee_aire_acondicionado { get; set;}
     public int? GrupoId { get; set;}
     public int? SucursalId { get; set;}
     public string? Imagen { get; set;}

      public GrupoDTO? Grupo { get; set; }
      public SucursalDTO? Sucursal { get; set; }
    }
}
