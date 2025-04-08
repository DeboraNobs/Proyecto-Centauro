
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using proyecto_centauro.Data;
using proyecto_centauro.Interfaces;
using proyecto_centauro.Models;

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
                .Include(c => c.Grupo)
                .ToListAsync(); 
        }

        public async Task<IEnumerable<Coche>> ObtenerCochesFiltrados(int? sucursalId)
        {
            if (sucursalId.HasValue) {
                return await _context.Coches
                .Where(c => c.SucursalId == sucursalId)
                .ToListAsync();
            }

            return await _context.Coches.ToListAsync();
        }
        
        public async Task<Coche> ObtenerCochePorId(int id)
        {
            var coche = await _context.Coches.FindAsync(id);
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
            if (coche != null) {
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