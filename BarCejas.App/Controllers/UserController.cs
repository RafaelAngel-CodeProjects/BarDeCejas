using BarCejas.App.Models;
using BarCejas.App.Utils;
using BarCejas.Data.Interfaces;
using BarCejas.Entities;
using BarCejas.Entities.Enumerations;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using BarCejas.App.Areas.Admin.Models;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Facebook;
using System.Linq;
using AutoMapper;
using Newtonsoft.Json.Linq;
using BarCejas.App.Areas.Admin.Controllers;
using System.IO;
using System.Collections.Generic;
using System.Security.Principal;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using System.Diagnostics;

namespace BarCejas.App.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IUsuarioService _usuarioService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly IOrderService _orderService;
        private readonly IServicioService _servicioService;
        private readonly IProfesionalService _profesionalService;
        private readonly IContactoLocalService _contactoLocalService;
        private readonly IMapper _mapper;
        private readonly IHistoricoIngresosService _historicoIngresosService;

        public UserController(IConfiguration configuration, ITokenService tokenService, IUsuarioService usuarioService,
            IPasswordHasher passwordHasher, IOrderService orderService, IServicioService servicioService, IProfesionalService profesionalService, IContactoLocalService contactoLocalService, IMapper mapper, IHistoricoIngresosService historicoIngresosService)
        {
            _configuration = configuration;
            _tokenService = tokenService;
            _usuarioService = usuarioService;
            _passwordHasher = passwordHasher;
            _orderService = orderService;
            _profesionalService = profesionalService;
            _contactoLocalService = contactoLocalService;
            _servicioService = servicioService;
            _mapper = mapper;
            _historicoIngresosService = historicoIngresosService;
        }

        [AllowAnonymous]
        public PartialViewResult ModalRegisterLogin(RegistreViewModel obj = null, bool pLogin = false)
        {
            ViewBag.Generes = Util.GetItemsByEnumGenere();
            if (obj != null && obj.Email != null)
            {
                ViewBag.IsLogin = pLogin;
                ViewBag.User = obj;
            }
            return PartialView("_ModalRegisterLogin");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registre(RegistreViewModel model)
        {
            string viewstring = "";
            bool success = true;
            try
            {

                if (ModelState.IsValid || model.Registro != RegistreType.Email.ToString())
                {
                    if (model.Registro != RegistreType.Email.ToString())
                    {
                        if (model.Telefono is null)
                        {
                            success = false;
                            viewstring += " Debe ingresar un número de teléfono.";
                        }
                        if (model.IdTipoGenero == 0)
                        {
                            success = false;
                            viewstring += " Debe seleccionar un género";
                        }
                        if (!model.LoginOrRegistre)
                        {
                            if (model.Password is null)
                            {
                                success = false;
                                viewstring += " Debe ingresar una contraseña.";
                            }
                            else
                            {
                                if (!model.Password.Equals(model.ConfirmarPassword))
                                {
                                    success = false;
                                    viewstring += " Las contraseñas no coinciden.";
                                }

                            }


                        }
                    }
                    if (!success)
                        return Json(new { success = success, mensaje = viewstring });

                    Usuario entity = new Usuario()
                    {
                        Email = model.Email,
                        Genero = model.Genero,
                        Telefono = model.Telefono,
                        Registro = model.Registro,
                        IdTipoGenero = model.IdTipoGenero,
                        IdTipoUsuario = (int)RoleType.Cliente,
                        Password = model.Password != null ? _passwordHasher.Hash(model.Password) : string.Empty,
                        EsActivo = true,
                        Nombre = model.Nombre,
                        Apellido = model.Apellido,
                        FechaNacimiento = model.FechaNacimiento,
                        Avatar = model.Avatar
                    };

                    success = await _usuarioService.InsertUsuario(entity);
                    if (success)
                        viewstring = "Se ha registrado el usuario.";
                    else
                        viewstring = "Ha ocurrido un error al registrar usuario.";
                    string token = _tokenService.BuildToken(_configuration["Authenticarion:SecretKey"].ToString(), _configuration["Authenticarion:Issuer"].ToString(), entity);

                    if (token != null)
                    {
                        HttpContext.Session.SetString("Token", token);
                        //return Ok(true);
                    }
                }
                return Json(new { success = success, mensaje = viewstring });
                //return BadRequest();
            }
            catch (Exception ex)
            {
                return Json(new { success = false, mensaje = ex.Message });
            }
        }

        /// <summary>
        /// lOGIN with google
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> Registre(Usuario model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Usuario entity = new Usuario()
                    {
                        Email = model.Email,
                        Genero = model.Genero,
                        Telefono = model.Telefono,
                        Registro = model.Registro,
                        IdTipoGenero = model.IdTipoGenero,
                        IdTipoUsuario = (int)RoleType.Cliente,
                        Password = _passwordHasher.Hash(model.Password),
                        EsActivo = true
                    };

                    await _usuarioService.InsertUsuario(entity);

                    string token = _tokenService.BuildToken(_configuration["Authenticarion:SecretKey"].ToString(), _configuration["Authenticarion:Issuer"].ToString(), entity);

                    if (token != null)
                    {
                        HttpContext.Session.SetString("Token", token);
                        var result = await LoginUser(model);
                        if (result)
                            RedirectToAction("Index", "Home");
                        else
                            return BadRequest();
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Authorize(Roles = "Administrador, Secretario, Profesional, Cliente")]
        [HttpGet]
        [Route("Edit")]
        public IActionResult Edit()
        {
            Usuario model = GetUserIdentity();

            if (model != null)
            {
                UserViewModel user = _mapper.Map<UserViewModel>(model);
                //return View(model);
                return View(user);
            }
            return NotFound();
        }


        [HttpPost]
        [Route("Update")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Update(UserViewModel model)
        {
            try
            {
                Usuario newModel = _mapper.Map<Usuario>(model);

                if (!string.IsNullOrEmpty(model.ConfirmarPassword) && string.IsNullOrEmpty(model.Avatar))
                    newModel.Password = _passwordHasher.Hash(newModel.Password);

                if (await _usuarioService.UpdateUsuario(newModel))
                    return Ok(new { isCompleted = true, message = "Acción completada con éxito." });
                else
                    return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada.", exception = ex });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Authentication(LoginViewModel model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var validation = await IsValidUser(model);
                    if (validation.Item1)
                    {
                        this.LoginUser(validation.Item2);
                        //string token = _tokenService.BuildToken(_configuration["Authenticarion:SecretKey"].ToString(), _configuration["Authenticarion:Issuer"].ToString(), validation.Item2);
                        //if (token != null)
                        //{
                        //    HttpContext.Session.SetString("Token", token);
                        //    return Ok(true);
                        //}

                        await _historicoIngresosService.InsertHistoricoIngresos(new HistoricoIngresos { EsIngresoWeb = true, FechaIngreso = DateTime.UtcNow.AddHours(-3), IdUsuario = validation.Item2.Id });

                        return Ok(true);
                    }
                    else
                    {
                        return BadRequest(new { isCompleted = false, message = "Usuario o contraseña invalida." });
                    }
                }
                if (string.IsNullOrEmpty(model.Email))
                    return BadRequest(new { isCompleted = false, message = "Por favor indique el email." });
                else
                    return BadRequest(new { isCompleted = false, message = "Por favor indique la contraseña." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada.", exception = ex });
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult CheckToken()
        {
            string token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return (RedirectToAction("Index", "Home"));
            }

            if (!_tokenService.IsTokenValid(_configuration["Authenticarion:SecretKey"].ToString(), _configuration["Authenticarion:Issuer"].ToString(), token))
            {
                return (RedirectToAction("Index", "Home"));
            }

            return (RedirectToAction("Index", "Home"));
        }

        [Authorize]
        public IActionResult Perfil()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> MyTurns()
        {
            Usuario model = GetUserIdentity();

            if (model != null)
            {
                List<HistoricoOrdenesCliente> turnos = await _orderService.GetCustomerOrderHistory(model.Id);

                return View(turnos.AsEnumerable());
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> CancelShift(int id)
        {
            try
            {
                if (id > 0)
                {
                    List<OrdenesVigentes> ordenes = await _orderService.GetCurrentOrders(id, -1, -1, -1, -1, -1, null, null);
                    if (ordenes.Count > 0)
                        return PartialView("_CancelShift", ordenes.FirstOrDefault());
                }

                return BadRequest(new { isCompleted = false, message = "Registro no encontrado" });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    isCompleted = false,
                    message = "La acción no pudo ser completada.",
                    exception = ex
                });
            }
        }

        [HttpPost("ChangeStatus")]
        public async Task<IActionResult> ChangeStatus(int IdOrden)
        {
            try
            {
                if (IdOrden > 0)
                    if (await _orderService.ChangeStatusOrder(IdOrden, (int)EstatusOrden.Anulada))
                        return Ok(new { isCompleted = true, message = "Acción completada con éxito." });
                    else
                        return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada." });

                return BadRequest(new { isCompleted = false, message = "Registro no encontrado" });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    isCompleted = false,
                    message = "La acción no pudo ser completada.",
                    exception = ex
                });
            }
        }

        #region LogGoogle
        [AllowAnonymous]
        public async Task GoogleLogin(bool pLogin = false)
        {

            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties()
            {

                RedirectUri = Url.Action("GoogleResponse", "User", new { pLogin = pLogin })
            });
        }
        [AllowAnonymous]
        public async Task<IActionResult> GoogleResponse(bool pLogin = false)
        {
            var user = new RegistreViewModel();
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //var jj = result.Principal.Identities.FirstOrDefault().ToString(); //Prueba
            var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
            {
                claim.Issuer,
                claim.OriginalIssuer,
                claim.Type,
                claim.Value
            });
            if (result.Succeeded && result.Principal.Claims.Count() > 0)
            {
                user.Nombre = result.Principal.FindFirst(ClaimTypes.GivenName).Value;
                user.Apellido = result.Principal.FindFirst(ClaimTypes.Surname).Value;
                user.Email = result.Principal.FindFirst(ClaimTypes.Email).Value;
                user.Registro = RegistreType.Gmail.ToString();
                user.Avatar = result.Principal.Claims.LastOrDefault().Value;
                user.IdTipoUsuario = (int)RoleType.Cliente;
                user.LoginOrRegistre = pLogin;
                var userFind = await _usuarioService.GetUsuarioByEmail(user.Email);
                if (userFind != null) //El usuario existe en BD
                {
                    userFind.Avatar = user.Avatar;
                    //Generar token
                    string token = _tokenService.BuildToken(_configuration["Authenticarion:SecretKey"].ToString(), _configuration["Authenticarion:Issuer"].ToString(), _mapper.Map<Usuario>(user));
                    if (token != null)
                    {
                        if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
                        {
                            HttpContext.Session.SetString("Token", token);
                        }

                        var UserClaims = await LoginUser(userFind);
                        if (UserClaims)
                            return RedirectToAction("Index", "Home");
                    }
                    return BadRequest();

                }//Se debe registrar
                else
                {
                    var jsonUser = JsonConvert.SerializeObject(user);

                    TempData["User"] = jsonUser;
                    TempData["IsLogin"] = pLogin;
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return BadRequest();
            }

            // return BadRequest();
            //return Url.Action("Registre", user);

        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
              CookieAuthenticationDefaults.AuthenticationScheme);
            //No creo que sea esto
            //HttpContext.Session.Clear();
            //if (Request.Cookies.Count > 0)
            //{
            //    foreach (var cookie in Request.Cookies.Keys)
            //    {
            //        Response.Cookies.Delete(cookie);
            //    }
            //}
            return RedirectToAction("Index", "Home");

        }

        public async Task<IActionResult> _RegistreForm(RegistreViewModel obj)
        {

            return PartialView(obj);
        }
        #endregion

        #region LogFacebook

        [AllowAnonymous]
        //[Route("facebook-login")]
        public async Task FacebookLogin(bool pLogin = false)
        {

            await HttpContext.ChallengeAsync(FacebookDefaults.AuthenticationScheme, new AuthenticationProperties()
            {

                RedirectUri = Url.Action("FacebookResponse", "User", new { pLogin = pLogin })
            });
        }

        [AllowAnonymous]
        public async Task<IActionResult> FacebookResponse(bool pLogin = false)
        {
            var user = new RegistreViewModel();
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //var jj = result.Principal.Identities.FirstOrDefault().ToString(); //Prueba
            var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
            {
                claim.Issuer,
                claim.OriginalIssuer,
                claim.Type,
                claim.Value
            });
            if (result.Succeeded && result.Principal.Claims.Count() > 0)
            {
                user.Nombre = result.Principal.FindFirst(ClaimTypes.GivenName).Value;
                user.Apellido = result.Principal.FindFirst(ClaimTypes.Surname).Value;
                user.Email = result.Principal.FindFirst(ClaimTypes.Email).Value;
                user.Avatar = result.Principal.Claims.LastOrDefault().Value;
                user.Registro = RegistreType.Facebook.ToString();
                user.IdTipoUsuario = (int)RoleType.Cliente;
                user.LoginOrRegistre = pLogin;
                var userFind = await _usuarioService.GetUsuarioByEmail(user.Email);
                if (userFind != null) //El usuario existe en BD
                {
                    userFind.Avatar = user.Avatar;
                    //Generar token
                    string token = _tokenService.BuildToken(_configuration["Authenticarion:SecretKey"].ToString(), _configuration["Authenticarion:Issuer"].ToString(), _mapper.Map<Usuario>(user));
                    if (token != null)
                    {
                        if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
                        {
                            HttpContext.Session.SetString("Token", token);
                        }

                        var UserClaims = await LoginUser(userFind);
                        if (UserClaims)
                            return RedirectToAction("Index", "Home");
                    }
                    return BadRequest();

                }//Se debe registrar
                else
                {
                    var jsonUser = JsonConvert.SerializeObject(user);

                    TempData["User"] = jsonUser;
                    TempData["IsLogin"] = pLogin;
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        #endregion

        #region Validación y autenticación de usuario
        private async Task<(bool, Usuario)> IsValidUser(LoginViewModel model)
        {
            try
            {
                var user = await _usuarioService.GetUsuarioByEmail(model.Email);
                if (user != null)
                    return (_passwordHasher.Check(user.Password, model.Password), user);
                else
                    return (false, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //---------------------------------------------------------------------------------------------

        private async Task<bool> LoginUser(Usuario obj)
        {
            string RolTipoUsuario = Enum.GetName(typeof(RoleType), obj.IdTipoUsuario);
            try
            {
                var identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Email, obj.Email),
                    new Claim(ClaimTypes.Name, JsonConvert.SerializeObject(obj)),
                    new Claim(ClaimTypes.Role,  RolTipoUsuario),
                }, CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Email, ClaimTypes.Role);

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }


        #endregion

        #region Recuperacion de clave
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RecoverKey(string Id)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(Id))
                {
                    Usuario model = await _usuarioService.GetUsuarioByEmail(Id);
                    if (model != null)
                    {
                        if ((bool)model.EsActivo)
                        {
                            return Ok(new { isCompleted = SendEmail("ConfirmationEmail", "Administrador", "ConfirmationEmail", "Recuperación de contraseña", model), message = "Por favor verifique su bandeja de entrada." });
                        }
                        else
                        {
                            return BadRequest(new { isCompleted = false, message = "Disculpe, pero el usuario se encuentra inactivo para realizar esta operación." });
                        }
                    }
                    else
                    {
                        return BadRequest(new { isCompleted = false, message = "Por favor especifique un correo electrónico valido." });
                    }
                }
                else
                {
                    return BadRequest(new { isCompleted = false, message = "Por favor especifique el correo electrónico." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { isCompleted = false, message = "Disculpe, la acción no pudo ser completada.", exception = ex });
            }
        }


        [AllowAnonymous]
        [HttpGet("ConfirmationEmail")]
        [Route("{id}")]
        public async Task<IActionResult> ConfirmationEmail(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                Usuario user = await _usuarioService.GetUsuarioByEmail(id);

                if (user != null)
                {
                    //user.ConfirmacionPassword = string.Empty;
                    TempData["IndValidate"] = true; TempData["User"] = user;
                }
            }
            else
            {
                TempData["IndValidate"] = false; TempData["message"] = "Operación fallida";
            }
            return RedirectToAction("Index", "Administrador");
        }

        private bool SendEmail(string actionName, string controllerName, string emailTemplate, string emailSubject, Usuario user)
        {
            var callbackUrl = Url.Action(actionName, controllerName, new { Id = user.Id }, protocol: Request.Scheme);
            var message = EMailTemplate(emailTemplate);
            message = message.Replace("@ViewBag.Link", callbackUrl);
            return EmailHelper.EnviarEmail(user.Email, emailSubject, message);
        }

        private string EMailTemplate(string template)
        {
            //var templateFilePath = HostingEnvironment.MapPath("~/Views/Shared/") + template + ".cshtml";
            //StreamReader objstreamreaderfile = new StreamReader(templateFilePath);
            //var body = objstreamreaderfile.ReadToEnd();
            //objstreamreaderfile.Close();
            //return body;
            return string.Empty;
        }
        #endregion
    }
}