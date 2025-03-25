using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proyecto_centauro.Data;
using proyecto_centauro.Interfaces;
using proyecto_centauro.Models;
using proyecto_centauro.Repositorios;

namespace proyecto_centauro.Controllers
{
    [Route("api/grupo")]
    [ApiController]
    public class GrupoController : Controller
    {
        private readonly IGrupoRepositorio _grupoRepositorio;

        public GrupoController(IGrupoRepositorio grupoRepositorio)
        {
            _grupoRepositorio = grupoRepositorio;
        }

        [HttpGet]
        public async Task <ActionResult<IEnumerable<Grupo>>> GetGrupos()
        {
            var grupos = await _grupoRepositorio.ObtenerTodos();
            return Json(grupos);
        }
        
        [HttpGet("{id}")]
         public async Task <ActionResult<Grupo>> GetGrupoById(int id)
        {
            var grupo = await _grupoRepositorio.ObtenerGrupoPorId(id);
            if (grupo == null) return NotFound();
            return Json(grupo);
        }

        [HttpPost]
        public async Task<ActionResult<Grupo>> CrearGrupo([FromBody] Grupo grupo)
        {
            if (grupo == null) return BadRequest("EL GRUPO no puede ser nulo");
            await _grupoRepositorio.AgregarGrupo(grupo);
            return StatusCode(201, grupo);
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult<Grupo>> EditarGrupo(int id, [FromBody] Grupo grupo) 
        {
            if (id != grupo.Id) return BadRequest();
            await _grupoRepositorio.ActualizarGrupo(grupo);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> BorrarGrupo(int id) 
        {
            var existeGrupo = await _grupoRepositorio.ExisteGrupo(id);
            if (!existeGrupo) return NotFound();

            await _grupoRepositorio.EliminarGrupo(id);
            return NoContent();
        }
    }
}
