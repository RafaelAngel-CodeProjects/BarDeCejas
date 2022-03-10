using BarCejas.App.Areas.Admin.Models;
using BarCejas.Data.Interfaces;
using BarCejas.Data.Services;
using BarCejas.Entities;
using BarCejas.Entities.Enumerations;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BarCejas.App.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Authorize(Roles = nameof(RoleType.Administrador))]
    public class ManagerNewnessController : BaseController
    {

        private readonly INewnessService _service;
        private readonly string _imgRuta;
        private readonly IViewRenderService _viewRenderService;

       
        public ManagerNewnessController(INewnessService service, IConfiguration configuration, IViewRenderService viewRenderService) //IMapper mapper, ILogger<ManagerNewnessController> logger)
        {
            _service = service;
            _imgRuta = configuration.GetValue<string>("RutaImg");
            _viewRenderService = viewRenderService;
            //_mapper = mapper;
            //_logger = logger;
        }
        public IActionResult Index()
        {
           
            IEnumerable<Novedades> Newness;
            Newness = _service.GetAll().OrderByDescending(x=>x.Fecha);
            return View(Newness);
        } 

        public async Task<IActionResult> Create_edit(long Id = 0) 
        {

            Novedades model = new Novedades();
            try
            {
                if (Id > 0)
                {
                    model = await _service.GetNewnessById(Id);
                    ViewBag.Title = "Editar Novedad";
                    return View(new NovedadesViewModel(model) );
                }
                else {
                    ViewBag.Title = "Crear Novedad";
                }
                
            }
            catch (Exception ex)
            {

                throw;
            }
            
            return View();
        }
        
        [HttpPost]
      
        public async Task<IActionResult> OnPostNovedades(/*IFormCollection formCollection*//*Novedades model, IFormFile RutaImagen*/ NovedadesViewModel model) {
            var viewstring = string.Empty;
            bool resultado = false;
            IEnumerable<Novedades> Newness= new Novedades[] { };
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.Archivo != null && model.Archivo.Length > 0)
                    {
                        model.RutaImagen = await SaveImgAsync(model.Archivo, _imgRuta + "Novedades");

                    }
                    #region InsertUpdat
                    model.IndEstatus = model.Id == 0 ? model.IndEstatus = true : model.IndEstatus;
                    resultado = model.Id == 0 ? await _service.InsertNewness(model) : await _service.UpdateNewness(model);
                    if (resultado)
                    {
                        viewstring = "Cambio satisfactorio.";
                        //Newness = _service.GetAll();
                        //viewstring = await _viewRenderService.RenderToStringAsync("_List", Newness);
                        // return Content(viewstring);
                        return Json(new { success = resultado, mensaje = viewstring });
                    }

                    #endregion

                }
                else {

                   
                    return Json(new { success = resultado, mensaje = "Debe completar los campos requeridos." });

                }
                return Json(new { success = resultado, mensaje = viewstring}); 
                
            }
            catch (Exception ex)
            {

                viewstring = ex.Message;
            }

            return Json(new { success = false, mensaje = viewstring });
        }

        public IActionResult ListadoNovedades() {

            IEnumerable<Novedades> Newness;
            Newness = _service.GetAll();
            return View("_List", Newness);
        }

        [HttpPost]
     
        public async Task<JsonResult> CambiarEstatusNovedad(Novedades pNov)
        {
            string mensaje;
            bool result;
            try
            {
                result = await _service.ChangeStatus(pNov.Id, pNov.IndEstatus);
                mensaje = "Cambio satisfactorio.";
                return result
                    ? Json(new { success = result, mensaje = mensaje})
                    : Json(new { success = result, mensaje = "Disculpe, ha ocurrido un error." });

            }
            catch (Exception ex)
            {
                mensaje = "Ha ocurrido un error. " + ex.Message;
                throw;
            }
        }

        [HttpPost]
    
        public async Task<JsonResult> ActivarIndHome(Novedades pNov)
        {
            string mensaje;
            bool result;
            try
            {
                result = await _service.ActivateIndHome(pNov.Id, pNov.IndHome);
                mensaje = "Cambio satisfactorio.";
                return result
                    ? Json(new { success = result, mensaje = mensaje})
                    : Json(new { success = result, mensaje = "Disculpe, ha ocurrido un error." });
            }
            catch (Exception ex)
            {
                mensaje = "Ha ocurrido un error. " + ex.Message;
                throw;
            }
        }

    }
}
