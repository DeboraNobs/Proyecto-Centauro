using Microsoft.AspNetCore.Mvc;
using proyecto_centauro.Interfaces;
using proyecto_centauro.Models;
using proyecto_centauro.Repositorios;
using Microsoft.AspNetCore.Identity;

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
            var passwordHasher = new PasswordHasher<Usuario>();
            if (usuario.Password == null) return BadRequest("");
            usuario.Password = passwordHasher.HashPassword(usuario, usuario.Password);
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
        public async Task<ActionResult> BorrarUsuario(int id)
        {
            var existeUsuario = await _userRepositorio.ExisteUsuarioAsync(id);
            if (!existeUsuario) return NotFound(); // si no existe el usuario devuelve un not found 

            await _userRepositorio.EliminarAsync(id); // si existe lo elimina y no devuelve nada
            return NoContent();
        }

        [HttpPost("login")]
        public async Task<ActionResult<Usuario>> Login([FromBody] Login loginData)
        {
            var usuario = await _userRepositorio.ValidarCredencialesAsync(loginData.Email, loginData.Password);
            if (usuario == null) return Unauthorized("Credenciales incorrectas");

            return Ok(usuario); 
        }

    }

}