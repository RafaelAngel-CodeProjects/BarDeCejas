using BarCejas.App.Models;
using BarCejas.App.Utils;
using BarCejas.Data.Interfaces;
using BarCejas.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarCejas.App.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IContactoLocalService _branchOfficesService;
        private readonly IProfesionalService _professionalService;

        //Ctor
        public CheckoutController(IOrderService orderService, IContactoLocalService branchOfficeService,
                                  IProfesionalService professionalService)
        {
            _orderService = orderService;
            _branchOfficesService = branchOfficeService;
            _professionalService = professionalService;
        }
      
        //Index
        public ActionResult Index()
        {
            return View();
        }

        //POST: Metodo de Consulta De Horarios Profesionales Filters
        [HttpPost]
        public ActionResult BookingService(ProfessionalScheduleViewModel model)
        {
            IEnumerable<ContactoLocal> BranchOffices = _branchOfficesService.GetContactoLocalAll().Where(cl => cl.EsActivo == true);
            IEnumerable<Profesional> Professionals = _professionalService.GetProfesionalAll().Result.Where(p => p.EsActivo == true);
            model.BranchOffices = BranchOffices;
            model.Professionals = Professionals.Where(x=>x.IdContactoLocal == model.BranchOfficeId);
            model.ProfessionalSchedule = ProfessionalScheduleSearch(model.ServiceId, model.PackageId, model.OrderId, model.BranchOfficeId, model.ProfessionalId, model.ServiceDate);
            return View(model);
        }

        //GET: Metodo Index (La primera vez se obtienen todos los profesionales asociados al Servicio)
        public ActionResult BookingService(int serviceId, int packageId, int orderId)
        {
            IEnumerable<ContactoLocal> BranchOffices = _branchOfficesService.GetContactoLocalAll().Where(cl => cl.EsActivo == true);
            IEnumerable<Profesional> Professionals = _professionalService.GetProfesionalAll().Result.Where(p => p.EsActivo == true);
            ProfessionalScheduleViewModel model = new ProfessionalScheduleViewModel
            {
                ServiceId = serviceId,
                PackageId = packageId,
                OrderId = orderId,
                BranchOfficeId = BranchOffices.First().Id,
                ProfessionalId = Professionals.First().Id,
                ServiceDate = DateTime.Now,
                BranchOffices = BranchOffices
            };

            model.Professionals = Professionals.Where(x => x.IdContactoLocal == model.BranchOfficeId);
            model.ProfessionalSchedule = ProfessionalScheduleSearch(serviceId, packageId, orderId, 0, 0, DateTime.Now);
            return View(model);
        }

        //Metodo Generico de Busqueda
        private List<List<Entities.GetHorariosProfesionalResult>> ProfessionalScheduleSearch(int serviceId, int packageId, int orderId, int sucursalId, int professionalId, DateTime ServiceDate)
        {
            List<Entities.GetHorariosProfesionalResult> horariosProfesionalResults = _orderService.spGetHorariosProfesionalsResultAsync(serviceId,sucursalId,ServiceDate);

            List<List<Entities.GetHorariosProfesionalResult>> GroupedHours = horariosProfesionalResults
                                                                                .GroupBy(hpr => hpr.IdProfesional)
                                                                                .Select(grp => grp.ToList())
                                                                                .ToList();
            return GroupedHours;
        }

    }
}