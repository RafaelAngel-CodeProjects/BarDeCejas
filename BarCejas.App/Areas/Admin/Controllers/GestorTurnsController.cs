using AutoMapper;

using BarCejas.App.Areas.Admin.Models;
using BarCejas.Data.Interfaces;
using BarCejas.Entities;
using BarCejas.Entities.Enumerations;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarCejas.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[Controller]")]
    [Authorize(Roles = "Administrador, Turnos")]
    public class GestorTurnsController : BaseController
    {
        private readonly IProfesionalService _profesionalService;
        private readonly IOrderService _orderService;
        private readonly IContactoLocalService _contactoLocalService;
        private readonly IServicioService _servicioService;
        private readonly IMapper _mapper;

        public GestorTurnsController(IProfesionalService profesionalService, IOrderService orderService, IContactoLocalService contactoLocalService, IServicioService servicioService, IMapper mapper)
        {
            _profesionalService = profesionalService;
            _orderService = orderService;
            _contactoLocalService = contactoLocalService;
            _servicioService = servicioService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            Usuario user = GetUserIdentity();
            ViewData["IsAdmin"] = user.IdTipoUsuario == (int)RoleType.Profesional ? false : true;

            if (user.IdTipoUsuario == (int)RoleType.Profesional)
            {
                var professional = await _profesionalService.GetProfesionalByIdUsuario(user.Id);
                ViewBag.Profesionales = _profesionalService.GetProfesionalAll().Result.Where(x => x.Id == professional.Id).ToList();
                ViewBag.Locales = _contactoLocalService.GetContactoLocalAll().Where(x => x.Id == professional.IdContactoLocal).ToList();
            }
            else
            {
                ViewBag.Profesionales = _profesionalService.GetProfesionalAll().Result.ToList();
                ViewBag.Locales = _contactoLocalService.GetContactoLocalAll().ToList();
            }

            return View(new ProfesionalViewModel());
        }

        [HttpGet]
        [Route("GetTurns")]
        public async Task<IActionResult> GetTurns(int idProfessional, int idLocal)
        {
            try
            {
                List<Entities.AgendaResult> orderitems = _orderService.spConsultarAgendaTurnosResultAsync(idProfessional, idLocal);
                orderitems = orderitems.Select(x => { x.RutaImagenPaagina = Url.Content(x.RutaImagenPaagina);
                    x.FechaDeCita = new DateTime(x.FechaDeCita.Date.Year,x.FechaDeCita.Date.Month,x.FechaDeCita.Date.Day).AddHours(Convert.ToDouble(x.Hora.Substring(0, 2))).AddMinutes(Convert.ToDouble(x.Hora.Substring(3, 2)));
                    return x; }).ToList();
                return Ok(orderitems);
            }
            catch (Exception e)
            {
                return BadRequest("Ha ocurrido un error del lado del servidor.");
            }
        }
    }
}
