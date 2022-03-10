using BarCejas.Data.DataContext;
using BarCejas.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BarCejas.Data.Repositories
{
    public class BaseStoredProcedureRepository : IStoredProcedureRepository
    {
        private readonly BardecejasContext _procedures;

        public BaseStoredProcedureRepository(BardecejasContext procedures)
        {
            _procedures = procedures;
        }

        public async Task<List<spGetReportePaqueteResult>> spGetReportePaqueteAsync(string NombrePaquete, string NombreProfesional, string NombreCliente, DateTime? FechaIncio, DateTime? FechaFin, string NombreLocal, int? MedioDePago, int? EstadoTurno, int? EstadoPago)
        {
            try
            {
                return await _procedures.Procedures.spGetReportePaqueteAsync(NombrePaquete, NombreProfesional, NombreCliente, FechaIncio, FechaFin, NombreLocal, MedioDePago, EstadoTurno, EstadoPago);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<spGetReporteServicioResult>> spGetReporteServicioAsync(string NombreServicio, string NombreProfesional, string NombreCliente, DateTime? FechaIncio, DateTime? FechaFin, string NombreLocal, int? MedioDePago, int? EstadoTurno, int? EstadoPago)
        {
            try
            {
                return await _procedures.Procedures.spGetReporteServicioAsync(NombreServicio, NombreProfesional, NombreCliente, FechaIncio, FechaFin, NombreLocal, MedioDePago, EstadoTurno, EstadoPago);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<spGetReporteProfesionalResult>> spGetReporteProfesionalAsync(string NombreServicio, string NombreProfesional, decimal? Precio, DateTime? FechaIncio, DateTime? FechaFin)
        {
            try
            {
                return await _procedures.Procedures.spGetReporteProfesionalAsync(NombreServicio, NombreProfesional, Precio, FechaIncio, FechaFin);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<spConsultarHistoricoOrdenesClienteResult>> spConsultarHistoricoOrdenesClienteAsync(int IdCliente)
        {
            try
            {
                return await _procedures.Procedures.spConsultarHistoricoOrdenesClienteAsync(IdCliente);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<spConsultarOrdenesResult>> spConsultarOrdenesAsync(int IdOrdenIten, int IdServicio, int IdProfesional, int IdModalidadPago, int IdEstatusOrden, int IdEstatusPago, DateTime? FechaInicio, DateTime? FechaFin)
        {
            try
            {
                return await _procedures.Procedures.spConsultarOrdenesAsync(IdOrdenIten, IdServicio, IdProfesional, IdModalidadPago, IdEstatusOrden, IdEstatusPago, FechaInicio, FechaFin);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
