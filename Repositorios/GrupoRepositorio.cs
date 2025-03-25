using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proyecto_centauro.Data;
using proyecto_centauro.Interfaces;
using proyecto_centauro.Models;

namespace proyecto_centauro.Repositorios
{
    public class GrupoRepositorio : IGrupoRepositorio
    {
        private readonly BBDDContext _context;

        public GrupoRepositorio(BBDDContext contexto)
        {
            _context = contexto;
        }
        
        public async Task<IEnumerable<Grupo>> ObtenerTodos()
        {
            return await _context.Grupos.ToListAsync();
        }
        public async Task<Grupo> ObtenerGrupoPorId(int id) 
        {
            var grupo = await _context.Grupos.FindAsync(id);
            if (grupo == null) throw new KeyNotFoundException($"No se ha hallado el grupo con id {id}");
            return grupo;
        }
        public async Task AgregarGrupo(Grupo grupo) 
        {
            _context.Grupos.Add(grupo);
            await _context.SaveChangesAsync();
        }
        public async Task ActualizarGrupo(Grupo grupo) 
        {
            _context.Entry(grupo).State = EntityState.Modified;
            await _context.SaveChangesAsync(); 
        }
        public async Task EliminarGrupo(int id)
        {
            var grupo = await _context.Grupos.FindAsync(id);
            if (grupo != null) {
                _context.Grupos.Remove(grupo);
                await _context.SaveChangesAsync(); 
            }
        }

        public async Task<bool> ExisteGrupo(int id)
        {
            return await _context.Grupos.AnyAsync(g => g.Id == id);
        }
    }
}