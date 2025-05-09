
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using proyecto_centauro.Data;
using proyecto_centauro.Interfaces;
using proyecto_centauro.Models;
using proyecto_centauro.Models.DTO;

namespace proyecto_centauro.Repositorios
{
    public class CocheRepositorio : ICocheRepositorio
    {
        private readonly BBDDContext _context;
        private readonly IMapper _mapper;

        public CocheRepositorio(BBDDContext contexto, IMapper mapper)
        {
            _context = contexto;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CocheDTO>> ObtenerTodosCoches()
        {
            var coches = await _context.Coches.ToListAsync();
            var cochesDTO = _mapper.Map<List<CocheDTO>>(coches);
            return cochesDTO;
        }


        public async Task<IEnumerable<Coche>> ObtenerTodosCochesConRelacionGrupo()
        {

            var coches = await _context.Coches
                .Include(c => c.Grupo)
                .Include(c => c.Sucursal)
                .ToListAsync();

            var cochesDTO = _mapper.Map<List<Coche>>(coches);
            return cochesDTO;


            /*
            return await _context.Coches

                .Select(c => new Coche
                {
                    Id = c.Id,
                    Marca = c.Marca,
                    Modelo = c.Modelo,
                    Descripcion = c.Descripcion,
                    Patente = c.Patente,
                    Tipo_coche = c.Tipo_coche,
                    Tipo_cambio = c.Tipo_cambio,
                    Num_plazas = c.Num_plazas,
                    Num_maletas = c.Num_maletas,
                    Num_puertas = c.Num_puertas,
                    Posee_aire_acondicionado = c.Posee_aire_acondicionado,
                    GrupoId = c.GrupoId,
                    SucursalId = c.SucursalId,
                    Imagen = c.Imagen,
                    Grupo = c.Grupo == null ? null : new Grupo // gracias a crear un nuevo grupo se devuelven bien los datos 
                    {
                        Id = c.Grupo.Id,
                        Nombre = c.Grupo.Nombre,
                        Precio = c.Grupo.Precio,
                        Descripcion = c.Grupo.Descripcion,
                    },
                    Sucursal = c.Sucursal == null ? null : new Sucursal
                    {
                        Id = c.Sucursal.Id,
                        Nombre = c.Sucursal.Nombre,
                    }
                })
                .OrderBy(s => s.Sucursal!.Id)
                .ToListAsync();
            */
        }


        public async Task<IEnumerable<CocheDisponibilidadDTO>> ObtenerCochesFiltrados
            (
                int? sucursalId, DateTime? fechainicio, DateTime? fechaFin, TimeSpan? horarioRecogida, TimeSpan? horarioDevolucion
            )
        {
            if (!sucursalId.HasValue || !fechainicio.HasValue || !fechaFin.HasValue || !horarioRecogida.HasValue || !horarioDevolucion.HasValue) // si falta un parámetro de búsqueda devuelve una lista vacía
                return [];

            var cochesSucursal = await _context.Coches
                .Include(c => c.Grupo)
                .Include(c => c.Sucursal)
                .Where(c => c.SucursalId == sucursalId)
                .ToListAsync();

            // aqui le digo: convierteme el objeto que te paso por (parametro) en <esteDTO> : Map<TDestino>(objetoOrigen)
            var cochesDTO = _mapper.Map<List<CocheDisponibilidadDTO>>(cochesSucursal); // aqui lo que hace es crear un objeto de tipo CocheDTO para cada coche,
                                                                                       // agregando todas las propiedades que coinciden en Coche y CocheDTO.           

            // agrupo coches por grupo
            var cochesPorGrupo = cochesDTO
                .Where(c => c.Grupo != null)
                .GroupBy(c => c.Grupo!.Id)
                .ToDictionary(g => g.Key, g => g.ToList());

            // cuento alquileres activos por grupo para ese rango de fechas
            var alquileres = await _context.Alquileres
                .Where(a =>
                    a.Fechainicio <= fechaFin &&
                    a.FechaFin >= fechainicio)
                .ToListAsync();

            var alquileresEnRango = alquileres
                .Where(a =>
                    a.Fechainicio <= fechaFin &&
                    a.FechaFin >= fechainicio &&
                    a.HorarioRecogida < horarioDevolucion && // inicio del alquiler existente es antes del fin de la solicitud
                    a.HorarioDevolucion > horarioRecogida)   // fin del alquiler existente es después del inicio de la solicitud
                .GroupBy(a => a.GrupoId)
                .Select(g => new { GrupoId = g.Key, Cantidad = g.Count() })
                .ToList();

            var alquileresPorGrupo = alquileresEnRango // obtengo la cantidad de alquileres activos por grupo
                .Where(a => a.GrupoId != null)
                .ToDictionary(a => a.GrupoId!.Value, a => a.Cantidad);

            // filtro coches de grupos donde haya disponibilidad
            var cochesDisponibles = new List<CocheDisponibilidadDTO>();
            foreach (var grupo in cochesPorGrupo)
            {
                var grupoId = grupo.Key;
                var listaCoches = grupo.Value; // lista de coches de un determinado grupo

                var totalCoches = listaCoches.Count;
                var cochesAlquilados = alquileresPorGrupo.TryGetValue(grupoId, out int value) ? value : 0;

                var disponibles = totalCoches - cochesAlquilados;
                if (disponibles > 0)
                {
                    for (int i = 0; i < disponibles && i < listaCoches.Count; i++)
                    {
                        cochesDisponibles.Add(listaCoches[i]);
                    }
                }
            }
            return cochesDisponibles;
        }


        public async Task<Coche> ObtenerCochePorId(int id)
        {
            var coche = await _context.Coches
                .Include(c => c.Grupo) // muestra todos los datos de los grupos relacionados, permite acceder a Grupo.precio
                .Include(a => a.Sucursal)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (coche == null) throw new KeyNotFoundException($"No se encontró un coche con id {id}");

            return coche;
        }


        public async Task AgregarCoche(CocheDTO cocheDTO)
        {
            var coche = _mapper.Map<Coche>(cocheDTO); // CocheDTO → Coche
            _context.Coches.Add(coche);
            await _context.SaveChangesAsync();
        }
        public async Task ActualizarCoche(Coche coche)
        {
            _context.Entry(coche).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task EliminarCoche(int id)
        {
            var coche = await _context.Coches.FindAsync(id);
            if (coche != null)
            {
                _context.Coches.Remove(coche);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> ExisteCoche(int id)
        {
            return await _context.Coches.AnyAsync(a => a.Id == id);
        }
    }
}







/* COCHE CONTROLLER SIN USAR AUTOMAPPER

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using proyecto_centauro.Data;
using proyecto_centauro.Interfaces;
using proyecto_centauro.Models;
using proyecto_centauro.Models.DTO;

namespace proyecto_centauro.Repositorios
{
    public class CocheRepositorio : ICocheRepositorio
    {
        private readonly BBDDContext _context;

        public CocheRepositorio(BBDDContext contexto)
        {
            _context = contexto;
        }

        public async Task<IEnumerable<Coche>> ObtenerTodosCoches()
        {
            return await _context.Coches.ToListAsync();
        }


        public async Task<IEnumerable<Coche>> ObtenerTodosCochesConRelacionGrupo()
        {
            return await _context.Coches

                .Select(c => new Coche
                {
                    Id = c.Id,
                    Marca = c.Marca,
                    Modelo = c.Modelo,
                    Descripcion = c.Descripcion,
                    Patente = c.Patente,
                    Tipo_coche = c.Tipo_coche,
                    Tipo_cambio = c.Tipo_cambio,
                    Num_plazas = c.Num_plazas,
                    Num_maletas = c.Num_maletas,
                    Num_puertas = c.Num_puertas,
                    Posee_aire_acondicionado = c.Posee_aire_acondicionado,
                    GrupoId = c.GrupoId,
                    SucursalId = c.SucursalId,
                    Imagen = c.Imagen,
                    Grupo = c.Grupo == null ? null : new Grupo // gracias a crear un nuevo grupo se devuelven bien los datos 
                    {
                        Id = c.Grupo.Id,
                        Nombre = c.Grupo.Nombre,
                        Precio = c.Grupo.Precio,
                        Descripcion = c.Grupo.Descripcion,
                    },
                    Sucursal = c.Sucursal == null ? null : new Sucursal
                    {
                        Id = c.Sucursal.Id,
                        Nombre = c.Sucursal.Nombre,
                    }
                })
                .OrderBy(s => s.Sucursal!.Id)
                .ToListAsync();
        }


        public async Task<IEnumerable<CocheDisponibilidadDTO>> ObtenerCochesFiltrados
            (
                int? sucursalId, DateTime? fechainicio, DateTime? fechaFin, TimeSpan? horarioRecogida, TimeSpan? horarioDevolucion
            )
        {
            if (!sucursalId.HasValue || !fechainicio.HasValue || !fechaFin.HasValue || !horarioRecogida.HasValue || !horarioDevolucion.HasValue) // si falta un parámetro de búsqueda devuelve una lista vacía
                return [];

            // obtengo todos los coches de la sucursal indicada
            var cochesSucursal = await _context.Coches
                .Where(c => c.SucursalId == sucursalId)
                .Select(c => new CocheDisponibilidadDTO // transformo a un DTO con toda la info necesaria para mostrar.
                {
                    Id = c.Id,
                    Marca = c.Marca,
                    Modelo = c.Modelo,
                    Descripcion = c.Descripcion,
                    Patente = c.Patente,
                    Tipo_coche = c.Tipo_coche,
                    Tipo_cambio = c.Tipo_cambio,
                    Num_plazas = c.Num_plazas,
                    Num_maletas = c.Num_maletas,
                    Num_puertas = c.Num_puertas,
                    Imagen = c.Imagen,
                    Posee_aire_acondicionado = c.Posee_aire_acondicionado,
                    Grupo = c.Grupo == null ? null : new GrupoDTO
                    {
                        Id = c.Grupo.Id,
                        Nombre = c.Grupo.Nombre,
                        Precio = c.Grupo.Precio
                    },
                    Sucursal = c.Sucursal == null ? null : new SucursalDTO
                    {
                        Id = c.Sucursal.Id,
                        Nombre = c.Sucursal.Nombre,
                    }
                })
                .ToListAsync();

            // cuento alquileres activos por grupo para ese rango de fechas
            var alquileres = await _context.Alquileres
                .Where(a =>
                    a.Fechainicio <= fechaFin &&
                    a.FechaFin >= fechainicio)
                .ToListAsync();

            var alquileresEnRango = alquileres
                .Where(a =>
                    a.Fechainicio <= fechaFin &&
                    a.FechaFin >= fechainicio &&
                    a.HorarioRecogida < horarioDevolucion && // inicio del alquiler existente es antes del fin de la solicitud
                    a.HorarioDevolucion > horarioRecogida)   // fin del alquiler existente es después del inicio de la solicitud
                .GroupBy(a => a.GrupoId)
                .Select(g => new { GrupoId = g.Key, Cantidad = g.Count() })
                .ToList();

            var alquileresPorGrupo = alquileresEnRango // obtengo la cantidad de alquileres activos por grupo
                .Where(a => a.GrupoId != null)
                .ToDictionary(a => a.GrupoId!.Value, a => a.Cantidad);

            // agrupo coches por grupo
            var cochesPorGrupo = cochesSucursal
                .Where(c => c.Grupo != null)
                .GroupBy(c => c.Grupo!.Id)
                .ToDictionary(g => g.Key, g => g.ToList());

            // filtro coches de grupos donde haya disponibilidad
            var cochesDisponibles = new List<CocheDisponibilidadDTO>();
            foreach (var grupo in cochesPorGrupo)
            {
                var grupoId = grupo.Key;
                var listaCoches = grupo.Value; // lista de coches de un determinado grupo

                var totalCoches = listaCoches.Count;
                var cochesAlquilados = alquileresPorGrupo.TryGetValue(grupoId, out int value) ? value : 0;

                var disponibles = totalCoches - cochesAlquilados;
                if (disponibles > 0)
                {
                    for (int i = 0; i < disponibles && i < listaCoches.Count; i++)
                    {
                        cochesDisponibles.Add(listaCoches[i]);
                    }
                }
            }
            return cochesDisponibles;
        }


        public async Task<Coche> ObtenerCochePorId(int id)
        {
            var coche = await _context.Coches
                .Include(c => c.Grupo) // muestra todos los datos de los grupos relacionados, permite acceder a Grupo.precio
                .Include(a => a.Sucursal)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (coche == null) throw new KeyNotFoundException($"No se encontró un coche con id {id}");

            return coche;
        }


        public async Task AgregarCoche(Coche coche)
        {
            _context.Coches.Add(coche);
            await _context.SaveChangesAsync();
        }
        public async Task ActualizarCoche(Coche coche)
        {
            _context.Entry(coche).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task EliminarCoche(int id)
        {
            var coche = await _context.Coches.FindAsync(id);
            if (coche != null)
            {
                _context.Coches.Remove(coche);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> ExisteCoche(int id)
        {
            return await _context.Coches.AnyAsync(a => a.Id == id);
        }
    }
} */