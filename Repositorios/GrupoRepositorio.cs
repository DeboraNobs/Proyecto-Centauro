using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proyecto_centauro.Data;
using proyecto_centauro.Interfaces;
using proyecto_centauro.Models;
using proyecto_centauro.Models.DTO;

namespace proyecto_centauro.Repositorios
{
    public class GrupoRepositorio : IGrupoRepositorio
    {
        private readonly BBDDContext _context;
        private readonly IMapper _mapper;
        
        public GrupoRepositorio(BBDDContext contexto, IMapper mapper)
        {
            _context = contexto;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<GrupoDTO>> ObtenerTodos()
        {
            var grupos =  await _context.Grupos.ToListAsync();
            var gruposDTO = _mapper.Map<List<GrupoDTO>>(grupos);
            return gruposDTO;
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