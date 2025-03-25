using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proyecto_centauro.Data;
using proyecto_centauro.Interfaces;
using proyecto_centauro.Models;

namespace proyecto_centauro.Repositorios
{
    public class ServicioRepositorio : IServicioRepositorio
    {
        private readonly BBDDContext _context;

        public ServicioRepositorio(BBDDContext contexto)
        {
            _context = contexto;
        }
        public async Task<IEnumerable<Servicio>> ObtenerTodosServicios()
        {
            return await _context.Servicios.ToListAsync();
        }

        public async Task<Servicio> ObtenerServicioPorId(int id)
        {
            var servicio = await _context.Servicios.FindAsync(id);
            if (servicio == null) throw new KeyNotFoundException($"No se ha encontrado un servicio con id: {id}");
            return servicio;
        }
  
        public async Task AgregarServicio(Servicio servicio)
        {
            _context.Servicios.Add(servicio);
            await _context.SaveChangesAsync();
        }
        public Task ActualizarServicio(Servicio servicio)
        {
            _context.Entry(servicio).State = EntityState.Modified;
            return _context.SaveChangesAsync();
        }

        public async Task EliminarServicio(int id)
        {
            var servicio = await _context.Servicios.FindAsync(id);
            if (servicio != null) {
                _context.Servicios.Remove(servicio);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExisteServicio(int id)
        {
            return await _context.Servicios.AnyAsync(e => e.Id == id);
        }
    }
}