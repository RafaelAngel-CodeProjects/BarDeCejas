using BarCejas.Data.Interfaces;
using BarCejas.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarCejas.Data.Services
{
    public class ServicioPaqueteService : IServicioPaqueteService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServicioPaqueteService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<ServicioPaquete> GetServicioPaqueteAll()
        {
            return _unitOfWork.servicioPaqueteRepository.GetAll();
        }

        public async Task<ServicioPaquete> GetServicioPaqueteById(long id)
        {
            var children = new string[] { "ServicioPaquete" };
            IEnumerable<ServicioPaquete> ServicePackage = await _unitOfWork.servicioPaqueteRepository.GetByEagerLoad((d => d.Id == id), children);
            return ServicePackage.FirstOrDefault();
        }

        //public async Task<bool> InsertServicio(Servicio entity)
        //{
        //    try
        //    {
        //        entity.EsActivo = true;
        //        entity.IdCategoriaNavigation = null;
        //       // entity.IdFormaPagoNavigation = null;
        //        entity.IdUsuarioAltaNavigation = null;
        //        //entity.IdModalidadPagoNavigation = null;
        //        await _unitOfWork.servicioRepository.Add(entity);
        //        await _unitOfWork.SaveChangeAsync();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public async Task<bool> UpdateServicio(Servicio entity)
        //{
        //    try
        //    {
        //        entity.IdCategoriaNavigation = null;
        //        //entity.IdFormaPagoNavigation = null;
        //        entity.IdUsuarioAltaNavigation = null;
        //        //entity.IdModalidadPagoNavigation = null;
        //        _unitOfWork.servicioRepository.Update(entity);
        //        await _unitOfWork.SaveChangeAsync();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public async Task<bool> ChangeStatus(long id)
        //{
        //    try
        //    {
        //        var entity = await _unitOfWork.servicioRepository.GetById(id);
        //        if (entity is null)
        //            throw new Exception("Registro no encontrado");

        //        entity.EsActivo = !entity.EsActivo;
        //        _unitOfWork.servicioRepository.Update(entity);
        //        await _unitOfWork.SaveChangeAsync();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
