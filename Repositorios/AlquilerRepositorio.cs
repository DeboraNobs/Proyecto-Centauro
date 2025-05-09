using AutoMapper;
using Microsoft.EntityFrameworkCore;
using proyecto_centauro.Data;
using proyecto_centauro.Interfaces;
using proyecto_centauro.Models;

namespace proyecto_centauro.Repositorios
{
    public class AlquilerRepositorio : IAlquilerRepositorio
    {
        private readonly BBDDContext _context;
        private readonly IMapper _mapper;
        
        public AlquilerRepositorio(BBDDContext contexto, IMapper mapper)
        {
            _context = contexto;
            _mapper = mapper; 
        }

        public async Task<IEnumerable<AlquilerDTO>> ObtenerTodos()
        {
            var alquileres = await _context.Alquileres
                                            .OrderBy(a => a.Fechainicio)
                                            .ToListAsync();

            var alquileresDTOS = _mapper.Map<List<AlquilerDTO>>(alquileres);
            return alquileresDTOS;
        }
        public async Task<Alquiler> ObtenerAlquilerPorId(int id)
        {
            var alquiler = await _context.Alquileres.FindAsync(id);
            if (alquiler == null) throw new KeyNotFoundException($"No se ha encontrado el alquiler con el id {id}");
            return alquiler;
        }


        public async Task AgregarAlquiler(Alquiler alquiler)
        {
            // primero debo asegurarme de que el Usuario relacionado con el UsersId existe antes de agregar el alquiler
            var usuario = await _context.Users.FindAsync(alquiler.UsersId);
            if (usuario == null) throw new KeyNotFoundException($"No se ha encontrado un usuario con id: {alquiler.UsersId}");

            _context.Alquileres.Add(alquiler);
            await _context.SaveChangesAsync();
        }

        /* tal vez pueda usar esta funcion para cuando se alquile desde fleet que no se selecciona fecha ni hora

                public async Task AgregarAlquiler(Alquiler alquiler)
                {
                    var usuario = await _context.Users.FindAsync(alquiler.UsersId) ?? throw new KeyNotFoundException($"No se ha encontrado un usuario con id: {alquiler.UsersId}");

                    // cantidad coches hay en el grupo
                    var totalCochesEnGrupo = await _context.Coches
                        .Where(c => c.GrupoId == alquiler.GrupoId)
                        .CountAsync();

                    // cantidad alquileres activos hay en el grupo
                    var alquileresActivos = await _context.Alquileres
                        .Where(a => a.GrupoId == alquiler.GrupoId && a.FechaFin > DateTime.Now) // mejorar la logica (tiene que ver si hay alguna reserva para mostrar, si hay un alquiler que este dentro de fechainicio-fecha fin no esta disponible el coche)
                        .CountAsync();

                    var cochesDisponibles = totalCochesEnGrupo - alquileresActivos;

                    if (cochesDisponibles <= 0) throw new InvalidOperationException("No hay coches disponibles en este grupo para alquilar.");

                    _context.Alquileres.Add(alquiler);
                    await _context.SaveChangesAsync();
                }
        */

        public async Task ActualizarAlquiler(Alquiler alquiler)
        {
            _context.Entry(alquiler).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAlquiler(int id)
        {
            var alquiler = await _context.Alquileres.FindAsync(id);
            if (alquiler != null)
            {
                _context.Alquileres.Remove(alquiler);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExisteAlquiler(int id)
        {
            return await _context.Alquileres.AnyAsync(e => e.Id == id);
        }
    }
}