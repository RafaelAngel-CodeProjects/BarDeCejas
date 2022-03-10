using BarCejas.Data.Interfaces;
using BarCejas.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarCejas.Data.Services
{
    public class PaqueteService : IPaquetesService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaqueteService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> ChangeStatus(long id)
        {
            try
            {
                var entity = await _unitOfWork.paqueteRepository.GetById(id);
                if (entity is null)
                    throw new Exception("Registro no encontrado");

                entity.EsActivo = !entity.EsActivo;
                _unitOfWork.paqueteRepository.Update(entity);
                await _unitOfWork.SaveChangeAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Paquete> GetPaqueteAll()
        {
            var children = new string[] { "ServicioPaquete" };
            IEnumerable<Paquete> Packages = _unitOfWork.paqueteRepository.GetByEagerLoad(children).Result;
            return Packages;
        }

        public async Task<IEnumerable<Paquete>> GetFullPaqueteAll()
        {

            var children = new string[] { "ServicioPaquete" };
            IEnumerable<Paquete> Packages = await _unitOfWork.paqueteRepository.GetByEagerLoad((x => x.EsActivo == true), children);
            return Packages;
        }

        public IEnumerable<Paquete> GetPaqueteAllClient()
        {
            DateTime date = new DateTime().Date;
            var children = new string[] { "ServicioPaquete" };
            return _unitOfWork.paqueteRepository.GetByEagerLoad((x => (x.FechaVigenciaInicio.HasValue && x.FechaVigenciaFin.HasValue && x.FechaVigenciaInicio >= date && x.FechaVigenciaFin <= date) || (bool)x.EsActivo), children).Result;
        }

        public async Task<Paquete> GetPaqueteById(long id)
        {
            var children = new string[] { "ServicioPaquete",
                "ServicioPaquete.IdServicioNavigation.ServicioProfesional.IdProfesionalNavigation.HorarioAtencionProfesional",
                "ServicioPaquete.IdServicioNavigation.ModalidadPagoServicio",
                "ServicioPaquete.IdServicioNavigation.ModalidadPagoServicio.IdModalidadPagoNavigation"
            };


            IEnumerable<Paquete> Packages = await _unitOfWork.paqueteRepository.GetByEagerLoad((d => d.Id == id), children);
            foreach (var item in Packages)
            {
                item.ServicioPaquete = item.ServicioPaquete.Where(sp => sp.IdServicioNavigation.ServicioProfesional.Count() > 0).ToList();
            }
            return Packages.First();
        }
        public async Task<bool> InsertPaquete(Paquete entity)
        {
            try
            {
                entity.EsActivo = true;
                await _unitOfWork.paqueteRepository.Add(entity);
                await _unitOfWork.SaveChangeAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdatePaquete(Paquete entity)
        {
            try
            {
                Paquete model = await _unitOfWork.paqueteRepository.GetById(entity.Id);

                #region Asignacion de modelo
                model.EsActivo = entity.EsActivo;
                model.DescripcionCorta = entity.DescripcionCorta;
                model.DescripcionLarga = entity.DescripcionLarga;
                model.Descuento = entity.Descuento;
                model.FechaVigenciaInicio = entity.FechaVigenciaInicio;
                model.FechaVigenciaFin = entity.FechaVigenciaFin;
                model.PrecioFinal = entity.PrecioFinal;
                model.Nombre = entity.Nombre;
                model.RutaImagenHome = entity.RutaImagenHome;
                model.RutaImagenTarjeta = entity.RutaImagenTarjeta;
                model.ServicioPaquete = entity.ServicioPaquete;
                #endregion

                _unitOfWork.paqueteRepository.Update(model);
                await _unitOfWork.SaveChangeAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
