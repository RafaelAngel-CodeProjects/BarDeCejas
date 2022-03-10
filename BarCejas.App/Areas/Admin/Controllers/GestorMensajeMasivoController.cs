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
    [Authorize(Roles = "Administrador")]
    public class GestorMensajeMasivoController : BaseController
    {
        private readonly IMensajeMasivoService _serviceMensajeMasivo;

        public GestorMensajeMasivoController(IMensajeMasivoService serviceMensajeMasivo)
        {
            _serviceMensajeMasivo = serviceMensajeMasivo;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<MensajeMasivo> lMensajeMasivo;
            lMensajeMasivo = await _serviceMensajeMasivo.GetMensajeMasivoAll();
            return View(lMensajeMasivo);
        }

        public async Task<IActionResult> _Listado()
        {
            IEnumerable<MensajeMasivo> lMensajeMasivo;
            lMensajeMasivo = await _serviceMensajeMasivo.GetMensajeMasivoAll();
            return PartialView(lMensajeMasivo);
        }

        [HttpGet]
        public async Task<IActionResult> Create_Edit(int Id = 0)
        {
            List<RoleType> lRol = Enum.GetValues(typeof(RoleType))
                               .Cast<RoleType>()
                               .ToList();

            ViewBag.lRol = lRol;
            if (Id > 0)
            {
                MensajeMasivo MensajeMasivo;
                MensajeMasivo = await _serviceMensajeMasivo.GetMensajeMasivoById(Id);
                if (MensajeMasivo != null && MensajeMasivo.Id > 0)
                    return View(MensajeMasivo);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create_Edit(MensajeMasivo model)
        {
            Usuario usuario = GetUserIdentity();
            bool resultado = false,
                 procesar = true;
            string Mensaje = "Disuclpe, no fue posible procesar su solicitud.",
                   MensajeError = string.Empty;
            if (model.Id == 0)
            {
                model.EsEliminado = false;
            }

            try
            {
                if (string.IsNullOrEmpty(model.Asunto))
                {
                    procesar = false;
                    Mensaje = "Debe ingresar un asunto.";
                }
                if (string.IsNullOrEmpty(model.Mensaje))
                {
                    procesar = false;
                    Mensaje += "Debe ingresar un mensaje.";
                }
                if (model.RolTipoUsuarioMensajeMasivo == null || model.RolTipoUsuarioMensajeMasivo.Count == 0)
                {
                    procesar = false;
                    Mensaje += "Debe seleccionar el tipo de usuarios que recibirá la notificación.";
                }
                else
                {

                    for (int i = 0; i < model.RolTipoUsuarioMensajeMasivo.Count(); i++)
                    {
                        model.RolTipoUsuarioMensajeMasivo.ToArray()[i].EsActivo = true;
                        if (model.Id > 0)
                            model.RolTipoUsuarioMensajeMasivo.ToArray()[i].IdMensajeMasivo = model.Id;
                    }

                }

                if (procesar)
                {
                    model.FechaModificacion = DateTime.Now;
                    model.EsEliminado = false;
                    if (model.Id > 0)
                    {
                        model.IdUsuarioModificacion = usuario.Id;
                        resultado = await _serviceMensajeMasivo.UpdateMensajeMasivo(model);
                        Mensaje = resultado ? "Registro editado satisfactoriamente." : Mensaje;
                    }
                    else
                    {
                        model.IdUsuarioAlta = usuario.Id;
                        model.IdUsuarioModificacion = usuario.Id;
                        model.FechaAlta = model.FechaModificacion;
                        await _serviceMensajeMasivo.InsertMensajeMasivo(model);
                        Mensaje = "Registro creado satisfactoriamente.";
                        resultado = true;
                    }
                    return Json(new { success = resultado, mensaje = Mensaje });
                }
            }
            catch (Exception e)
            {
                MensajeError = e.Message;
            }

            return Json(new { success = resultado, mensaje = Mensaje, MensajeError = MensajeError });
        }

        public async Task<IActionResult> Delete(int Id = 0)
        {
            bool success = false;
            string Mensaje = "Disculpe, debe ingresar datos válidos";
            if (Id > 0)
            {
                MensajeMasivo mensajeMasivo = new MensajeMasivo();
                mensajeMasivo = await _serviceMensajeMasivo.GetMensajeMasivoById(Id);
                if (mensajeMasivo != null && mensajeMasivo.Id > 0)
                {
                    mensajeMasivo.EsEliminado = mensajeMasivo.EsEliminado ? false : true;
                    mensajeMasivo.FechaModificacion = DateTime.Now;
                    success = await _serviceMensajeMasivo.UpdateMensajeMasivo(mensajeMasivo);
                    if (success)
                        Mensaje = "Registro actualizado satisfactoriamente.";
                    else
                        Mensaje = "Disculpe, no fue posible actualizar el registro.";
                }
            }
            return Json(new { success = success, mensaje = Mensaje });
        }
    }
}
