using Microsoft.AspNetCore.Mvc;
using proyecto_centauro.Interfaces;
using proyecto_centauro.Models;
using proyecto_centauro.Repositorios;

namespace proyecto_centauro.Controllers
{
    [Route("api/usuario")]
    [ApiController]
    
    // este controlador refactorizado usa la interfaz del repositorio  
    // en lugar de acceder directamente a la base de datos.
    public class UsuarioControllerRefactorizado : Controller
    {
        private readonly IUsuarioRepositorio _userRepositorio;

        public UsuarioControllerRefactorizado(IUsuarioRepositorio userRepositorio)
        {
            _userRepositorio = userRepositorio;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios() // Task<> significa que la función devuelve una tarea asíncrona.
        {
            var usuarios = await _userRepositorio.ObtenerTodosAsync(); // metodo de UsuarioRepositorio
            return Json(usuarios);
        }
        [HttpGet("{id}")] 
        public async Task<ActionResult<Usuario>> GetUsuarioById(int id)
        {
            var usuario = await _userRepositorio.ObtenerPorIdAsync(id); // metodo de UsuarioRepositorio
            if (usuario == null) return NotFound();
            return Json(usuario);
        }
        [HttpPost]
        public async Task<ActionResult<Usuario>> CrearUsuario([FromBody] Usuario usuario) 
        {
            if (usuario == null) return BadRequest("Usuario no debe ser nulo");
            await _userRepositorio.AgregarAsync(usuario);
            return StatusCode(201, usuario); 
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> EditarUsuario(int id, [FromBody] Usuario usuario) 
        { 
            if (id != usuario.Id) return BadRequest();
            await _userRepositorio.ActualizarAsync(usuario);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> BorrarUsuario(int id) {
            var existeUsuario = await _userRepositorio.ExisteUsuarioAsync(id);
            if (!existeUsuario) return NotFound(); // si no existe el usuario devuelve un not found 

            await _userRepositorio.EliminarAsync(id); // si existe lo elimina y no devuelve nada
            return NoContent(); 
        }

        [HttpPost("login")] // envío un post con un parametro login
        public async Task<ActionResult> Login([FromBody] Login request) 
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password)) 
                return BadRequest("El email y la contraseña son obligatorios");

            var usuario = await _userRepositorio.ValidarCredencialesAsync(request.Email, request.Password);
            if (usuario == null)  return NotFound("Usuario no encontrado");
            return Ok(new { message = $"Inicio de sesión con email: {usuario.Email}" }); // devuelve un objeto JSON para que el front pueda leer JSON
        }

    }
}