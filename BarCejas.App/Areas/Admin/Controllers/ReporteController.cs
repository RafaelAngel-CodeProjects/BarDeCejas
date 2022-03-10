using BarCejas.App.Areas.Admin.Models;
using BarCejas.Data.Interfaces;
using BarCejas.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarCejas.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrador, Profesional")]
    public class ReporteController : BaseController
    {
        private readonly IHistoricoIngresosService _historicoIngresosService;
        private readonly IUsuarioService _usuarioService;
        private readonly IReporteService _reporteService;

        public ReporteController(IHistoricoIngresosService historicoIngresosService, IUsuarioService usuarioService, IReporteService reporteService)
        {
            _historicoIngresosService = historicoIngresosService;
            _usuarioService = usuarioService;
            _reporteService = reporteService;
        }

        public IActionResult Index()
        {
            return RedirectToAction("ReporteDashboard");
        }

        public async Task<IActionResult> ReporteDashboard()
        {
            List<HistoricoIngresos> lHistoricoIngresos = _historicoIngresosService.GetHistoricoIngresosAll().ToList();
            ReporteDhasboardViewModel Dhasboard = new ReporteDhasboardViewModel();
            ViewBag.lHistoricoIngresos = lHistoricoIngresos;
            ViewBag.Cant_IngesosWeb = lHistoricoIngresos.Count();
            ViewBag.Cant_IngesosAPP = 0;
            ViewBag.lstMes = lHistoricoIngresos.GroupBy(x => x.FechaIngreso.Month);
            ViewBag.Cant_Usuario = _usuarioService.GetUsuarioAll().Count();
            var lstServicio = await _reporteService.spGetReporteServicioAsync("", "", "", null, null, "", 0, 0, 0);
            DateTime fecInicio = new DateTime(DateTime.UtcNow.AddHours(-3).Year, DateTime.UtcNow.AddHours(-3).Month, 1);
            ViewBag.Cant_ServicioMes = lstServicio.Where(x => x.Fecha >= fecInicio).Count();

            for (int i = 1; i <= 12; i++)
            {
                Dhasboard.lReporteMes
                    .Add(new ReporteMes
                    {
                        Mes = i,
                        CantMes = lHistoricoIngresos.Where(x => x.FechaIngreso.Month == i).Count()
                    });
            }

            return View(Dhasboard);
        }

        public async Task<IActionResult> ReportePaquete(string NombrePaquete = "", string NombreProfesional = "", string NombreCliente = "", DateTime? FechaIncio = null, DateTime? FechaFin = null, string NombreLocal = "", int MedioDePago = 0, int EstadoTurno = 0, int EstadoPago = 0)
        {
            var list = await _reporteService.GetReporteByPaquete(NombrePaquete, NombreProfesional, NombreCliente, FechaIncio, FechaFin, NombreLocal, MedioDePago, EstadoTurno, EstadoPago);
            return View(list);
        }

        public async Task<IActionResult> _ListadoReportePaquete(string NombrePaquete = "", string NombreProfesional = "", string NombreCliente = "", DateTime? FechaIncio = null, DateTime? FechaFin = null, string NombreLocal = "", int MedioDePago = 0, int EstadoTurno = 0, int EstadoPago = 0)
        {
            if (string.IsNullOrEmpty(NombrePaquete))
                NombrePaquete = string.Empty;
            if (string.IsNullOrEmpty(NombreProfesional))
                NombreProfesional = string.Empty;
            if (string.IsNullOrEmpty(NombreCliente))
                NombreCliente = string.Empty;
            if (string.IsNullOrEmpty(NombreCliente))
                NombreLocal = string.Empty;

            return PartialView(await _reporteService.GetReporteByPaquete(NombrePaquete, NombreProfesional, NombreCliente, FechaIncio, FechaFin, NombreLocal, MedioDePago, EstadoTurno, EstadoPago));
        }

        //---------------

        public async Task<IActionResult> ReporteServicio(string NombreServicio = "", string NombreProfesional = "", string NombreCliente = "", DateTime? FechaIncio = null, DateTime? FechaFin = null, string NombreLocal = "", int? MedioDePago = 0, int? EstadoTurno = 0, int? EstadoPago = 0)
        {
            return View(await _reporteService.spGetReporteServicioAsync(NombreServicio, NombreProfesional, NombreCliente, FechaIncio, FechaFin, NombreLocal, MedioDePago, EstadoTurno, EstadoPago));
        }

        public async Task<IActionResult> _ListadoReporteServicio(string NombreServicio = "", string NombreProfesional = "", string NombreCliente = "", DateTime? FechaIncio = null, DateTime? FechaFin = null, string NombreLocal = "", int MedioDePago = 0, int EstadoTurno = 0, int EstadoPago = 0)
        {
            if (string.IsNullOrEmpty(NombreServicio))
                NombreServicio = string.Empty;
            if (string.IsNullOrEmpty(NombreProfesional))
                NombreProfesional = string.Empty;
            if (string.IsNullOrEmpty(NombreCliente))
                NombreCliente = string.Empty;
            if (string.IsNullOrEmpty(NombreLocal))
                NombreLocal = string.Empty;
            return PartialView(await _reporteService.spGetReporteServicioAsync(NombreServicio, NombreProfesional, NombreCliente, FechaIncio, FechaFin, NombreLocal, MedioDePago, EstadoTurno, EstadoPago));
        }

        //---------------

        public async Task<IActionResult> ReporteProfesional(string NombreServicio = "", string NombreProfesional = "", decimal Precio = 0, DateTime? FechaIncio = null, DateTime? FechaFin = null)
        {
            return View(await _reporteService.GetReporteByProfesional(NombreServicio, NombreProfesional, Precio, FechaIncio, FechaFin));
        }

        public async Task<IActionResult> _ListadoReporteProfesional(string NombreServicio = "", string NombreProfesional = "", decimal Precio = 0, DateTime? FechaIncio = null, DateTime? FechaFin = null)
        {
            if (string.IsNullOrEmpty(NombreServicio))
                NombreServicio = string.Empty;
            if (string.IsNullOrEmpty(NombreProfesional))
                NombreProfesional = string.Empty;

            return PartialView(await _reporteService.GetReporteByProfesional(NombreServicio, NombreProfesional, Precio, FechaIncio, FechaFin));
        }
    }
}
