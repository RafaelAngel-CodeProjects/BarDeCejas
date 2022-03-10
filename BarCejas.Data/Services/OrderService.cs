using BarCejas.Data.DataContext;
using BarCejas.Data.Interfaces;
using BarCejas.Entities;
using BarCejas.Entities.Enumerations;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;

namespace BarCejas.Data.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly BardecejasContext _procedures;

        public OrderService(IUnitOfWork unitOfWork, BardecejasContext procedures)
        {
            _unitOfWork = unitOfWork;
            _procedures = procedures;
        }

        #region Orden

        public Task<IEnumerable<Orden>> GetOrders()
        {
            IEnumerable<Orden> contact = _unitOfWork.ordenRepository.GetAll();
            return Task.FromResult(contact);
        }

        public Orden GetOrdersById(long id)
        {
            return _unitOfWork.ordenRepository.GetAll().FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Orden> GetOrdersByUser(long id)
        {
            return _unitOfWork.ordenRepository.GetAll().Where(x => x.IdCliente == id);
        }
        public async Task<Entities.Orden> InsertOrder(Orden entity)
        {
            try
            {
                await _unitOfWork.ordenRepository.Add(entity);
                await _unitOfWork.SaveChangeAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ChangeStatusOrder(int id, int idStatus)
        {
            try
            {
                if (id > 0)
                {
                    var entity = await _unitOfWork.ordenRepository.GetById(id);
                    if (entity is null)
                        throw new Exception("Registro no encontrado.");

                    entity.IdEstatus = idStatus;

                    if (idStatus == (int)EstatusOrden.Anulada && entity.IdEstatusPago == (int)EstatusPago.pendiente)
                        entity.IdEstatusPago = (int)EstatusPago.Anulado;

                    _unitOfWork.ordenRepository.Update(entity);
                    await _unitOfWork.SaveChangeAsync();

                    return true;
                }
                throw new Exception("Registro no encontrado.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> RecordPayment(int idOrder, int idPaymentForm, string voucher)
        {
            try
            {
                if (idOrder > 0)
                {
                    var entity = await _unitOfWork.ordenRepository.GetById(idOrder);
                    if (entity is null)
                        throw new Exception("Registro no encontrado.");
                    
                    if (idPaymentForm != (int)FormaDePago.Efectivo)
                        entity.ComprobantePago = voucher;

                    entity.IdFormaPago = idPaymentForm;
                    entity.IdEstatusPago = (int)EstatusPago.Pagado;

                    _unitOfWork.ordenRepository.Update(entity);
                    await _unitOfWork.SaveChangeAsync();

                    return true;
                }
                throw new Exception("Registro no encontrado.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region OrderItem
        public OrdenItem GetOrdersItemById(long id)
        {
            return _unitOfWork.ordenItemsRepository.GetAll().FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<OrdenItem> GetOrdersItemByOrder(long id)
        {
            return _unitOfWork.ordenItemsRepository.GetAll().Where(x => x.IdOrden == id);
        }

        public IEnumerable<OrdenItem> GetAllOrderItems()
        {
            return _unitOfWork.ordenItemsRepository.GetAll();
        }
        public async Task<bool> InsertOrderItem(List<OrdenItem> entities)
        {
            try
            {
                foreach (var item in entities)
                {
                    int ordenItem = 0;
                    if (item.IdOrden > 0)
                    {
                        ordenItem = _unitOfWork.ordenItemsRepository.GetAll().Count(x => x.IdServicio == item.IdServicio && x.IdOrden == item.IdOrden);
                    }
                    if (ordenItem == 0)
                    {
                        await _unitOfWork.ordenItemsRepository.Add(entities.First());
                    }
                }
                await _unitOfWork.SaveChangeAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }

        public async Task<bool> UpdateOrderItem(OrdenItem entity)
        {
            try
            {
                if (entity.Id == 0)
                    throw new Exception("Registro no encontrado.");

                OrdenItem model = await _unitOfWork.ordenItemsRepository.GetById(entity.Id);

                if (model is null)
                    throw new Exception("Registro no encontrado.");

                model.Hora = entity.Hora;
                model.FechaDeCita = entity.FechaDeCita;
                model.IdProfesional = entity.IdProfesional;

                _unitOfWork.ordenItemsRepository.Update(model);

                await _unitOfWork.SaveChangeAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }

        public async Task<bool> UpdateOrder(Orden entity)
        {
            try
            {
                if (entity.Id == 0)
                    throw new Exception("Registro no encontrado.");

                Orden model = await _unitOfWork.ordenRepository.GetById(entity.Id);

                if (model is null)
                    throw new Exception("Registro no encontrado.");

                model.ComprobantePago = entity.ComprobantePago;
                model.IdCliente = entity.IdCliente;
                model.IdEstatus = entity.IdEstatus;
                model.IdModalidadPago = entity.IdModalidadPago;
                model.IdFormaPago = entity.IdFormaPago;
                model.MontoPaquete = entity.MontoPaquete;

                _unitOfWork.ordenRepository.Update(model);

                await _unitOfWork.SaveChangeAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }

        #endregion

        public async Task<List<HistoricoOrdenesCliente>> GetCustomerOrderHistory(int IdCliente)
        {
            try
            {
                List<HistoricoOrdenesCliente> history = new List<HistoricoOrdenesCliente>();
                var dbHistory = await _unitOfWork.reporteProcedureRepository.spConsultarHistoricoOrdenesClienteAsync(IdCliente);

                dbHistory.ForEach(x =>
                {
                    history.Add(new HistoricoOrdenesCliente()
                    {
                        IdOrden = x.IdOrden,
                        IdEstatus = x.IdEstatus,
                        IdEstatusPago = x.IdEstatusPago,
                        IdModalidadPago = x.IdModalidadPago,
                        IdFormaPago = x.IdFormaPago,
                        IdOrdenItem = x.IdOrdenItem,
                        Hora = x.Hora,
                        FechaDeCita = x.FechaDeCita,
                        Monto = x.Monto,
                        IdServicio = x.IdServicio,
                        Servicio = x.Servicio,
                        Duracion = x.Duracion,
                        RutaImagenPaagina = x.RutaImagenPaagina,
                        RutaImagenTarjeta = x.RutaImagenTarjeta,
                        RutaFormulario = x.RutaFormulario,
                        IdContacto = x.IdContacto,
                        Sede = x.Servicio,
                        Direccion = x.Direccion,
                        IdProfesional = x.IdProfesional,
                        Profesional = x.Profesional,
                        IdCliente = x.IdCliente,
                        Cliente = x.Cliente
                    });
                });
                return history;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<OrdenesVigentes>> GetCurrentOrders(int IdOrdenIten, int IdServicio, int IdProfesional, int IdModalidadPago, int IdEstatusOrden, int IdEstatusPago, DateTime? FechaInicio, DateTime? FechaFin)
        {
            List<OrdenesVigentes> currentOrders = new List<OrdenesVigentes>();
            var dbCurrentOrders = await _unitOfWork.reporteProcedureRepository.spConsultarOrdenesAsync(IdOrdenIten, IdServicio, IdProfesional, IdModalidadPago, IdEstatusOrden, IdEstatusPago, FechaInicio, FechaFin);

            dbCurrentOrders.ForEach(x =>
            {
                currentOrders.Add(new OrdenesVigentes()
                {
                    IdOrden = x.IdOrden,
                    IdEstatus = x.IdEstatus,
                    IdEstatusPago = x.IdEstatusPago,
                    IdModalidadPago = x.IdModalidadPago,
                    IdFormaPago = x.IdFormaPago,
                    IdOrdenItem = x.IdOrdenItem,
                    Hora = x.Hora,
                    FechaDeCita = x.FechaDeCita,
                    Monto = x.Monto,
                    IdServicio = x.IdServicio,
                    Servicio = x.Servicio,
                    Duracion = x.Duracion,
                    RutaFormulario = x.RutaFormulario,
                    IdContacto = x.IdContacto,
                    Sede = x.Servicio,
                    Direccion = x.Direccion,
                    IdProfesional = x.IdProfesional,
                    Profesional = x.Profesional,
                    IdCliente = x.IdCliente,
                    Cliente = x.Cliente
                });
            });
            return currentOrders;
        }

        public List<AgendaResult> spConsultarAgendaTurnosResultAsync(int IdProfesional, int IdContactoLocal)
        {
            try
            {
                var result = _procedures.Procedures.spConsultarAgendaTurnosAsync(IdProfesional, IdContactoLocal).Result.Select(cat => new AgendaResult
                {
                    apellidoCliente = cat.apellidoCliente,
                    DescripcionCorta = cat.DescripcionCorta,
                    direccionSuc = cat.direccionSuc,
                    Duracion = cat.Duracion,
                    estatus = cat.estatus,
                    FechaDeCita = cat.FechaDeCita,
                    Hora = cat.Hora,
                    Monto = cat.Monto,
                    modalidadPago = cat.modalidadPago,
                    nombreCliente = cat.nombreCliente,
                    nombreServicio = cat.nombreServicio,
                    Profesional = cat.Profesional,
                    RutaImagenPaagina = !string.IsNullOrWhiteSpace(cat.RutaImagenPaagina) ? cat.RutaImagenPaagina : "~/images/sinImagen.png",
                    Sede = cat.Sede,
                    IdContacto = cat.IdContacto,
                    IdOrden = cat.IdOrden,
                    IdOrdenItem = cat.IdOrdenItem,
                    IdEstatus = cat.IdEstatus,
                    IdEstatusPago = cat.IdEstatusPago,
                    IdFormaPago = cat.IdFormaPago,
                    IdProfesional = cat.IdProfesional,
                    IdServicio = cat.IdServicio
                });
                return result.ToList<AgendaResult>();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<OrdenItem>> GetOrdenItemByFilter(int idProfessional, int idLocal)
        {
            var children = new string[] { "IdProfesionalNavigation", "IdProfesionalNavigation.IdUsuarioNavigation", "IdOrdenNavigation",
                                            "IdOrdenNavigation.IdModalidadPagoNavigation", "IdOrdenNavigation.IdClienteNavigation", "IdOrdenNavigation.IdContactoLocalNavigation" };
            IEnumerable<OrdenItem> ordenItems = _unitOfWork.ordenItemsRepository.GetAll();
            foreach (var item in ordenItems)
            {
                item.IdProfesionalNavigation = _unitOfWork.profesionalRepository.GetById(item.IdProfesional).Result;
                item.IdOrdenNavigation = _unitOfWork.ordenRepository.GetById(item.IdOrden).Result;
                item.IdOrdenNavigation.IdClienteNavigation = _unitOfWork.usuarioRepository.GetById(item.IdOrdenNavigation.IdCliente).Result;
                item.IdOrdenNavigation.IdContactoLocalNavigation = _unitOfWork.contactoLocalRepository.GetById(item.IdOrdenNavigation.IdContactoLocal).Result;
                item.IdOrdenNavigation.IdModalidadPagoNavigation = _unitOfWork.modalidadPagoRepository.GetById((int)(item.IdOrdenNavigation.IdModalidadPago / 10)).Result;
            }
            if (idProfessional > 0 && idLocal > 0)
            {
                ordenItems = ordenItems.Where((x => x.IdProfesional == idProfessional && x.IdOrdenNavigation.IdContactoLocal == idLocal));
            }
            else if (idProfessional > 0 && idLocal == -1)
            {
                ordenItems = ordenItems.Where((x => x.IdProfesional == idProfessional));
            }
            else if (idProfessional == -1 && idLocal > 0)
            {
                ordenItems = ordenItems.Where((x => x.IdOrdenNavigation.IdContactoLocal == idLocal));
            }
            return ordenItems;
        }

        public List<Entities.GetHorariosProfesionalResult> spGetHorariosProfesionalsResultAsync(int IdServicio, int IdContactoLocal, DateTime ServiceDate)
        {
            try
            {
                var result = _procedures.Procedures.spGetHorariosProfesionalesAsync(7, 3, 6, Convert.ToDateTime("2017/03/07")).Result
                 .Select(x => new GetHorariosProfesionalResult
                 {
                     DescripcionProfesional = x.DescripcionProfesional,
                     DireccionSucursal = x.DireccionSucursal,
                     DuracionServicio = x.DuracionServicio,
                     HoraFinal = x.HoraFinal,
                     HoraInicial = x.HoraInicial,
                     IdProfesional = x.IdProfesional,
                     NombreProfesional = x.NombreProfesional,
                     NombreServicio = x.NombreServicio,
                     NombreSucursal = x.NombreSucursal
                 });
                return result.ToList<GetHorariosProfesionalResult>();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}