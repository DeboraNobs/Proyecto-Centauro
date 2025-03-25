using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using proyecto_centauro.Data;
using proyecto_centauro.Models;
using proyecto_centauro.Models.DTO;

namespace proyecto_centauro.Controllers
{
    [Route("api/servicio-alquiler")]
    [ApiController]
    public class ServicioAlquilerController : Controller
    {
        private readonly BBDDContext _context;

        public ServicioAlquilerController(BBDDContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServicioAlquiler>>> GetServiciosAlquileres()
        {
            var servicios = await _context.ServicioAlquileres.ToListAsync();
            return Ok(servicios);
        }

        [HttpPost]
        public async Task<IActionResult> CrearServicioAlquiler([FromBody] ServicioAlquilerDTO servicioAlquilerDTO)
        {
            // Verifica si los IDs existen en la base de datos
            var alquiler = await _context.Alquileres.FindAsync(servicioAlquilerDTO.AlquilerId);
            var servicio = await _context.Servicios.FindAsync(servicioAlquilerDTO.ServicioId);

            if (alquiler == null || servicio == null) return NotFound("AlquilerId o ServicioId no encontrados.");
            
            // Crea la relación
            var servicioAlquiler = new ServicioAlquiler
            {
                AlquilerId = servicioAlquilerDTO.AlquilerId,
                ServicioId = servicioAlquilerDTO.ServicioId,
                Precio = servicioAlquilerDTO.Precio
            };

            _context.ServicioAlquileres.Add(servicioAlquiler);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(CrearServicioAlquiler), new { id = servicioAlquiler.Id }, servicioAlquiler);
        }
    }
}