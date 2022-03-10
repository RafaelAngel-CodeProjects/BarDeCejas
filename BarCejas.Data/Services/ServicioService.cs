using BarCejas.Data.Interfaces;
using BarCejas.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarCejas.Data.Services
{
    public class ServicioService : IServicioService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServicioService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Servicio> GetServicioAll()
        {
            var children = new string[] { "IdCategoriaNavigation" };
            IEnumerable<Servicio> model = _unitOfWork.servicioRepository.GetByEagerLoad(children).Result;
            return model;
        }

        public IEnumerable<Servicio> GetServicioAllClient()
        {
            var children = new string[] { "IdCategoriaNavigation" };
            IEnumerable<Servicio> model = _unitOfWork.servicioRepository.GetByEagerLoad((s => (bool)s.EsActivo), children).Result;
            return model;
        }

        public async Task<Servicio> GetServicioById(long id)
        {
            var children = new string[] { "ServicioContactoLocal.IdContactoLocalNavigation",
                                            "FormaPagoServicio",
                                            "ModalidadPagoServicio.IdModalidadPagoNavigation",
                                            "ServicioProfesional.IdProfesionalNavigation.IdUsuarioNavigation"
                                            };
            IEnumerable<Servicio> contact = await _unitOfWork.servicioRepository.GetByEagerLoad((d => d.Id == id), children);
            if (contact is null || contact.Count() == 0)
                return null;
            return contact.FirstOrDefault();
        }

        public async Task<bool> InsertServicio(Servicio entity)
        {
            try
            {
                entity.EsActivo = true;
                entity.IdCategoriaNavigation = null;
                // entity.IdFormaPagoNavigation = null;
                entity.IdUsuarioAltaNavigation = null;
                entity.ServicioPaquete = null;
                await _unitOfWork.servicioRepository.Add(entity);
                await _unitOfWork.SaveChangeAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateServicio(Servicio entity)
        {
            try
            {

                Servicio model = await _unitOfWork.servicioRepository.GetById(entity.Id);

                #region Asignacion de contenido
                model.DescripcionLarga = entity.DescripcionLarga;
                model.DescripcionCorta = entity.DescripcionCorta;
                model.Descuento = entity.Descuento;
                model.FechaDescuentoInicio = entity.FechaDescuentoInicio;
                model.FechaDescuentoFin = entity.FechaDescuentoFin;
                model.Duracion = entity.Duracion;
                model.EsActivo = entity.EsActivo;
                model.Nombre = entity.Nombre;
                model.IdCategoria = entity.IdCategoria;
                model.IdUsuarioAlta = entity.IdUsuarioAlta;
                model.PrecioLista = entity.PrecioLista;
                model.PrecioPromocioal = entity.PrecioPromocioal;
                model.RecomendacionesReserva = entity.RecomendacionesReserva;
                model.RutaFormulario = entity.RutaFormulario;
                model.RutaImagenPaagina = entity.RutaImagenPaagina;
                model.RutaImagenTarjeta = entity.RutaImagenTarjeta;
                model.ServicioContactoLocal = entity.ServicioContactoLocal;
                model.ServicioProfesional = entity.ServicioProfesional;
                model.ModalidadPagoServicio = entity.ModalidadPagoServicio;
                model.FormaPagoServicio = entity.FormaPagoServicio;
                #endregion

                _unitOfWork.servicioRepository.Update(model);
                await _unitOfWork.SaveChangeAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ChangeStatus(long id)
        {
            try
            {
                var entity = await _unitOfWork.servicioRepository.GetById(id);
                if (entity is null)
                    throw new Exception("Registro no encontrado");

                entity.EsActivo = !entity.EsActivo;
                _unitOfWork.servicioRepository.Update(entity);
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
