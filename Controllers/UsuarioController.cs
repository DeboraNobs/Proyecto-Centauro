using Microsoft.AspNetCore.Mvc;
using proyecto_centauro.Interfaces;
using proyecto_centauro.Models;
using proyecto_centauro.Repositorios;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using proyecto_centauro.Interfaces.InterfacesDapper;
using proyecto_centauro.Interfaces.InterfacesBusiness;
using proyecto_centauro.Requests;

namespace proyecto_centauro.Controllers
{
    [Route("api/usuario")]
    [ApiController]

    // este controlador refactorizado usa la interfaz del repositorio  
    // en lugar de acceder directamente a la base de datos.

    public class UsuarioController(IUsuarioBusiness businnes, IConfiguration config) : Controller
    {
        [HttpGet]
        public async Task<ActionResult<List<UsuarioDTO>>> GetUsuarios([FromQuery] UsuarioModelValidation.Search request) // Task<> significa que la función devuelve una tarea asíncrona.
        {
            return await businnes.Search(request);
        }

        [HttpPost]
        public async Task<ActionResult<UsuarioDTO>> CrearUsuario([FromBody] UsuarioModelValidation.Insert request) // debo usar FromBody para que envie un json
        {
            return await businnes.Insert(request);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UsuarioDTO>> EditarUsuario(int id, [FromBody] UsuarioModelValidation.Modify request)
        {
            request.Id = id;
            return await businnes.Modify(request);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> BorrarUsuario(int id, [FromForm] UsuarioModelValidation.Delete request)
        {
            request.Id = id;
            return await businnes.Delete(request);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDTO>> ObtenerUsuarioPorId(int id)
        {
            return await businnes.ObtenerPorId(id);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UsuarioModelValidation.Login login)
        {
            var usuario = await businnes.ValidarCredenciales(login.Email, login.Password);

            if (usuario == null) return Unauthorized("Credenciales inválidas");

            // Generar token JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]!));

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Email, usuario.Email ?? string.Empty),
        new Claim(ClaimTypes.Name, !string.IsNullOrEmpty(usuario.Nombre) ? usuario.Nombre : usuario.Email ?? string.Empty),
    };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = "tuApp",
                Audience = "tuApp",
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                mensaje = "Estás autorizado ✅",
                token = tokenString,
                usuario = new
                {
                    usuario.Id,
                    usuario.Nombre,
                    usuario.Email
                }
            });
        }

    }
}

#region sin businnes


/* sin usar BUSINESS
********************************************************************
using Microsoft.AspNetCore.Mvc;
using proyecto_centauro.Interfaces;
using proyecto_centauro.Models;
using proyecto_centauro.Repositorios;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using proyecto_centauro.Interfaces.InterfacesDapper;

namespace proyecto_centauro.Controllers
{
    [Route("api/usuario")]
    [ApiController]

    // este controlador refactorizado usa la interfaz del repositorio  
    // en lugar de acceder directamente a la base de datos.

    public class UsuarioControllerRefactorizado : Controller
    {
        private readonly IUsuarioDapper _userRepositorio; // luego debere colocar IUsuarioRepositorio
        private readonly IConfiguration config;

        public UsuarioControllerRefactorizado(IUsuarioDapper userRepositorio, IConfiguration config) // luego debere colocar IUsuarioRepositorio
        {
            _userRepositorio = userRepositorio;
            this.config = config;
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
        public async Task<ActionResult> Login([FromBody] Login loginData)
        {
            try
            {
                Console.WriteLine("Solicitud de login recibida");

                var usuario = await _userRepositorio.ValidarCredencialesAsync(loginData.Email, loginData.Password);
                if (usuario == null) return Unauthorized("Credenciales incorrectas");

                Console.WriteLine("Usuario validado, generando token...");

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]!));

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, usuario.Email ?? string.Empty),
                    new Claim(ClaimTypes.Name, !string.IsNullOrEmpty(usuario.Nombre) ? usuario.Nombre : usuario.Email ?? string.Empty),
                };

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddHours(1),
                    Issuer = "tuApp",
                    Audience = "tuApp",
                    SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);


                Console.WriteLine($"Token generado: {tokenString}");

                return Ok(new
                {
                    mensaje = "Estás autorizado ✅",
                    token = tokenString,
                    usuario = new
                    {
                        usuario.Id,
                        usuario.Nombre,
                        usuario.Email
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en login: {ex.Message}");
                return StatusCode(500, "Error en el servidor");
            }
        }

    }
}

*/



#endregion