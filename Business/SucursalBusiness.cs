
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using AutoMapper;
using proyecto_centauro.Interfaces.InterfacesBusiness;
using proyecto_centauro.Interfaces.InterfacesValidation;
using proyecto_centauro.Models;
using proyecto_centauro.Models.DTO;
using proyecto_centauro.Requests;

namespace proyecto_centauro.Business
{
    public class SucursalBusiness : ISucursalBusiness
    {
        private readonly IMapper _mapper;
        private readonly ISucursalRepositorioV _repository;
        private readonly string _connectionString;

        public SucursalBusiness(IMapper mapper, ISucursalRepositorioV repository, IConfiguration configuration, IDbConnection connection) // ver si se usa DbConection o no)
        {
            this._mapper = mapper;
            this._repository = repository;
            _connectionString = configuration.GetConnectionString("DefaultConnection") // tomo la default conection que esta en appsettings.json
                ?? throw new InvalidOperationException("Cadena de conexión 'DefaultConnection' no encontrada.");
        }

        public async Task<List<SucursalDTO>> Search(SucursalModelValidation.Search search)
        {
            var resultSucursal = await _repository.Search(search);
            return _mapper.Map<List<SucursalDTO>>(resultSucursal);
        }

        /*
                public async Task<SucursalDTO> Insert(SucursalModelValidation.Insert insert)
                {
                    var validationTasks = new List<Task<bool>>
                    {
                        ValidateNombreAlreadyExists(insert.Nombre!)
                    };

                    await Task.WhenAll(validationTasks);

                    if (validationTasks.Any(t => t.Result == false))
                    {
                        throw new Exception("Alguna validación ha fallado en metodo Insert SucursalBusinnes");
                    }

                    var resultSucursal = await _repository.Insert(insert); // Este método debería devolver la entidad
                    return _mapper.Map<SucursalDTO>(resultSucursal);
                }
        */
        public async Task<SucursalDTO> Insert(SucursalModelValidation.Insert insert)
        {
            var validationTasks = new List<Task<bool>>
            {
                ValidateNombreAlreadyExists(insert.Nombre!) // Validar que no exista ya una sucursal con ese nombre
            };

            await Task.WhenAll(validationTasks);

            // Si alguna validación falla (en este caso, si el nombre ya existe)
            if (validationTasks.Any(t => t.Result == false))
            {
                throw new Exception("Alguna validación ha fallado en metodo Insert SucursalBusiness");
            }

            // Proceder con la inserción de la sucursal
            var resultSucursal = await _repository.Insert(insert);
            return _mapper.Map<SucursalDTO>(resultSucursal);
        }

        public async Task<bool> Delete(SucursalModelValidation.Delete delete)
        {
            var existeResult = await ValidateAlreadyExists(delete.Id);
            if (!existeResult)
            {
                throw new Exception("La sucursal a eliminar no se puede eliminar porque no existe");
            }

            return await _repository.Delete(delete);
        }


        public async Task<SucursalDTO> Modify(SucursalModelValidation.Modify modify)
        {
            var validationTasks = new List<Task<bool>>
            {
                ValidateAlreadyExists(modify.Id)
            };

            await Task.WhenAll(validationTasks);


            if (validationTasks.Any(t => t.Result == false))
            {
                throw new Exception("Alguna validación ha fallado en metodo Modify SucursalBusinnes");
            }

            return await _repository.Modify(modify);
        }


        #region Validators
        private async Task<bool> ValidateAlreadyExists(int id)
        {
            return await _repository.ExisteSucursal(id);
            /*if (searchResult == false)
            {
                throw new Exception("La sucursal no existe");
            }
            return searchResult; */
        }

        private async Task<bool> ValidateNombreAlreadyExists(string nombre)
        {
            var searchResult = await _repository.ExisteSucursalPorNombre(nombre);
            if (searchResult) // Si la sucursal ya existe
            {
                throw new Exception("La sucursal con nombre " + nombre + " ya existe");
            }
            return true; // Si no existe, está bien
        }


        #endregion
    }
}