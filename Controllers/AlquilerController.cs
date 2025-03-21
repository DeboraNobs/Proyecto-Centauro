using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using proyecto_centauro.Data;
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
            var alquileres = await _alquilerRepositorio.ObtenerTodos();
            return Json(alquileres);
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
            await _alquilerRepositorio.AgregarAlquiler(alquiler);
            return StatusCode(201, alquiler);
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
