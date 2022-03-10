using BarCejas.Data.DataContext;
using BarCejas.Entities;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarCejas.Data.Interfaces
{
    public interface IOrderService
    {
        #region Orden
        Task<IEnumerable<Orden>> GetOrders();
        Orden GetOrdersById(long id);
        IEnumerable<Orden> GetOrdersByUser(long id);
        Task<Entities.Orden> InsertOrder(Orden entity);
        Task<bool> ChangeStatusOrder(int id, int idStatus);
        Task<bool> RecordPayment(int idOrder, int idPaymentForm, string voucher);
     
       
        Task<bool> UpdateOrder(Orden entity);

        Task<IEnumerable<OrdenItem>> GetOrdenItemByFilter(int idProfessional, int idLocal);
        List<AgendaResult> spConsultarAgendaTurnosResultAsync(int IdProfesional, int IdContactoLocal);

        #endregion

        #region Orden Item

        IEnumerable<OrdenItem> GetOrdersItemByOrder(long id);
        OrdenItem GetOrdersItemById(long id);
        Task<bool> InsertOrderItem(List<OrdenItem> entities);

        IEnumerable<OrdenItem> GetAllOrderItems();

        Task<bool> UpdateOrderItem(OrdenItem entity);
        #endregion

        #region HorariosProfesionales
        List<Entities.GetHorariosProfesionalResult> spGetHorariosProfesionalsResultAsync(int IdServicio, int IdContactoLocal, DateTime ServiceDate);

        #endregion

        Task<List<HistoricoOrdenesCliente>> GetCustomerOrderHistory(int IdCliente);
        Task<List<OrdenesVigentes>> GetCurrentOrders(int IdOrdenIten, int IdServicio, int IdProfesional, int IdModalidadPago, int IdEstatusOrden, int IdEstatusPago, DateTime? FechaInicio, DateTime? FechaFin);
    }
}