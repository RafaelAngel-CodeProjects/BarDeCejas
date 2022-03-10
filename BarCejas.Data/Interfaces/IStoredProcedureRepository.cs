using BarCejas.Data.DataContext;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BarCejas.Data.Interfaces
{
    public interface IStoredProcedureRepository
    {
        Task<List<spGetReportePaqueteResult>> spGetReportePaqueteAsync(string NombrePaquete, string NombreProfesional, string NombreCliente, DateTime? FechaIncio, DateTime? FechaFin, string NombreLocal, int? MedioDePago, int? EstadoTurno, int? EstadoPago);

        Task<List<spGetReporteProfesionalResult>> spGetReporteProfesionalAsync(string NombreServicio, string NombreProfesional, decimal? Precio, DateTime? FechaIncio, DateTime? FechaFin);

        Task<List<spGetReporteServicioResult>> spGetReporteServicioAsync(string NombreServicio, string NombreProfesional, string NombreCliente, DateTime? FechaIncio, DateTime? FechaFin, string NombreLocal, int? MedioDePago, int? EstadoTurno, int? EstadoPago);

        Task<List<spConsultarHistoricoOrdenesClienteResult>> spConsultarHistoricoOrdenesClienteAsync(int IdCliente);

        Task<List<spConsultarOrdenesResult>> spConsultarOrdenesAsync(int IdOrdenIten, int IdServicio, int IdProfesional, int IdModalidadPago, int IdEstatusOrden, int IdEstatusPago, DateTime? FechaInicio, DateTime? FechaFin);
    }
}
