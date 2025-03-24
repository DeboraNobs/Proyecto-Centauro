using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proyecto_centauro.Data;
using proyecto_centauro.Interfaces;
using proyecto_centauro.Models;
using proyecto_centauro.Repositorios;

namespace proyecto_centauro.Controllers
{
    [Route("api/servicio")]
    [ApiController]
    public class ServicioController : Controller
    {
        private readonly IServicioRepositorio _servicioRepositorio;

        public ServicioController(IServicioRepositorio servicioRepositorio)
        {
            _servicioRepositorio = servicioRepositorio;
        }

         [HttpGet]
        public async Task<ActionResult<IEnumerable<Servicio>>> GetServicios()
        {
            var servicios = await _servicioRepositorio.ObtenerTodosServicios(); // metodo de ServicioRepositorio
            return Json(servicios);
        }

       [HttpGet("{id}")] 
        public async Task<ActionResult<Servicio>> GetUsuarioByID(int id)
        {
            var servicio = await _servicioRepositorio.ObtenerServicioPorId(id);
            if (servicio == null) return NotFound();
            return Json(servicio);
        }

        [HttpPost]
        public async Task<ActionResult<Servicio>> CrearServicio([FromBody] Servicio servicio)
        {
            if(servicio == null) return BadRequest("Servicio no debe ser nulo");

            await _servicioRepositorio.AgregarServicio(servicio);
            return StatusCode(201, servicio);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> EditarServicio(int id, [FromBody] Servicio servicio)
        {
           if (id != servicio.Id) return BadRequest();

           await _servicioRepositorio.ActualizarServicio(servicio);
           return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> BorrarServicio(int id)
        {
            var existeServicio = await _servicioRepositorio.ExisteServicio(id);
            if (!existeServicio) return NotFound();

            await _servicioRepositorio.EliminarServicio(id);
            return NoContent();
        }
    }
}
