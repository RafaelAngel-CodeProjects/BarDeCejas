using BarCejas.Entities.Reporte;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BarCejas.Data.Interfaces
{
    public interface IReporteService
    {
        Task<List<ReportePaquete>> GetReporteByPaquete(string NombrePaquete, string NombreProfesional, string NombreCliente, DateTime? FechaIncio, DateTime? FechaFin, string NombreLocal, int? MedioDePago, int? EstadoTurno, int? EstadoPago);

        Task<List<ReporteProfesional>> GetReporteByProfesional(string NombreServicio, string NombreProfesional, decimal? Precio, DateTime? FechaIncio, DateTime? FechaFin);

        Task<List<ReporteServicio>> spGetReporteServicioAsync(string NombreServicio, string NombreProfesional, string NombreCliente, DateTime? FechaIncio, DateTime? FechaFin, string NombreLocal, int? MedioDePago, int? EstadoTurno, int? EstadoPago);

    }
}
