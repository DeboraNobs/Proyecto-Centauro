using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using proyecto_centauro.Data;
using proyecto_centauro.Models;

namespace proyecto_centauro.Controllers
{
    [Route("api/usuario")]
    [ApiController]
    public class UsuarioController : Controller // Controller: clase base para controladores
    {
        private readonly UsuarioContext _context;  // variable de tipo UsuarioContext (creado en carpeta DATA) que representa el acceso a la bbdd

        public UsuarioController(UsuarioContext context) // controlador
        {
            _context = context;
        }

        // GET: Usuario
        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            var usuarios = await _context.Users.ToListAsync();
            return Json(usuarios);
        }

        // GET: Usuario/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int? id)
        {
            var usuario = await _context.Users.FindAsync(id);

            if (usuario == null) return NotFound();
            return Json(usuario);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Usuario usuario)
        {
            if (usuario == null)
            {
                return BadRequest("El usuario no puede ser nulo.");
            }

            Console.WriteLine($"Nombre: {usuario.Nombre}, Email: {usuario.Email}, Apellidos: {usuario.Apellidos}, Password: {usuario.Password}");

            if (string.IsNullOrEmpty(usuario.Nombre) ||
                string.IsNullOrEmpty(usuario.Email) ||
                string.IsNullOrEmpty(usuario.Apellidos) ||
                string.IsNullOrEmpty(usuario.Password))
            {
                return BadRequest("Todos los campos son obligatorios.");
            }

            _context.Users.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, usuario);
        }

        // GET: Usuario/Edit/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] Usuario usuario)
        {
            if (id != usuario.Id) return BadRequest();

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id)) return NotFound();
                throw;
            }

            return NoContent();
        }


        // Eliminar un usuario
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuario = await _context.Users.FindAsync(id);
            if (usuario == null) return NotFound();

            _context.Users.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
