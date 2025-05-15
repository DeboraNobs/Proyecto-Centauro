
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using AutoMapper;
using proyecto_centauro.Interfaces;
using proyecto_centauro.Interfaces.InterfacesBusiness;
using proyecto_centauro.Models;
using proyecto_centauro.Models.DTO;
using proyecto_centauro.Requests;

namespace proyecto_centauro.Business
{
    public class SucursalBusiness : ISucursalBusiness
    {
        private readonly IMapper _mapper;
        private readonly ISucursalRepositorio _repository;

        public SucursalBusiness(IMapper mapper, ISucursalRepositorio repository) 
        {
            this._mapper = mapper;
            this._repository = repository;
        }

        public async Task<List<SucursalDTO>> Search(SucursalModelValidation.Search search)
        {
            var resultSucursal = await _repository.Search(search);
            return _mapper.Map<List<SucursalDTO>>(resultSucursal);
        }

        public async Task<SucursalDTO> Insert(SucursalModelValidation.Insert insert)
        {
            var validationTasks = new List<Task<bool>>
            {
                ValidateNombreAlreadyExists(insert.Nombre!) // valida que no exista ya una sucursal con ese nombre
            };

            await Task.WhenAll(validationTasks); // ejecuta todas las consultas en paralelo, por ej 3 a la vez. Si que la que mas tarda, tarda 100 ms, toda la operacion tardara 100ms

            if (validationTasks.Any(t => t.Result == false))
            {
                throw new Exception("Alguna validación ha fallado en metodo Insert SucursalBusiness");
            }
            var resultSucursal = await _repository.Insert(insert);
            return _mapper.Map<SucursalDTO>(resultSucursal);
        }

        public async Task<bool> Delete(SucursalModelValidation.Delete delete)
        {
            var existeResult = await ValidateAlreadyExists(delete.Id);
            if (!existeResult) throw new Exception("La sucursal a eliminar no se puede eliminar porque no existe");
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
        }

        private async Task<bool> ValidateNombreAlreadyExists(string nombre)
        {
            var searchResult = await _repository.ExisteSucursalPorNombre(nombre);
            if (searchResult) throw new Exception("La sucursal con nombre " + nombre + " ya existe");
            return true;
        }


        #endregion
    }
}