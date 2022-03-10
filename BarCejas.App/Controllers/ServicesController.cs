using BarCejas.Data.Interfaces;
using BarCejas.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BarCejas.App.Controllers
{
    public class ServicesController : Controller
    {
        private readonly IServicioService _servicioService;
        private readonly ICategoriaService _categoriaService;

        public ServicesController(IServicioService servicioService, ICategoriaService categoriaService)
        {
            _servicioService = servicioService;
            _categoriaService = categoriaService;
        }

        [HttpGet]
        [Route("Services/Index")]
        public IActionResult Index()
        {
            ViewBag.Title = "Servicios";
            return View(_categoriaService.GetCategoriaAllClient());
        }

        [HttpGet]
        [Route("Category/{id}")]
        [Route("Services/Category/{id}")]
        public IActionResult ServicesCategory(int id)
        {
            try
            {
                Categoria model = _categoriaService.GetCategoriaById(id);
                if (model is null)
                    return NotFound();
                ViewBag.Title = $"Servicios de {model.Nombre}";
                return View(model);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("Service/{id}")]
        public async Task<IActionResult> ServiceAsync(long id)
        {
            Servicio model = await _servicioService.GetServicioById(id);
            if (model == null || model.Id == 0)
                return RedirectToAction("Index", "Home");
            return View(model);
            //return View();
        }

    }
}
