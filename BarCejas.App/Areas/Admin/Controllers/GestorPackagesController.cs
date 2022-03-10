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
    public class GestorPackagesController : BaseController
    {
        private readonly IServicioService _servicioService;
        private readonly IPaquetesService _paquetesService;
        private readonly string _imgRuta;
        private List<ServicioPaquete> _servicioPaquetes;

        public GestorPackagesController(IServicioService servicioService, IPaquetesService paquetesService, IConfiguration configuration)
        {
            _servicioService = servicioService;
            _paquetesService = paquetesService;
            _imgRuta = configuration.GetValue<string>("RutaImg");
            _servicioPaquetes = new List<ServicioPaquete>();
        }

        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<Paquete> list = _paquetesService.GetPaqueteAll();
            return View(list);
        }

        public PartialViewResult List()
        {
            IEnumerable<Paquete> list = _paquetesService.GetPaqueteAll();
            return PartialView("_PaquetesList", list);
        }

        [HttpGet]
        [Route("New")]
        public IActionResult New()
        {
            ViewBag.Title = "Nuevo paquete de servicios";
            ViewBag.Action = Actions.New;
            ViewBag.Servicios = Util.GetServicioSelectListItems(_servicioService.GetServicioAll());
            return View("AddOrEdit", new PaqueteViewModel(new Paquete()));
        }

        [HttpPost]
        [Route("Registre")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registre(PaqueteViewModel model)
        {
            try
            {

                if (model.ServicioPaquete.Count == 0)
                    return BadRequest(new { isCompleted = false, message = "Debe de agregar al menos un servicio para completar la acción." });

                foreach (var item in model.ServicioPaquete)
                {
                    item.IdServicioNavigation = _servicioService.GetServicioById(Convert.ToInt32(item.IdServicio)).Result;
                }
                //if (ModelState.IsValid)
                //{
                if (model.ArchivoImagenTarjeta != null && model.ArchivoImagenTarjeta.Length > 0)
                    model.RutaImagenTarjeta = await SaveImgAsync(model.ArchivoImagenTarjeta, _imgRuta + "Paquetes");

                if (model.ArchivoImagenHome != null && model.ArchivoImagenHome.Length > 0)
                    model.RutaImagenHome = await SaveImgAsync(model.ArchivoImagenHome, _imgRuta + "Paquetes");

                if (string.IsNullOrEmpty(model.RutaImagenTarjeta))
                    return BadRequest(new { isCompleted = false, message = "Imagen tarjeta requerida para completar la acción." });


                model.PrecioFinal = model.ServicioPaquete.Sum(x => (decimal)x.IdServicioNavigation.PrecioPromocioal * x.Cantidad);

                if (await _paquetesService.InsertPaquete(model))
                    return Ok(new { isCompleted = true, message = "Acción completada con éxito." });
                else
                    return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada." });
                //}
                //else
                //    return BadRequest(new { isCompleted = false, message = "Contenido invalido." });
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
            Paquete model = await _paquetesService.GetPaqueteById(id);
            if (model != null)
            {
                ViewBag.Title = "Editar paquete de servicios";
                ViewBag.Action = Actions.Edit;
                ViewBag.Servicios = Util.GetServicioSelectListItems(_servicioService.GetServicioAll());
                model.PrecioFinal = model.ServicioPaquete.Sum(x => (decimal)x.IdServicioNavigation.PrecioPromocioal * x.Cantidad);
                return View("AddOrEdit", new PaqueteViewModel(model));
            }
            return NotFound();
        }

        [HttpPost]
        [Route("Update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PaqueteViewModel model)
        {
            try
            {
                if (model.ServicioPaquete.Count == 0)
                    return BadRequest(new { isCompleted = false, message = "Debe de agregar al menos un servicio para completar la acción." });

                foreach (var item in model.ServicioPaquete)
                {
                    item.IdServicioNavigation = _servicioService.GetServicioById(Convert.ToInt32(item.IdServicio)).Result;
                }
                //if (ModelState.IsValid)
                //{
                if (model.ArchivoImagenTarjeta != null && model.ArchivoImagenTarjeta.Length > 0)
                    model.RutaImagenTarjeta = await SaveImgAsync(model.ArchivoImagenTarjeta, _imgRuta + "Paquetes");

                if (model.ArchivoImagenHome != null && model.ArchivoImagenHome.Length > 0)
                    model.RutaImagenHome = await SaveImgAsync(model.ArchivoImagenHome, _imgRuta + "Paquetes");

                if (string.IsNullOrEmpty(model.RutaImagenTarjeta))
                    return BadRequest(new { isCompleted = false, message = "Imagen tarjeta requerida para completar la acción." });

                model.PrecioFinal = model.ServicioPaquete.Sum(x => (decimal)x.IdServicioNavigation.PrecioPromocioal * x.Cantidad);

                #region Validación para los detalles
                List<ServicioPaquete> newListServicioPaquete = new List<ServicioPaquete>();

                Paquete oldModel = await _paquetesService.GetPaqueteById(model.Id);

                oldModel.ServicioPaquete.ToList().ForEach(x =>
                {
                    model.ServicioPaquete.ToList().ForEach(y =>
                    {
                        if (y.IdServicio == x.IdServicio)
                        {
                            if (!(bool)x.EsActivo)
                                x.EsActivo = true;
                            else
                                x.EsActivo = false;
                            newListServicioPaquete.Add(x);
                        }
                    });

                });

                model.ServicioPaquete.ToList().ForEach(x =>
                {
                    if (!oldModel.ServicioPaquete.Any(y => y.IdServicio == x.IdServicio))
                    {
                        x.IdPaquete = model.Id;
                        newListServicioPaquete.Add(x);
                    }
                });

                model.ServicioPaquete = newListServicioPaquete;
                #endregion

                if (await _paquetesService.UpdatePaquete(model))
                    return Ok(new { isCompleted = true, message = "Acción completada con éxito." });
                else
                    return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada." });
                //}
                //else
                //    return BadRequest(new { isCompleted = false, message = "Contenido invalido." });
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
                    if (await _paquetesService.ChangeStatus(id))
                        return Ok(new { isCompleted = true, message = "Acción completada con éxito." });
                    else
                        return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada." });
                return BadRequest(new { isCompleted = false, message = "Contenido invalido" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada.", exception = ex });
            }
        }

        [HttpGet]
        [Route("AddPackageServices")]
        public IActionResult AddPackageServices()
        {
            try
            {
                ServicioPaquete model = new ServicioPaquete();
                model.IdServicioNavigation = new Servicio();
                ViewBag.Servicios = Util.GetServicioSelectListItems(_servicioService.GetServicioAll());
                return PartialView("_ServicioPaquete", model);
            }
            catch (Exception ex)
            {
                return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada.", exception = ex });
            }

        }

        [HttpGet("{id}")]
        [Route("PriceService")]
        public async Task<IActionResult> PriceService(long id)
        {
            try
            {
                Servicio model = await _servicioService.GetServicioById(id);
                if (model != null)
                    return Ok(new { listPrice = model.PrecioLista, promotionalPrice = model.PrecioPromocioal, dediscount = model.Descuento is null ? 0 : model.Descuento });

                return NotFound(new { isCompleted = false, message = "Registro no encontrado" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada.", exception = ex });
            }

        }
    }
}
