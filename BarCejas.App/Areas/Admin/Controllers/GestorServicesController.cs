using BarCejas.App.Areas.Admin.Models;
using BarCejas.App.Utils;
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
    [Area("Admin")]
    [Route("Admin/[Controller]")]
    [Authorize(Roles = nameof(RoleType.Administrador))]
    public class GestorServicesController : BaseController
    {
        private readonly IServicioService _servicioService;
        private readonly IModalidadPagoService _modalidadPagoService;
        private readonly IFormaPagoService _formaPagoService;
        private readonly ICategoriaService _categoriaService;
        private readonly IContactoLocalService _contactoLocalService;
        private readonly IProfesionalService _profesionalService;
        private readonly string _imgRuta;

        public GestorServicesController(IServicioService servicioService, IModalidadPagoService modalidadPagoService, IFormaPagoService formaPagoService, ICategoriaService categoriaService, IContactoLocalService contactoLocalService, IProfesionalService profesionalService, IConfiguration configuration)
        {
            _servicioService = servicioService;
            _modalidadPagoService = modalidadPagoService;
            _formaPagoService = formaPagoService;
            _categoriaService = categoriaService;
            _contactoLocalService = contactoLocalService;
            _profesionalService = profesionalService;
            _imgRuta = configuration.GetValue<string>("RutaImg");
        }

        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<Servicio> list = _servicioService.GetServicioAll();
            return View(list);
        }

        public PartialViewResult List()
        {
            IEnumerable<Servicio> list = _servicioService.GetServicioAll();
            return PartialView("_ServicioList", list);
        }

        [Route("New")]
        public async Task<IActionResult> New()
        {
            //Servicio model = new Servicio();
            ViewBag.Title = "Nuevo servicio";
            ViewBag.Action = Actions.New;
            ViewBag.ModalidadesPago = Util.GetModalidadPagoSelectListItems(_modalidadPagoService.GetModalidadPagoAll());
            ViewBag.FormasPago = Util.GetFormaPagoSelectListItems(_formaPagoService.GetFormaPagoAll());
            ViewBag.Categorias = Util.GetCategoriaSelectListItems(_categoriaService.GetCategoriaAll());
            ViewBag.ContactoLocales = Util.GetContactoLocalSelectListItems(_contactoLocalService.GetContactoLocalAll());
            ViewBag.Profesionales = Util.GetProfesionalSelectListItems(await _profesionalService.GetProfesionalAll());
            return View("AddOrEdit", new ServicioViewModel(new Servicio()));
        }

        [HttpPost]
        [Route("Registre")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registre(ServicioViewModel model, List<int> ServicioContactoLocal, List<int> ServicioProfesional, List<int> ModalidadPagoServicio, List<int> FormaPagoServicio)
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                Usuario user = GetUserIdentity();

                if (user != null)
                    model.IdUsuarioAlta = user.Id;

                if (model.ArchivoImagenTarjeta != null && model.ArchivoImagenTarjeta.Length > 0)
                    model.RutaImagenTarjeta = await SaveImgAsync(model.ArchivoImagenTarjeta, _imgRuta + "Servicios");

                if (model.ArchivoFormulario != null && model.ArchivoFormulario.Length > 0)
                    model.RutaFormulario = await SaveImgAsync(model.ArchivoFormulario, _imgRuta + "Servicios");

                if (model.ArchivoImagenPaagina != null && model.ArchivoImagenPaagina.Length > 0)
                    model.RutaImagenPaagina = await SaveImgAsync(model.ArchivoImagenPaagina, _imgRuta + "Servicios");
                if (model.ArchivoVideoPagina != null && model.ArchivoVideoPagina.Length > 0)
                    model.RutaVideoPagina = await SaveImgAsync(model.ArchivoVideoPagina, _imgRuta + "Servicios");

                if (string.IsNullOrEmpty(model.RutaImagenTarjeta))
                    return BadRequest(new { isCompleted = false, message = "Imagen tarjeta requerida para completar la acción." });
                if (string.IsNullOrEmpty(model.RutaFormulario))
                    return BadRequest(new { isCompleted = false, message = "Formulario de servicio requerido para completar la acción." });

                ServicioProfesional.ForEach(x => model.ServicioProfesional.Add(new ServicioProfesional()
                {
                    IdProfesional = x,
                    EsActivo = true
                }));
                ServicioContactoLocal.ForEach(x => model.ServicioContactoLocal.Add(new ServicioContactoLocal()
                {
                    IdContactoLocal = x,
                    EsActivo = true
                }));

                ModalidadPagoServicio.ForEach(x => model.ModalidadPagoServicio.Add(new ModalidadPagoServicio()
                {
                    IdModalidadPago = x,
                    EsActivo = true
                }));
                FormaPagoServicio.ForEach(x => model.FormaPagoServicio.Add(new FormaPagoServicio()
                {
                    IdFormaPago = x,
                    EsActivo = true
                }));

                if (await _servicioService.InsertServicio(model))
                    return Ok(new { isCompleted = true, message = "Acción completada con éxito." });
                else
                    return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada." });
                //}
                //return BadRequest(new { isCompleted = false, message = "Contenido invalido." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada.", exception = ex });
            }
        }

        [HttpGet]
        [Route("Edit/{id}")]
        public async Task<IActionResult> Edit(long id)
        {
            Servicio model = await _servicioService.GetServicioById(id);

            if (model != null)
            {
                ViewBag.Title = "Editar servicio";
                ViewBag.Action = Actions.Edit;
                ViewBag.ModalidadesPago = Util.GetModalidadPagoSelectListItems(_modalidadPagoService.GetModalidadPagoAll(), model.ModalidadPagoServicio);
                ViewBag.FormasPago = Util.GetFormaPagoSelectListItems(_formaPagoService.GetFormaPagoAll(), model.FormaPagoServicio);
                ViewBag.Categorias = Util.GetCategoriaSelectListItems(_categoriaService.GetCategoriaAll());
                ViewBag.ContactoLocales = Util.GetContactoLocalSelectListItems(_contactoLocalService.GetContactoLocalAll(), model.ServicioContactoLocal);
                ViewBag.Profesionales = Util.GetProfesionalSelectListItems(await _profesionalService.GetProfesionalAll(), model.ServicioProfesional);
                return View("AddOrEdit", new ServicioViewModel(model));
            }
            return NotFound();
        }

        [HttpPost]
        [Route("Update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ServicioViewModel model, List<int> ServicioContactoLocal, List<int> ServicioProfesional, List<int> ModalidadPagoServicio, List<int> FormaPagoServicio)
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                Usuario user = GetUserIdentity();

                if (user != null)
                    model.IdUsuarioAlta = user.Id;

                if (model.ArchivoImagenTarjeta != null && model.ArchivoImagenTarjeta.Length > 0)
                    model.RutaImagenTarjeta = await SaveImgAsync(model.ArchivoImagenTarjeta, _imgRuta + "Servicios");

                if (model.ArchivoFormulario != null && model.ArchivoFormulario.Length > 0)
                    model.RutaFormulario = await SaveImgAsync(model.ArchivoFormulario, _imgRuta + "Servicios");

                if (model.ArchivoImagenPaagina != null && model.ArchivoImagenPaagina.Length > 0)
                    model.RutaImagenPaagina = await SaveImgAsync(model.ArchivoImagenPaagina, _imgRuta + "Servicios");
                if (model.ArchivoVideoPagina != null && model.ArchivoVideoPagina.Length > 0)
                    model.RutaVideoPagina = await SaveImgAsync(model.ArchivoVideoPagina, _imgRuta + "Servicios");

                if (string.IsNullOrEmpty(model.RutaImagenTarjeta))
                    return BadRequest(new { isCompleted = false, message = "Imagen tarjeta requerida para completar la acción." });
                if (string.IsNullOrEmpty(model.RutaFormulario))
                    return BadRequest(new { isCompleted = false, message = "Formulario de servicio requerido para completar la acción." });

                #region Llenado de objetos 
                ServicioProfesional.ForEach(x => model.ServicioProfesional.Add(new ServicioProfesional()
                {
                    IdProfesional = x,
                }));
                ServicioContactoLocal.ForEach(x => model.ServicioContactoLocal.Add(new ServicioContactoLocal()
                {
                    IdContactoLocal = x,
                }));
                ModalidadPagoServicio.ForEach(x => model.ModalidadPagoServicio.Add(new ModalidadPagoServicio()
                {
                    IdModalidadPago = x
                }));
                FormaPagoServicio.ForEach(x => model.FormaPagoServicio.Add(new FormaPagoServicio()
                {
                    IdFormaPago = x
                }));
                #endregion

                #region Validación para los detalles
                List<ServicioProfesional> newListServicioProfesional = new List<ServicioProfesional>();
                List<ServicioContactoLocal> newListServicioContactoLocal = new List<ServicioContactoLocal>();
                List<ModalidadPagoServicio> newListModalidadPagoServicio = new List<ModalidadPagoServicio>();
                List<FormaPagoServicio> newListFormaPagoServicio = new List<FormaPagoServicio>();
                Servicio oldServicio = await _servicioService.GetServicioById(model.Id);

                #region newListProfesionao

                oldServicio.ServicioProfesional.ToList().ForEach(x =>
                {
                    model.ServicioProfesional.ToList().ForEach(y =>
                    {
                        if (y.IdProfesional == x.IdProfesional)
                        {
                            if (!(bool)x.EsActivo)
                                x.EsActivo = true;
                            else
                                x.EsActivo = false;
                            newListServicioProfesional.Add(x);
                        }
                    });

                });

                model.ServicioProfesional.ToList().ForEach(x =>
                {
                    if (!oldServicio.ServicioProfesional.Any(y => y.IdProfesional == x.IdProfesional))
                    {
                        x.IdServicio = model.Id;
                        newListServicioProfesional.Add(x);
                    }
                });
                #endregion

                #region newListServicioContactoLocal
                oldServicio.ServicioContactoLocal.ToList().ForEach(x =>
                {
                    model.ServicioContactoLocal.ToList().ForEach(y =>
                    {
                        if (y.IdContactoLocal == x.IdContactoLocal)
                        {
                            if (!(bool)x.EsActivo)
                                x.EsActivo = true;
                            else
                                x.EsActivo = false;
                            newListServicioContactoLocal.Add(x);
                        }
                    });

                });

                model.ServicioContactoLocal.ToList().ForEach(x =>
                {
                    if (!oldServicio.ServicioContactoLocal.Any(y => y.IdContactoLocal == x.IdContactoLocal))
                    {
                        x.IdServicio = model.Id;
                        newListServicioContactoLocal.Add(x);
                    }
                });
                #endregion

                #region newListModalidadPagoServicio
                oldServicio.ModalidadPagoServicio.ToList().ForEach(x =>
                {
                    model.ModalidadPagoServicio.ToList().ForEach(y =>
                    {
                        if (y.IdModalidadPago == x.IdModalidadPago)
                        {
                            if (!(bool)x.EsActivo)
                                x.EsActivo = true;
                            else
                                x.EsActivo = false;
                            newListModalidadPagoServicio.Add(x);
                        }
                    });

                });

                model.ModalidadPagoServicio.ToList().ForEach(x =>
                {
                    if (!oldServicio.ModalidadPagoServicio.Any(y => y.IdModalidadPago == x.IdModalidadPago))
                    {
                        x.IdServicio = model.Id;
                        newListModalidadPagoServicio.Add(x);
                    }
                });
                #endregion

                #region newListFormaPagoServicio
                oldServicio.FormaPagoServicio.ToList().ForEach(x =>
                {
                    model.FormaPagoServicio.ToList().ForEach(y =>
                    {
                        if (y.IdFormaPago == x.IdFormaPago)
                        {
                            if (!(bool)x.EsActivo)
                                x.EsActivo = true;
                            else
                                x.EsActivo = false;
                            newListFormaPagoServicio.Add(x);
                        }
                    });

                });

                model.FormaPagoServicio.ToList().ForEach(x =>
                {
                    if (!oldServicio.FormaPagoServicio.Any(y => y.IdFormaPago == x.IdFormaPago))
                    {
                        x.IdServicio = model.Id;
                        newListFormaPagoServicio.Add(x);
                    }
                });
                #endregion

                model.ModalidadPagoServicio = newListModalidadPagoServicio;
                model.FormaPagoServicio = newListFormaPagoServicio;
                model.ServicioProfesional = newListServicioProfesional;
                model.ServicioContactoLocal = newListServicioContactoLocal;
                #endregion

                if (await _servicioService.UpdateServicio(model))
                    return Ok(new { isCompleted = true, message = "Acción completada con éxito." });
                else
                    return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada." });
                //}
                //return BadRequest(new { isCompleted = false, message = "Contenido invalido." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada.", exception = ex });
            }
        }

        [HttpPost("ChangeStatus")]
        public async Task<IActionResult> ChangeStatus(long id)
        {
            try
            {
                if (id > 0)
                {
                    if (await _servicioService.ChangeStatus(id))
                        return Ok(new { isCompleted = true, message = "Acción completada con éxito." });
                    else
                        return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada." });
                }
                return BadRequest(new { isCompleted = false, message = "Contenido invalido." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada.", exception = ex });
            }
        }
    }
}
