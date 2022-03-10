using BarCejas.App.Areas.Admin.Models;
using BarCejas.Data.Interfaces;
using BarCejas.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarCejas.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrador")]
    public class GestorBannerController : BaseController
    {
        private readonly IBannerService _serviceBanner;
        private readonly string _imgRuta;

        public GestorBannerController(IBannerService serviceBanner, IConfiguration configuration)
        {
            _serviceBanner = serviceBanner;
            _imgRuta = configuration.GetValue<string>("RutaImg");
        }

        public IActionResult Index()
        {
            return RedirectToAction("Create_Edit");
        }

        [HttpGet]
        public async Task<IActionResult> Create_Edit(int Id = 0)
        {
            if (Id > 0)
            {
                Banner banner;
                banner = await _serviceBanner.GetBannerById(Id);
                if (banner != null && banner.Id > 0)
                    return View(new BannerViewModel(banner));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create_Edit(BannerViewModel model)
        {
            bool resultado = false, 
                 procesar = true;
            string Mensaje = "Disuclpe, no fue posible procesar su solicitud.", 
                   MensajeError = string.Empty;
            if (model.Id == 0)
            {
                model.EsActivo = true;
                model.RutaImagenDestok = string.Empty;
                model.RutaImagenMobile = string.Empty;
            }

            try
            {
                if (model.ImagenDestok != null && model.ImagenDestok.Length > 0)
                    model.RutaImagenDestok = await SaveImgAsync(model.ImagenDestok, _imgRuta + "Banner");
                if (model.ImagenMobile != null && model.ImagenMobile.Length > 0)
                    model.RutaImagenMobile = await SaveImgAsync(model.ImagenMobile, _imgRuta + "Banner");
                if (string.IsNullOrEmpty(model.TextoBoton))
                    model.TextoBoton = string.Empty;
                if (string.IsNullOrEmpty(model.LinkBoton))
                    model.LinkBoton = string.Empty;

                if (string.IsNullOrEmpty(model.Titulo))
                {
                    procesar = false;
                    Mensaje = "Debe ingresar un títutlo.";
                }
                if (string.IsNullOrEmpty(model.Texto))
                {
                    procesar = false;
                    Mensaje += "Debe ingresar un texto.";
                }
                if (string.IsNullOrEmpty(model.RutaImagenDestok))
                {
                    procesar = false;
                    Mensaje += "Debe ingresar imagen para vista escritorio.";
                }
                if (string.IsNullOrEmpty(model.RutaImagenMobile))
                {
                    procesar = false;
                    Mensaje += "Debe ingresar imagen para vista mobile.";
                }
                if (procesar)
                {
                    model.FechaModif = DateTime.UtcNow;
                    if (model.Id > 0)
                    {
                        resultado = await _serviceBanner.UpdateBanner(model);
                        Mensaje = resultado ? "Registro editado satisfactoriamente." : Mensaje;
                    }
                    else
                    {
                        model.FechaAlta = model.FechaModif;
                        await _serviceBanner.InsertBanner(model);
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

            return Json(new { success = false, mensaje = Mensaje, MensajeError = MensajeError });
        }

        public async Task<IActionResult> Delete(int Id = 0)
        {
            bool success = false;
            string Mensaje = "Disculpe, debe ingresar datos válidos";
            if (Id > 0)
            {
                Banner banner;
                banner = await _serviceBanner.GetBannerById(Id);
                if (banner != null && banner.Id > 0) 
                {
                    banner.EsEliminado = banner.EsEliminado ? false : true;
                    banner.FechaModif = DateTime.Now;
                    success = await _serviceBanner.UpdateBanner(banner);
                    if (success)
                        Mensaje = "Registro actualizado satisfactoriamente.";
                    else
                        Mensaje = "Disculpe, no fue posible actualizar el registro.";
                }
            }
            return Json(new { success = success, mensaje = Mensaje });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int Id = 0, bool EsActivo = false)
        {
            bool success = false;
            string Mensaje = "Disculpe, debe ingresar datos válidos";
            if (Id > 0)
            {
                Banner banner;
                banner = await _serviceBanner.GetBannerById(Id);
                if (banner != null && banner.Id > 0)
                {
                    banner.EsActivo = EsActivo;
                    banner.FechaModif = DateTime.Now;
                    success = await _serviceBanner.UpdateBanner(banner);
                    if (success)
                        Mensaje = "Registro actualizado satisfactoriamente.";
                    else
                        Mensaje = "Disculpe, no fue posible actualizar el registro.";
                }
            }
            return Json(new { success = success, mensaje = Mensaje });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrdenBanner(List<Banner> lBanner)
        {
            bool resultado = false;
            string Mensaje = "Disculpe, no fue posible procesar su solicitud.", mensajeError = string.Empty;
            try
            {
                resultado = await _serviceBanner.UpdateOrdenBanner(lBanner);

                return Json(new { success = resultado, mensaje = Mensaje});
            }
            catch (Exception e)
            {
                mensajeError = e.Message;
            }
            return Json(new { success = false, mensaje = "Disculpe, debe ingresar datos válidos.", mensajeError = mensajeError });
        }


    }
}
