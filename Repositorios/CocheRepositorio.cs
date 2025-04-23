
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
                    // Imagen = c.Imagen,
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

        /*
                public async Task<IEnumerable<Coche>> ObtenerCochesFiltradosBySucursalId(int? sucursalId)
                {
                    if (sucursalId.HasValue)
                    {
                        return await _context.Coches
                            .Where(c => c.SucursalId == sucursalId)
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
                                Grupo = c.Grupo == null ? null : new Grupo
                                {
                                    Id = c.Grupo.Id,
                                    Nombre = c.Grupo.Nombre,
                                    Precio = c.Grupo.Precio,
                                    Descripcion = c.Grupo.Descripcion
                                },
                                Sucursal = c.Sucursal == null ? null : new Sucursal
                                {
                                    Id = c.Sucursal.Id,
                                    Nombre = c.Sucursal.Nombre,
                                }
                            })
                            .ToListAsync();
                    }
                    return await _context.Coches.ToListAsync();
                }
        */






        /*  funciona *************************************************************
                public async Task<IEnumerable<Coche>> ObtenerCochesFiltrados(int? sucursalId, DateTime? fechainicio, DateTime? fechaFin)
                {
                    if (!sucursalId.HasValue || !fechainicio.HasValue || !fechaFin.HasValue)
                        return [];

                    // Paso 1: Traer todos los coches de la sucursal
                    var cochesSucursal = await _context.Coches
                        .Where(c => c.SucursalId == sucursalId)

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
                                // Imagen = c.Imagen,
                                Grupo = c.Grupo == null ? null : new Grupo
                                {
                                    Id = c.Grupo.Id,
                                    Nombre = c.Grupo.Nombre,
                                    Precio = c.Grupo.Precio,
                                    Descripcion = c.Grupo.Descripcion
                                },
                                Sucursal = c.Sucursal == null ? null : new Sucursal
                                {
                                    Id = c.Sucursal.Id,
                                    Nombre = c.Sucursal.Nombre,
                                }
                            })

                        .ToListAsync();

                    // Paso 2: Agrupar coches por grupo
                    var cochesPorGrupo = cochesSucursal
                        .Where(c => c.GrupoId.HasValue)
                        .GroupBy(c => c.GrupoId!.Value)
                        .ToDictionary(g => g.Key, g => g.ToList());

                    // Paso 3: Contar alquileres activos por grupo para ese rango de fechas
                    var alquileresEnRango = await _context.Alquileres
                        .Where(a =>
                            a.Fechainicio <= fechaFin &&
                            a.FechaFin >= fechainicio
                        )
                        .GroupBy(a => a.GrupoId)
                        .Select(g => new { GrupoId = g.Key, Cantidad = g.Count() })
                        .ToListAsync();

                    var alquileresPorGrupo = alquileresEnRango
                        .Where(a => a.GrupoId != null)
                        .ToDictionary(a => a.GrupoId!.Value, a => a.Cantidad);

                    // Paso 4: Filtrar coches de grupos donde haya disponibilidad
                    var cochesDisponibles = new List<Coche>();

                    foreach (var grupo in cochesPorGrupo)
                    {
                        var grupoId = grupo.Key;
                        var coches = grupo.Value;
                        var totalCoches = coches.Count;
                        var alquileres = alquileresPorGrupo.TryGetValue(grupoId, out int value) ? value : 0;

                        var disponibles = totalCoches - alquileres;

                        if (disponibles > 0)
                        {
                            // Agregar solo la cantidad disponible
                            cochesDisponibles.AddRange(coches.Take(disponibles));
                        }
                    }

                    return cochesDisponibles;
                }
        */




        public async Task<IEnumerable<CocheDisponibilidadDTO>> ObtenerCochesFiltrados(int? sucursalId, DateTime? fechainicio, DateTime? fechaFin)
        {
            if (!sucursalId.HasValue || !fechainicio.HasValue || !fechaFin.HasValue)
                return [];

            // Paso 1: Traer todos los coches de la sucursal
            var cochesSucursal = await _context.Coches
                .Where(c => c.SucursalId == sucursalId)
                .Select(c => new CocheDisponibilidadDTO
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
                    Grupo = c.Grupo == null ? null : new GrupoDTO
                    {
                        Id = c.Grupo.Id,
                        Nombre = c.Grupo.Nombre,
                    },
                    Sucursal = c.Sucursal == null ? null : new SucursalDTO
                    {
                        Id = c.Sucursal.Id,
                        Nombre = c.Sucursal.Nombre,
                    }
                })
                .ToListAsync();

            // Paso 2: Agrupar coches por grupo
            var cochesPorGrupo = cochesSucursal
                .Where(c => c.Grupo != null)
                .GroupBy(c => c.Grupo!.Id)
                .ToDictionary(g => g.Key, g => g.ToList());

            // Paso 3: Contar alquileres activos por grupo para ese rango de fechas
            var alquileresEnRango = await _context.Alquileres
                .Where(a =>
                    a.Fechainicio <= fechaFin &&
                    a.FechaFin >= fechainicio
                )
                .GroupBy(a => a.GrupoId)
                .Select(g => new { GrupoId = g.Key, Cantidad = g.Count() })
                .ToListAsync();

            var alquileresPorGrupo = alquileresEnRango
                .Where(a => a.GrupoId != null)
                .ToDictionary(a => a.GrupoId!.Value, a => a.Cantidad);

            // Paso 4: Filtrar coches de grupos donde haya disponibilidad
            var cochesDisponibles = new List<CocheDisponibilidadDTO>();

            foreach (var grupo in cochesPorGrupo)
            {
                var grupoId = grupo.Key;
                var coches = grupo.Value;
                var totalCoches = coches.Count;
                var alquileres = alquileresPorGrupo.TryGetValue(grupoId, out int value) ? value : 0;

                var disponibles = totalCoches - alquileres;

                if (disponibles > 0)
                {
                    cochesDisponibles.AddRange(coches.Take(disponibles));
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
            /*var c = await _context.Coches.FindAsync(coche.Id);
            c.Marca = coche.Marca;
            c.Sucursal = coche.Sucursal;
            */
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