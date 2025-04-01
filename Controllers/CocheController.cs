using Microsoft.AspNetCore.Mvc;
using proyecto_centauro.Interfaces;
using proyecto_centauro.Models;
using proyecto_centauro.Models.DTO;

namespace proyecto_centauro.Controllers
{
    [Route("api/coche")]
    [ApiController]
    public class CocheController : Controller
    {
        private readonly ICocheRepositorio _cocheRepositorio;

        public CocheController(ICocheRepositorio cocheRepositorio)
        {
            _cocheRepositorio = cocheRepositorio;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Coche>>> GetCoches()
        {
            var coches = await _cocheRepositorio.ObtenerTodosCoches();
            return Ok(coches);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Coche>> GetCocheById(int id)
        {
            var coche = await _cocheRepositorio.ObtenerCochePorId(id);
            if (coche == null) return NotFound();
            return Ok(coche);
        }
        [HttpPost]
        public async Task<ActionResult> CrearCoche([FromBody] Coche coche)
        {
            if (coche == null) return BadRequest("Coche no debe ser nulo");
            await _cocheRepositorio.AgregarCoche(coche);

            var cocheDTO = new CocheDTO
            {
                Id = coche.Id,
                Marca = coche.Marca,
                Modelo = coche.Modelo,
                Descripcion = coche.Descripcion,
                Patente = coche.Patente,
                Tipo_coche = coche.Tipo_coche,
                Tipo_cambio = coche.Tipo_cambio,
                Num_plazas = coche.Num_plazas,
                Num_maletas = coche.Num_maletas,
                Num_puertas = coche.Num_puertas,
                Posee_aire_acondicionado = coche.Posee_aire_acondicionado
            };
            return StatusCode(201, cocheDTO);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> EditarCoche(int id, [FromBody] Coche coche)
        {
            if(id != coche.Id) return BadRequest();
            await _cocheRepositorio.ActualizarCoche(coche);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> BorrarCoche(int id)
        {
            var existeCoche = await _cocheRepositorio.ExisteCoche(id);
            if (!existeCoche) return NotFound();

            await _cocheRepositorio.EliminarCoche(id);
            return NoContent();
        }

    }
}