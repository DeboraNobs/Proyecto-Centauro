using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proyecto_centauro.Data;
using proyecto_centauro.Models;
using proyecto_centauro.Models.DTO;

namespace proyecto_centauro.Controllers.TablasPivote
{
    [Route("api/grupo-alquiler")]
    [ApiController]
    public class GrupoAlquilerController : Controller
    {
        private readonly BBDDContext _context;

        public GrupoAlquilerController(BBDDContext contexto)
        {
            _context = contexto;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GrupoAlquiler>>> GetGruposAlquileres() 
        {
            var gruposAlquileres = await _context.GruposAlquileres.ToListAsync();
            return Ok(gruposAlquileres);
        }

        [HttpPost]
        public async Task<ActionResult> CrearGrupoAlquiler([FromBody] GrupoAlquilerDTO grupoAlquilerDTO)
        {
            var grupo = await _context.Grupos.FindAsync(grupoAlquilerDTO.GrupoId);
            var alquiler = await _context.Alquileres.FindAsync(grupoAlquilerDTO.AlquilerId);

            if (grupo == null || alquiler == null) return NotFound();

            var grupoAlquiler = new GrupoAlquiler
            {
                AlquilerId = grupoAlquilerDTO.AlquilerId,
                GrupoId = grupoAlquilerDTO.GrupoId,
                Precio = grupoAlquilerDTO.Precio
            };

            _context.GruposAlquileres.Add(grupoAlquiler);
            await _context.SaveChangesAsync();
            return StatusCode(201, grupoAlquiler);
        }
    }
}