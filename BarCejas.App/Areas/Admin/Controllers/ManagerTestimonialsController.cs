using BarCejas.App.Areas.Admin.Models;
using BarCejas.Data.Interfaces;
using BarCejas.Entities;
using BarCejas.Entities.Enumerations;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarCejas.App.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Authorize(Roles = nameof(RoleType.Administrador))]
    public class ManagerTestimonialsController : BaseController
    {
        private readonly ITestimonial _service;
        private readonly IBannerService _serviceBanner;
        private readonly string _imgRuta;
        private const long PesoByteToMB = 1048576;

        public ManagerTestimonialsController(ITestimonial service, IConfiguration configuration, IBannerService serviceBanner)
        {
            _imgRuta = configuration.GetValue<string>("RutaImg");
            _service = service;
            _serviceBanner = serviceBanner;
        }

        public IActionResult Index(bool IndActivo=false, bool IndActivoBanner = false)
        {
            IEnumerable<Testimonios> Testimonial = IndActivo ? _service.GetTestimonialAllActive().Result.OrderBy(x => x.Orden) : _service.GetAll().OrderBy(x => x.Orden); 
            ViewBag.IndActivo = IndActivo;

            IEnumerable<Banner> lBanner = IndActivoBanner ? _serviceBanner.GetBannerAll().Where(x => x.EsActivo == IndActivoBanner).OrderBy(x => x.Orden) : _serviceBanner.GetBannerAll().OrderBy(x => x.Orden);
            ViewBag.IndActivoBanner = IndActivoBanner;

            HomeViewModel homeViewModel = new HomeViewModel(lBanner.ToList(), Testimonial.ToList());
            return View(homeViewModel);
        }

        public async Task<IActionResult> Create_edit(long Id = 0)
        {

            Testimonios model = new Testimonios();
            try
            {
                if (Id > 0)
                {
                    model = await _service.GetTestimonialById(Id);
                    ViewBag.Title = "Editar Testimonio";
                    return View(new TestimonioViewModel(model));
                }
                else
                {
                    ViewBag.Title = "Crear Testimonio";
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> OnPostTestimonial(/*Testimonios model, IFormFile RutaImagen*/ TestimonioViewModel model)
        {
            var viewstring = string.Empty;
            bool resultado = false;
          
            try
            {
                if (ModelState.IsValid)
                {
                    #region imagen

                    if (model.Archivo != null && model.Archivo.Length > 0)
                    {
                        if (calcularZise(model.Archivo.Length, 1, PesoByteToMB))
                        {
                            model.RutaImagen = await SaveImgAsync(model.Archivo, _imgRuta + "Testimonios");
                        }
                        else
                        {
                            viewstring = "La imagen del autor no puede exceder de 1MB.";
                            resultado = false;
                            return Json(new { success = resultado, mensaje = viewstring });
                        }
                    }

                    #endregion

                    #region InsertUpdat

                    model.IndActivo = model.Id == 0 ? model.IndActivo = true : model.IndActivo;
                    resultado = model.Id == 0 ? await _service.InsertTestimonial(model) : await _service.UpdateTestimonial(model);
                    if (resultado)
                    {
                        viewstring = "Se ha realizado los cambios.";
                        return Json(new { success = resultado, mensaje = viewstring });
                    }

                    #endregion

                }
                else {
                    viewstring = "Alguno de los campos no son válidos.";
                }
              
                return Json(new { success = resultado, mensaje = viewstring });

            }
            catch (Exception ex)
            {

                throw;
            }

        }



        [HttpPost]
        
        public async Task<JsonResult> CambiarEstatusTestimonio(Testimonios pPreg)
        {
            string mensaje;
            bool result;
            try
            {
                result = await _service.ChangeStatus(pPreg.Id, pPreg.IndActivo);
                mensaje = "Cambio satisfactorio.";
                return result
                    ? Json(new { success = result, mensaje = "Cambio satisfactorio." })
                    : Json(new { success = result, mensaje = "Disculpe, ha ocurrido un error." });

            }
            catch (Exception ex)
            {
                mensaje = "Ha ocurrido un error. " + ex.Message;
                throw;
            }
        }

        public async Task<JsonResult> Delete(long Id, int NrOrden)
        {
            string mensaje;
            bool result;
            try
            {
                if (Id > 0)
                {
                    result = await _service.DeleteTestimonial(Id, NrOrden);
                    mensaje = "Cambio satisfactorio.";
                }
                else
                {
                    mensaje = "Debe seleccionar el item.";
                    result = false;
                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.InnerException.Message);
            }
            return Json(new { success = result, mensaje = mensaje });
        }


        [HttpPost]
      
        public async Task<JsonResult> updatePositions(List<Testimonios> ptestimonios)
        {

            string mensaje = "";
            bool result = false;
            try
            {
                if (ptestimonios.Count > 0)
                {
                    for (int i = 0; i < ptestimonios.Count; i++)
                    {
                        var idorden = i + 1;
                        var newPosition = idorden;
                        ptestimonios[i].Orden = newPosition;
                    }
                    //Call method reorder
                    result = await _service.Reorder(ptestimonios);
                    return result
                ? Json(new { success = result, mensaje = "Orden actualizado." })
                : Json(new { success = result, mensaje = "Disculpe, ha ocurrido un error." });
                }
                else
                {
                    _ = Json(new { success = false, mensaje = "Disculpe, ha ocurrido un error." });
                }

                return Json(new { succes = result, mensaje = mensaje });

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpPost]
        public IActionResult ListTestimonials(bool IndActivo = false) {
            IEnumerable<Testimonios> Testimonial = IndActivo ? _service.GetTestimonialAllActive().Result.OrderBy(x => x.Orden) : _service.GetAll().OrderBy(x => x.Orden);
            ViewBag.IndActivo = IndActivo;
            return PartialView("_List", Testimonial);
        }

        [HttpPost]
        public IActionResult ListBanner(bool IndActivoBanner = false)
        {
            IEnumerable<Banner> lBanner = IndActivoBanner ? _serviceBanner.GetBannerAll().Where(x => x.EsActivo == IndActivoBanner).OrderBy(x => x.Orden) : _serviceBanner.GetBannerAll().OrderBy(x => x.Orden);
            ViewBag.IndActivoBanner = IndActivoBanner;
            return PartialView("_ListBanner", lBanner);
        }
    }
}

