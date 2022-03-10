using AutoMapper;

using BarCejas.App.Areas.Admin.Models;
using BarCejas.App.Models;
using BarCejas.App.Utils;
using BarCejas.Data.Interfaces;
using BarCejas.Entities;
using BarCejas.Entities.Enumerations;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BarCejas.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[Controller]")]
    [Authorize(Roles = nameof(RoleType.Administrador))]
    public class GestorCustomerController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;

        public GestorCustomerController(IUsuarioService usuarioService, IPasswordHasher passwordHasher, IMapper mapper)
        {
            _usuarioService = usuarioService;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<Usuario> list = _usuarioService.GetUsuarioByTipo((int)RoleType.Cliente);

            return View(list);
        }

        public PartialViewResult List()
        {
            IEnumerable<Usuario> list = _usuarioService.GetUsuarioByTipo((int)RoleType.Cliente);

            return PartialView("_UsuarioList", list);
        }

        [Route("New")]
        public IActionResult New()
        {
            ViewBag.Generes = Util.GetItemsByEnumGenere();
            UserViewModel model = new UserViewModel();
            model.IdTipoUsuario = (int)RoleType.Cliente;
            model.Registro = RegistreType.Email.ToString();
            ViewBag.Title = "Nuevo cliente";
            ViewBag.Action = Actions.New;
            return View("AddOrEdit", model);
        }

        [HttpPost]
        [Route("Registre")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registre(UserViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Usuario newModel = _mapper.Map<Usuario>(model);
                    newModel.Password = _passwordHasher.Hash(newModel.Password);

                    if (await _usuarioService.InsertUsuario(newModel))
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

        [Route("View/{id:int}")]
        public async Task<IActionResult> View(int id)
        {
            ViewBag.Generes = Util.GetItemsByEnumGenere();

            UserViewModel model = _mapper.Map<UserViewModel>(await _usuarioService.GetUsuarioById(id));
            if (model != null)
            {
                ViewBag.Title = "Datos del cliente";
                ViewBag.Action = Actions.View;
                return View("AddOrEdit", model);
            }
            return NotFound("Registro no encontrado");
        }

        [HttpGet]
        [Route("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {

            UserViewModel model = _mapper.Map<UserViewModel>(await _usuarioService.GetUsuarioById(id));
            if (model != null)
            {
                ViewBag.Generes = Util.GetItemsByEnumGenere();
                ViewBag.Title = "Editar cliente";
                ViewBag.Action = Actions.Edit;
                return View("AddOrEdit", model);
            }
            return NotFound("Registro no encontrado");
        }

        [HttpPost]
        [Route("Update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserViewModel model)
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                    Usuario newModel = _mapper.Map<Usuario>(model);
                    //newModel.Password = _passwordHasher.Hash(newModel.Password);

                    if (await _usuarioService.UpdateUsuario(newModel))
                        return Ok(new { isCompleted = true, message = "Acción completada con éxito." });
                    else
                        return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada." });
                //}
                //return BadRequest(new { isCompleted = false, message = "Contenido invalido" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada.", exception = ex });
            }
        }

        [HttpGet("ChangePassword")]
        [Route("{id}")]
        public async Task<IActionResult> ChangePassword(int id)
        {
            Usuario model = await _usuarioService.GetUsuarioById(id);
            if (model != null)
            {
                return PartialView("_ChangePassword", new ChangePasswordViewModel() { Id = model.Id });
            }

            return NotFound(new { isCompleted = false, message = "Registro no encontrado."});
        }

        [HttpPost]
        [Route("ChangePassword")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await _usuarioService.ChangePassword(model.Id, _passwordHasher.Hash(model.Password)))
                        return Ok(new { isCompleted = true, message = "Acción completada con éxito." });
                    else
                        return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada." });
                }
                return BadRequest(new { isCompleted = false, message = "Contenido invalido" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada.", exception = ex });
            }
        }

        [HttpPost("ChangeStatus")]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            try
            {
                if (id > 0)
                {
                    if (await _usuarioService.ChangeStatus(id))
                        return Ok(new { isCompleted = true, message = "Acción completada con éxito." });
                    else
                        return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada." });
                }
                return BadRequest(new { isCompleted = false, message = "Contenido invalido" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada.", exception = ex });
            }
        }

        public async Task GoogleLogin()
        {

            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties()
            {

                RedirectUri = Url.Action("GoogleResponse")
            });
        }

        public async Task<IActionResult> GoogleResponse()
        {

            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var jj = result.Principal.Identities.FirstOrDefault().ToString(); //Prueba
            var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
            {
                claim.Issuer,
                claim.OriginalIssuer,
                claim.Type,
                claim.Value
            });

            // authenticateResult.Principal.FindFirst(ClaimTypes.NameIdentifier)
            var tt = JsonConvert.DeserializeObject<GoogleProfileViewModel>(jj);
            return Json(claims);
        }
    }
}
