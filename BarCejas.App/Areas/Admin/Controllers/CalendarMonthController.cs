using BarCejas.Data.Interfaces;
using BarCejas.Entities.Enumerations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarCejas.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[Controller]")]
    [Authorize(Roles = nameof(RoleType.Administrador))]
    public class CalendarMonthController: Controller
    {
        private readonly IServicioService _servicioService;
    private readonly IModalidadPagoService _modalidadPagoService;
    private readonly IFormaPagoService _formaPagoService;
    private readonly IContactoLocalService _contactoLocalService;
    private readonly IProfesionalService _profesionalService;

        public CalendarMonthController(IServicioService servicioService, IModalidadPagoService modalidadPagoService, IFormaPagoService formaPagoService, IContactoLocalService contactoLocalService, IProfesionalService profesionalService)
        {
            _servicioService = servicioService;
            _modalidadPagoService = modalidadPagoService;
            _formaPagoService = formaPagoService;
            _contactoLocalService = contactoLocalService;
            _profesionalService = profesionalService;
        }
        public ActionResult Index()
        {
            return View();
        }

        // GET: CalendarMonthController/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        // GET: CalendarMonthController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: CalendarMonthController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: CalendarMonthController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        // POST: CalendarMonthController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: CalendarMonthController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: CalendarMonthController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
