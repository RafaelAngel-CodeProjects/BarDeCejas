using BarCejas.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BarCejas.Data.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using BarCejas.App.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using BarCejas.Entities.Enumerations;

namespace BarCejas.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = nameof(RoleType.Administrador))]
    public class GestorLocalContactController : Controller
    {
        private readonly IContactoLocalService _serviceContacto;
        private readonly IDiaService _serviceDia;
        private readonly IMediosContactoEmpresaService _serviceMediosContactoEmpresa;

        public GestorLocalContactController(IContactoLocalService serviceContacto, IDiaService serviceDia, IMediosContactoEmpresaService serviceMediosContactoEmpresa)
        {
            _serviceContacto = serviceContacto;
            _serviceDia = serviceDia;
            _serviceMediosContactoEmpresa = serviceMediosContactoEmpresa;
        }

        public IActionResult Index()
        {
            IEnumerable<ContactoLocal> lContact;
            lContact = _serviceContacto.GetContactoLocalAll();

            IEnumerable<MediosContactoEmpresa> lMediosContactoEmpresa = _serviceMediosContactoEmpresa.GetMediosContactoEmpresaAll();

            if (lMediosContactoEmpresa != null && lMediosContactoEmpresa.Count() > 0)
                ViewBag.MediosContactoEmpresa = lMediosContactoEmpresa.First();
            else
                ViewBag.MediosContactoEmpresa = new MediosContactoEmpresa
                {
                    Id = 0,
                    LinkFacebook = string.Empty,
                    LinkInstagram = string.Empty,
                    Whatsapp = string.Empty
                };

            return View(lContact.OrderBy(x => x.Orden));
        }

        public IActionResult _Listado()
        {
            IEnumerable<ContactoLocal> lContact;
            lContact = _serviceContacto.GetContactoLocalAll();
            return PartialView(lContact.OrderBy(x => x.Orden));
        }

        public IActionResult _ItemListado(ContactoLocal obj) {
            return PartialView(obj);
        }

        [HttpGet]
        public async Task<IActionResult> Create_Edit(int Id = 0)
        {
            List<Dia> lDia = _serviceDia.GetDiasAll().OrderBy(x => x.Id).ToList();
            ViewBag.lDia = lDia;
            if (Id > 0)
            {
                ContactoLocal Contact;
                Contact = await _serviceContacto.GetContactoLocalById(Id);
                if (Contact != null && Contact.Id > 0)
                    return View(Contact);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create_Edit(ContactoLocalViewModel model)
        {
            bool resultado = false, procesar = true;
            string Mensaje = "Disuclpe, no fue posible procesar su solicitud.";
            model.EsActivo = model.Id == 0 ? true : model.EsActivo;

            model.Dia = model.Dia.Where(x => x > 0).ToList();
            model.HorarioAtencionLocal = model.HorarioAtencionLocal.Where(x => model.Dia.Any(a => a == x.IdDia)).ToList();

            if (string.IsNullOrEmpty(model.Nombre))
            {
                procesar = false;
                Mensaje = "Debe ingresar un nombre.";
            }
            if (string.IsNullOrEmpty(model.Direccion))
            {
                procesar = false;
                Mensaje += "Debe ingresar una direccción.";
            }
            if (string.IsNullOrEmpty(model.CoordenadaLatitud))
            {
                procesar = false;
                Mensaje += "Debe ingresar la coordenada de latitud.";
            }
            if (string.IsNullOrEmpty(model.CoordenadaLongitud))
            {
                procesar = false;
                Mensaje += "Debe ingresar la coordenada de longitud.";
            }
            if (string.IsNullOrEmpty(model.Telefono))
            {
                procesar = false;
                Mensaje += "Debe ingresar un teléfono.";
            }
            if (model.Dia == null || model.Dia.Count() == 0 || model.HorarioAtencionLocal == null || model.HorarioAtencionLocal.Count() == 0)
            {
                procesar = false;
                Mensaje += "Debe seleccionar los dias del horario del local.";
            }

            if (procesar)
            {
                model.FechaModif = DateTime.UtcNow;
                if (model.Id > 0)
                {
                    resultado = await _serviceContacto.UpdateContactoLocal(model);
                    Mensaje = resultado ? "Registro editado satisfactoriamente." : Mensaje;
                }
                else
                {
                    model.FechaAlta = model.FechaModif;
                    await _serviceContacto.InsertContactoLocal(model);
                    Mensaje = "Registro creado satisfactoriamente.";
                    resultado = true;
                }
                return Json(new { success = resultado, mensaje = Mensaje });
            }
            return Json(new { success = false, mensaje = Mensaje });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int Id = 0)
        {
            bool success = false;
            string Mensaje = "Disculpe, debe ingresar datos válidos";
            if (Id > 0)
            {
                ContactoLocal Contact;
                Contact = await _serviceContacto.GetContactoLocalById(Id);
                if (Contact != null && Contact.Id > 0)
                {
                    Contact.EsEliminado = true;
                    Contact.FechaModif = DateTime.Now;
                    success = await _serviceContacto.UpdateContactoLocal(Contact);
                    if (success)
                        Mensaje = "Registro actualizado satisfactoriamente.";
                    else
                        Mensaje = "Disculpe, no fue posible actualizar el registro.";
                }
            }
            return Json(new { success = success, mensaje = Mensaje });
        }

        public async Task<IActionResult> UpdateStatus(int Id = 0, bool EsActivo = false)
        {
            bool success = false;
            string Mensaje = "Disculpe, debe ingresar datos válidos";
            if (Id > 0)
            {
                ContactoLocal Contact;
                Contact = await _serviceContacto.GetContactoLocalById(Id);
                if (Contact != null && Contact.Id > 0) {
                    Contact.EsActivo = EsActivo;
                    Contact.FechaModif = DateTime.Now;
                    success = await _serviceContacto.UpdateContactoLocal(Contact);
                    if (success)
                        Mensaje = "Registro actualizado satisfactoriamente.";
                    else
                        Mensaje = "Disculpe, no fue posible actualizar el registro.";
                }
            }
            return Json(new { success = success, mensaje = Mensaje });
        }

        [HttpPost]
        public async Task<IActionResult> Create_MediosContactoEmpresa(MediosContactoEmpresa model, List<ContactoLocal> lst)
        {
            bool resultado = false;
            string Mensaje = "Disculpe, no fue posible procesar su solicitud.";
            if (ModelState.IsValid)
            {
                if (model.Id > 0)
                {
                    resultado = await _serviceMediosContactoEmpresa.UpdateMediosContactoEmpresa(model);
                    Mensaje = resultado ? "Registro editado satisfactoriamente." : Mensaje;
                }
                else
                {
                    await _serviceMediosContactoEmpresa.InsertMediosContactoEmpresa(model);
                    Mensaje = "Registro creado satisfactoriamente.";
                    resultado = true;
                }

                if (lst != null && lst.Count > 0) {
                    await _serviceContacto.UpdateOrdenContactoLocal(lst);
                }

                return Json(new { success = resultado, mensaje = Mensaje });
            }
            return Json(new { success = false, mensaje = "Disculpe, debe ingresar datos válidos." });
        }

        private string ObtenerErrorMSJModel(ModelStateDictionary obj)
        {

            return string.Join(", ", obj.Values.SelectMany(v => v.Errors)
                                                      .Select(e => e.ErrorMessage)) + ".";
        }
    }
}
