using AutoMapper;
using proyecto_centauro.Interfaces;
using proyecto_centauro.Interfaces.InterfacesBusiness;
using proyecto_centauro.Models;
using proyecto_centauro.Requests;

namespace proyecto_centauro.Business
{
    public class UsuarioBusiness : IUsuarioBusiness
    {
        private readonly IMapper _mapper;
        private readonly IUsuarioRepositorio _repository;


        public UsuarioBusiness(IMapper mapper, IUsuarioRepositorio repository)
        {
            this._mapper = mapper;
            this._repository = repository;
        }

        public async Task<List<UsuarioDTO>> Search(UsuarioModelValidation.Search search)
        {
            var resultUsuario = await _repository.Search(search);
            return _mapper.Map<List<UsuarioDTO>>(resultUsuario);
        }
        public async Task<UsuarioDTO> Insert(UsuarioModelValidation.Insert insert)
        {
            var validationTasks = new List<Task<bool>>
            {
                ValidateNombreAlreadyExists(insert.Nombre!)
            };

            await Task.WhenAll(validationTasks); // ejecuta todas las consultas en paralelo, por ej 3 a la vez. Si que la que mas tarda, tarda 100 ms, toda la operacion tardara 100ms

            if (validationTasks.Any(t => t.Result == false))
            {
                throw new Exception("Alguna validación ha fallado en metodo Insert UsuarioBusiness");
            }
            var resultadoUsuario = await _repository.Insert(insert);
            return _mapper.Map<UsuarioDTO>(resultadoUsuario);
        }
        public async Task<bool> Delete(UsuarioModelValidation.Delete delete)
        {
            var existeResult = await ValidateAlreadyExists(delete.Id);
            if (!existeResult) throw new Exception("El usuario a eliminar no se puede eliminar porque no existe");
            return await _repository.Delete(delete);
        }
        public async Task<UsuarioDTO> Modify(UsuarioModelValidation.Modify modify)
        {
            var validationTasks = new List<Task<bool>>
            {
                ValidateAlreadyExists(modify.Id)
            };

            await Task.WhenAll(validationTasks); // ejecuta todas las consultas en paralelo, por ej 3 a la vez. Si que la que mas tarda, tarda 100 ms, toda la operacion tardara 100ms

            if (validationTasks.Any(t => t.Result == false))
            {
                throw new Exception("Alguna validación ha fallado en metodo Insert UsuarioBusiness");
            }
            return await _repository.Modify(modify);
        }

        public async Task<UsuarioDTO?> ValidarCredenciales(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El email no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("La contraseña no puede estar vacía.");

            var usuario = await _repository.ValidarCredencialesAsync(email, password);

            if (usuario == null) return null;

            return _mapper.Map<UsuarioDTO>(usuario);
        }

        public async Task<UsuarioDTO> ObtenerPorId(int id)
        {
            
            var validationTasks = new List<Task<bool>>
            {
                ValidateAlreadyExists(id)
            };

            await Task.WhenAll(validationTasks); // ejecuta todas las consultas en paralelo, por ej 3 a la vez. Si que la que mas tarda, tarda 100 ms, toda la operacion tardara 100ms

            if (validationTasks.Any(t => t.Result == false))
            {
                throw new Exception("Alguna validación ha fallado en metodo Insert UsuarioBusiness");
            }
            return await _repository.ObtenerPorId(id);
        } 


        #region Validators

        private async Task<bool> ValidateAlreadyExists(int id)
        {
            return await _repository.ExisteUsuario(id);
        }

        private async Task<bool> ValidateNombreAlreadyExists(string nombre)
        {
            var searchResult = await _repository.ExisteUsuarioPorNombre(nombre);
            if (searchResult) throw new Exception("El usuario con nombre: " + nombre + " ya existe.");
            return true;
        }

        #endregion
    }
}