
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
                    }
                })
                .ToListAsync();
        }

        /*
        public async Task<IEnumerable<Coche>> ObtenerCochesFiltrados(int? sucursalId)
        {
            if (sucursalId.HasValue)
            {
                return await _context.Coches
                .Where(c => c.SucursalId == sucursalId)
                .Include(c => c.Grupo)
                .Include(a => a.Sucursal)
                .ToListAsync();
            }

            return await _context.Coches.ToListAsync();
        }
*/

        public async Task<IEnumerable<Coche>> ObtenerCochesFiltrados(int? sucursalId)
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