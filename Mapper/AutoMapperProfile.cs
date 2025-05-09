using AutoMapper;
using proyecto_centauro.Models;
using proyecto_centauro.Models.DTO;

namespace proyecto_centauro.Mapper
{
    public class AutoMapperProfile : Profile // Profile tiene todas las características que AutoMapper maneja
    {
        public AutoMapperProfile() 
        {
            // ¿Estás leyendo datos de la base y los vas a mostrar? → Entidad → DTO
            // ¿Estás recibiendo datos del cliente y los vas a guardar? → DTO → Entidad

            // CreateMap<Origen, Destino>();
            CreateMap<Coche, CocheDisponibilidadDTO>(); // mapeo un Coche, conviertelo a un CocheDisponibilidadDTO
            CreateMap<Grupo, GrupoDTO>();
            CreateMap<Sucursal, SucursalDTO>();

            CreateMap<CocheDTO, Coche>();
            CreateMap<Coche, CocheDTO>();

            CreateMap<Alquiler, AlquilerDTO>(); // para obtener todos los alquileres

            CreateMap<Grupo, GrupoDTO>();
        }
    }
}
