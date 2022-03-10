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
    [Area("Admin")]
    [Route("Admin/[Controller]")]
    [Authorize(Roles = nameof(RoleType.Administrador))]
    public class GestorCategoryController : BaseController
    {
        private readonly ICategoriaService _categoriaService;
        private readonly string _imgRuta;

        public GestorCategoryController(ICategoriaService categoriaService, IConfiguration configuration)
        {
            _categoriaService = categoriaService;
            _imgRuta = configuration.GetValue<string>("RutaImg");
        }

        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<Categoria> list = _categoriaService.GetCategoriaAll();
            return View(list);
        }

        public PartialViewResult List()
        {
            IEnumerable<Categoria> list = _categoriaService.GetCategoriaAll();
            return PartialView("_CategoriaList", list);
        }

        [HttpPost]
        [Route("Registre")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registre(CategoriaViewModel model)
        {
            try
            {
                //if (ModelState.IsValid)
                //{

                if (model.ArchivoImagen != null && model.ArchivoImagen.Length > 0)
                    model.RutaImagen = await SaveImgAsync(model.ArchivoImagen, _imgRuta + "Categorias");
                
                if (string.IsNullOrEmpty(model.RutaImagen))
                    return BadRequest(new { isCompleted = false, message = "Imagen de la categoria requerida para completar la acción." });

                if (await _categoriaService.InsertCategoria(model))
                    return Ok(new { isCompleted = true, message = "Acción completada con éxito." });
                else
                    return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada." });
                //}
            }
            catch (Exception ex)
            {
                return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada.", exception = ex });
            }
        }

        [HttpGet]
        [Route("New")]
        public IActionResult New()
        {
            Categoria model = new Categoria();
            ViewBag.Title = "Nueva categoria";
            ViewBag.Action = Actions.New;
            return View("AddOrEdit", model);
        }

        [HttpGet]
        [Route("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            Categoria model = _categoriaService.GetCategoriaById(id);
            if (model != null)
            {
                ViewBag.Title = "Editar categoria";
                ViewBag.Action = Actions.Edit;
                return View("AddOrEdit", new CategoriaViewModel(model));
            }
            return NotFound();
        }

        [HttpPost]
        [Route("Update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoriaViewModel model)
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                if (model.ArchivoImagen != null && model.ArchivoImagen.Length > 0)
                    model.RutaImagen = await SaveImgAsync(model.ArchivoImagen, _imgRuta + "Categorias");

                if (string.IsNullOrEmpty(model.RutaImagen))
                    return BadRequest(new { isCompleted = false, message = "Imagen de la categoria requerida para completar la acción." });

                if (await _categoriaService.UpdateCategoria(model))
                    return Ok(new { isCompleted = true, message = "Acción completada con éxito." });
                else
                    return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada." });
                //}
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("ChangeStatus")]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            try
            {
                if (id > 0)
                    if (await _categoriaService.ChangeStatus(id))
                        return Ok(new { isCompleted = true, message = "Acción completada con éxito." });
                    else
                        return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada." });
                return BadRequest(new { isCompleted = false, message = "Contenido invalido" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
