using Microsoft.AspNetCore.Mvc;
using proyecto_centauro.Interfaces;
using proyecto_centauro.Models;

namespace proyecto_centauro.Controllers
{
    [Route("api/alquiler")]
    [ApiController]
    public class AlquilerController : Controller
    {
        private readonly IAlquilerRepositorio _alquilerRepositorio;

        public AlquilerController(IAlquilerRepositorio alquilerRepositorio)
        {
            _alquilerRepositorio = alquilerRepositorio;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Alquiler>>> GetAlquileres()
        {
            try
            {
                var alquileres = await _alquilerRepositorio.ObtenerTodos();
                return Json(alquileres);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error al obtener los alquileres.", detalle = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Alquiler>> GetAlquilerById(int id)
        {
            var alquiler = await _alquilerRepositorio.ObtenerAlquilerPorId(id);
            if (alquiler == null) return NotFound();
            return Json(alquiler);
        }

        [HttpPost]
        public async Task<ActionResult> CrearAlquiler([FromBody] Alquiler alquiler)
        {
            if (alquiler == null) return BadRequest("Alquiler no debe ser nulo");

            try
            {
                await _alquilerRepositorio.AgregarAlquiler(alquiler);

                var alquilerDTO = new AlquilerDTO
                {
                    Id = alquiler.Id,
                    Fechainicio = alquiler.Fechainicio,
                    FechaFin = alquiler.FechaFin,
                    LugarRecogida = alquiler.LugarRecogida!,
                    LugarDevolucion = alquiler.LugarDevolucion!,
                    HorarioRecogida = alquiler.HorarioRecogida,
                    HorarioDevolucion = alquiler.HorarioDevolucion,
                    UsersId = alquiler.UsersId,
                    GrupoId = alquiler.GrupoId
                };

                return StatusCode(201, alquilerDTO);
            }
            catch (KeyNotFoundException ex)
            {
                return Unauthorized(new { message = "Debe loguearse para alquilar un coche" , detail = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = "No hay coches disponibles en este grupo para alquilar." , detail = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", detail = ex.Message });
            }
        }



        [HttpPut("{id}")]
        public async Task<ActionResult> EditarAlquiler(int id, [FromBody] Alquiler alquiler)
        {
            if (id != alquiler.Id) return BadRequest();
            await _alquilerRepositorio.ActualizarAlquiler(alquiler);
            return NoContent();

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> BorrarAlquiler(int id)
        {
            var existeAlquiler = await _alquilerRepositorio.ExisteAlquiler(id);
            if (!existeAlquiler) return NotFound();
            await _alquilerRepositorio.EliminarAlquiler(id);
            return NoContent();
        }


    }
}
