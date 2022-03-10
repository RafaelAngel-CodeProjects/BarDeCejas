using BarCejas.Data.Interfaces;
using BarCejas.Entities.Reporte;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BarCejas.Data.Services
{
    public class ReporteService : IReporteService
    {

        private readonly IUnitOfWork _unitOfWork;

        public ReporteService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ReportePaquete>> GetReporteByPaquete(string NombrePaquete, string NombreProfesional, string NombreCliente, DateTime? FechaIncio, DateTime? FechaFin, string NombreLocal, int? MedioDePago, int? EstadoTurno, int? EstadoPago)
        {
            List<ReportePaquete> list = new List<ReportePaquete>();

            var result = await _unitOfWork.reporteProcedureRepository.spGetReportePaqueteAsync(NombrePaquete, NombreProfesional, NombreCliente, FechaIncio, FechaFin, NombreLocal, MedioDePago, EstadoTurno, EstadoPago);//.spGetReportePaqueteAsync(NombrePaquete, NombreProfesional, NombreCliente, FechaIncio, FechaFin, NombreLocal, MedioDePago, EstadoTurno, EstadoPago);

            foreach (var item in result)
            {
                list.Add(new ReportePaquete
                {
                    IdOrden = item.IdOrden,
                    NombreProfesional = item.NombreProfesional,
                    NombrePaquete = item.NombrePaquete,
                    NombreCliente = item.NombreCliente,
                    Fecha = item.Fecha,
                    NombreLocal = item.NombreLocal,
                    Precio = item.Precio,
                    MedioDePago = Convert.ToInt32(item.MedioDePago),
                    FormaDePago = item.FormaDePago,
                    EstadoTurno = item.EstadoTurno,
                    EstadoPago = item.EstadoPago
                });
            }
            return list;
        }

        public async Task<List<ReporteProfesional>> GetReporteByProfesional(string NombreServicio, string NombreProfesional, decimal? Precio, DateTime? FechaIncio, DateTime? FechaFin)
        {
            List<ReporteProfesional> list = new List<ReporteProfesional>();

            var result = await _unitOfWork.reporteProcedureRepository.spGetReporteProfesionalAsync(NombreServicio, NombreProfesional, Precio, FechaIncio, FechaFin);

            foreach (var item in result)
            {
                list.Add(new ReporteProfesional
                {
                    IdOrden = item.IdOrden,
                    NombreProfesional = item.NombreProfesional,
                    NombreServicio = item.NombreServicio,
                    Precio = item.Precio,
                    Fecha = item.Fecha
                });
            }

            return list;
        }

        public async Task<List<ReporteServicio>> spGetReporteServicioAsync(string NombreServicio, string NombreProfesional, string NombreCliente, DateTime? FechaIncio, DateTime? FechaFin, string NombreLocal, int? MedioDePago, int? EstadoTurno, int? EstadoPago)
        {
            List<ReporteServicio> list = new List<ReporteServicio>();

            var result = await _unitOfWork.reporteProcedureRepository.spGetReporteServicioAsync(NombreServicio, NombreProfesional, NombreCliente, FechaIncio, FechaFin, NombreLocal, MedioDePago, EstadoTurno, EstadoPago);

            foreach (var item in result)
            {
                list.Add(new ReporteServicio
                {
                    IdOrden = item.IdOrden,
                    NombreProfesional = item.NombreProfesional,
                    NombreServicio = item.NombreServicio,
                    NombreCliente = item.NombreCliente,
                    Fecha = item.Fecha,
                    NombreLocal = item.NombreLocal,
                    Precio = item.Precio,
                    MedioDePago = item.MedioDePago,
                    FormaDePago = item.FormaDePago,
                    EstadoTurno = item.EstadoTurno,
                    EstadoPago = item.EstadoPago
                });
            }

            return list;
        }
    }
}
