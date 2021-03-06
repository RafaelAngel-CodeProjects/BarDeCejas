using BarCejas.App.Areas.Admin.Models;
using BarCejas.Data.Interfaces;
using BarCejas.Data.Services;
using BarCejas.Entities;
using BarCejas.Entities.Enumerations;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace BarCejas.App.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Authorize(Roles = nameof(RoleType.Administrador))]
    public class ManagerFrequentQuestionController : Controller
    {
        private readonly IFrequentQuestion _service;

        public ManagerFrequentQuestionController(IFrequentQuestion service, IConfiguration configuration) 
        {
            _service = service;
           
        }
        public IActionResult Index(bool IndActivo = false)
        {
            IEnumerable<PreguntasFrecuentes> FrequentQuestion;
            FrequentQuestion = _service.GetAll().OrderBy(x => x.Orden);
            ViewBag.IndActivo = IndActivo;
            return View(FrequentQuestion);
        }

        public async Task<IActionResult> Create_edit(long Id = 0)
        {

            PreguntasFrecuentes model = new PreguntasFrecuentes();
            try
            {
                if (Id > 0)
                {
                    model = await _service.GetFrequentQuestionById(Id);
                    ViewBag.Title = "Editar Pregunta";
                    return View(new PreguntasFrecuentesViewModel(model));
                }
                else
                {
                    ViewBag.Title = "Crear Pregunta";
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return View();
        }

        [HttpPost]
        
        public async Task<IActionResult> OnPostFrequent(/*IFormCollection formCollection*/PreguntasFrecuentesViewModel model)
        {
            var viewstring = string.Empty;
            bool resultado = false;
            PreguntasFrecuentes list = new PreguntasFrecuentes ();
            try
            {
                if (ModelState.IsValid)
                {
                    #region InsertUpdat

                    list = _service.GetAllNoTracking().Where(x => x.Pregunta.Equals(model.Pregunta, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (list != null && list.Id > 0 && list.Id != model.Id)
                        return Json(new { success = false, mensaje = "La pregunta ya existe." });
                    else
                        list = new PreguntasFrecuentes();
                    model.IndEstatus = model.Id == 0 ? model.IndEstatus = true : model.IndEstatus;
                    resultado = model.Id == 0 ? await _service.InsertFrequentQuestion(model) : await _service.UpdateFrequentQuestion(model);
                    if (resultado)
                    {
                        viewstring = "Se ha realizado los cambios.";
                        return Json(new { success = resultado, mensaje = viewstring });
                    }

                    #endregion

                }
                else
                {
                    return Json(new { success = resultado, mensaje = "El campo respuesta es obligatorio." });

                }
                return Json(new { success = resultado, mensaje = viewstring });

            }
            catch (Exception ex)
            {

                viewstring = ex.Message;
            }

            return Json(new { success = false, mensaje = viewstring });
        }

        

        [HttpPost]
       
        public async Task<JsonResult> CambiarEstatusPreguntas(PreguntasFrecuentes pPreg)
        {
            string mensaje;
            bool result;
            try
            {
                result = await _service.ChangeStatus(pPreg.Id, pPreg.IndEstatus);
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
                    result = await _service.DeleteFrequentQuestion(Id, NrOrden);
                    mensaje = "Cambio satisfactorio.";
                }
                else {
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
     
        public async Task<JsonResult> updatePositions(List<PreguntasFrecuentes> preg) {

            string mensaje= "";
            bool result = false;
            try
            {
                if (preg.Count > 0)
                {
                    for (int i = 0; i < preg.Count; i++)
                    {
                        var idorden = i + 1;
                        var newPosition = idorden;
                        preg[i].Orden = newPosition;
                    }
                    //Call method reorder
                    result = await _service.Reorder(preg);
                    return result
                ? Json(new { success = result, mensaje = "Orden actualizado." })
                : Json(new { success = result, mensaje = "Disculpe, ha ocurrido un error." });
                }
                else {
                    _ = Json(new { success = false, mensaje = "Disculpe, ha ocurrido un error." });
                }

                return Json(new { success = result, mensaje = mensaje });

            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }

        [HttpPost]
        public IActionResult ListFrequentQuestion(bool IndActivo = false)
        {
            IEnumerable<PreguntasFrecuentes> Testimonial = IndActivo ? _service.GetFrequentAllActive().Result.OrderBy(x => x.Orden) : _service.GetAll().OrderBy(x => x.Orden);
            ViewBag.IndActivo = IndActivo;
            return PartialView("_List", Testimonial);
        }
    }
    
}
