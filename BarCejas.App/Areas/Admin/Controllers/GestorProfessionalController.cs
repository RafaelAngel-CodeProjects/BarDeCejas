using AutoMapper;

using BarCejas.App.Areas.Admin.Models;
using BarCejas.App.Models;
using BarCejas.App.Utils;
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
    [Route("Admin/[Controller]")]
    [Authorize(Roles = "Administrador, Profesional")]
    public class GestorProfessionalController : Controller
    {
        private readonly IContactoLocalService _contactoLocalService;
        private readonly IProfesionalService _profesionalService;
        private readonly IServicioService _servicioService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IDiaService _diaService;
        private readonly IMapper _mapper;

        public GestorProfessionalController(IContactoLocalService contactoLocalService, IProfesionalService profesionalService, IServicioService servicioService, IPasswordHasher passwordHasher, IDiaService diaService, IMapper mapper)
        {
            _contactoLocalService = contactoLocalService;
            _profesionalService = profesionalService;
            _servicioService = servicioService;
            _passwordHasher = passwordHasher;
            _diaService = diaService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Profesional> list = await _profesionalService.GetProfesionalAll();
            return View(list);
        }

        public async Task<PartialViewResult> List()
        {
            IEnumerable<Profesional> list = await _profesionalService.GetProfesionalAll();

            return PartialView("_ProfesionalList", list);
        }

        [HttpGet]
        [Route("New")]
        public IActionResult New()
        {
            ViewBag.Generes = Util.GetItemsByEnumGenere();
            ViewBag.ServicioProfesional = Util.GetServicioSelectListItems(_servicioService.GetServicioAll());
            ViewBag.ContactoLocales = Util.GetContactoLocalSelectListItems(_contactoLocalService.GetContactoLocalAll());
            ProfesionalViewModel model = new ProfesionalViewModel();
            model.IdUsuarioNavigation = new Usuario();
            model.IdUsuarioNavigation.IdTipoUsuario = (int)RoleType.Profesional;
            model.IdUsuarioNavigation.Registro = RegistreType.Email.ToString();
            ViewBag.Title = "Nuevo profesional";
            ViewBag.Action = Actions.New;
            return View("AddOrEdit", model);
        }

        [HttpPost]
        [Route("Registre")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registre(ProfesionalViewModel model, List<int> ServicioProfesional)
        {
            try
            {

                //if (ModelState.IsValid)
                //{
                // no guardar profesional porque no llega el genero, se coloco asi para poder guardar, temporal
                model.IdUsuarioNavigation.Genero = model.IdUsuarioNavigation.IdTipoGenero == 1 ? "1" : "2";


                model.IdUsuarioNavigation.Password = _passwordHasher.Hash(model.Password);
                ServicioProfesional.ForEach(x => model.ServicioProfesional.Add(new ServicioProfesional()
                {
                    IdServicio = x,
                    EsActivo = true
                }));
                // no guardaba porque esta campo llegaba null
                model.IdUsuarioNavigation.Registro = "";

                if (await _profesionalService.InsertProfesional(model))
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

        [HttpGet]
        [Route("View/{id}")]
        public async Task<IActionResult> View(int id)
        {

            ProfesionalViewModel model = _mapper.Map<ProfesionalViewModel>(await _profesionalService.GetProfesionalById(id));

            if (model != null)
            {
                ViewBag.Generes = Util.GetItemsByEnumGenere();
                ViewBag.ServicioProfesional = Util.GetServicioSelectListItems(_servicioService.GetServicioAll(),model.ServicioProfesional);
                ViewBag.ContactoLocales = Util.GetContactoLocalSelectListItems(_contactoLocalService.GetContactoLocalAll());
                ViewBag.Title = "Datos del profesional";
                ViewBag.Action = Actions.View;
                return View("AddOrEdit", model);
            }
            return NotFound("Registro no encontrado");
        }

        [HttpGet]
        [Route("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {

            ProfesionalViewModel model = _mapper.Map<ProfesionalViewModel>(await _profesionalService.GetProfesionalById(id));

            if (model != null)
            {
                ViewBag.Generes = Util.GetItemsByEnumGenere();
                ViewBag.ServicioProfesional = Util.GetServicioSelectListItems(_servicioService.GetServicioAll(), model.ServicioProfesional);
                ViewBag.ContactoLocales = Util.GetContactoLocalSelectListItems(_contactoLocalService.GetContactoLocalAll());
                ViewBag.Title = "Editar profesional";
                ViewBag.Action = Actions.Edit;
                return View("AddOrEdit", model);
            }
            return NotFound("Registro no encontrado");
        }

        [HttpPost]
        [Route("Update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProfesionalViewModel model, List<int> ServicioProfesional)
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                model.IdUsuarioNavigation.Password = _passwordHasher.Hash(model.Password);

                ServicioProfesional.ForEach(x => model.ServicioProfesional.Add(new ServicioProfesional()
                {
                    IdServicio = x
                }));

                #region Validaciones para los detalles
                List<ServicioProfesional> newListProfesionao = new List<ServicioProfesional>();

                Profesional oldProfesional = await _profesionalService.GetProfesionalById(model.Id);

                oldProfesional.ServicioProfesional.ToList().ForEach(x =>
                {
                    model.ServicioProfesional.ToList().ForEach(y =>
                    {
                        if (y.IdServicio == x.IdServicio)
                        {
                            if (!(bool)x.EsActivo)
                                x.EsActivo = true;
                            else
                                x.EsActivo = false;
                            newListProfesionao.Add(x);
                        }
                    });

                });

                model.ServicioProfesional.ToList().ForEach(x =>
                {
                    if (!oldProfesional.ServicioProfesional.Any(y => y.IdServicio == x.IdServicio))
                    {
                        x.IdProfesional = model.Id;
                        newListProfesionao.Add(x);
                    }
                });


                model.ServicioProfesional = newListProfesionao;
                #endregion

                if (await _profesionalService.UpdateProfesional(model))
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

        [HttpPost("ChangeStatus")]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            try
            {
                if (id > 0)
                    if (await _profesionalService.ChangeStatus(id))
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
        [Route("AttentionHours/{id}")]
        public async Task<IActionResult> AttentionHours(int id)
        {
            try
            {
                ProfesionalViewModel model = _mapper.Map<ProfesionalViewModel>(await _profesionalService.GetProfesionalById(id));

                if (model != null)
                {
                    ViewBag.Title = "Asignar horarios";
                    ViewBag.Dias = _diaService.GetDiasAll().OrderBy(x => x.Id).ToList();
                    ViewBag.Action = model.HorarioAtencionProfesional.Count > 0 ? Actions.Edit : Actions.New;
                    return View("AttentionHours", model);
                }
                return NotFound();
            }
            catch
            {
                return BadRequest("Ha ocurrido un error del lado del servidor.");
            }
        }

        [HttpPost("AttentionHoursPost")]
        public async Task<IActionResult> AttentionHours(ProfesionalViewModel model)
        {
            try
            {
                if (model.HorarioAtencionProfesional.Count(x => x.EsActivo != null && (bool)x.EsActivo && (string.IsNullOrWhiteSpace(x.HoraInicio.ToString()) && string.IsNullOrWhiteSpace(x.HoraFin.ToString()))) > 0 || model.HorarioAtencionProfesional.Count(x => x.EsActivo != null) == 0)
                    return BadRequest(new { isCompleted = false, message = "Por favor indique el horario de atención del profesional." });
                //if (ModelState.IsValid)
                //{
                model.HorarioAtencionProfesional = model.HorarioAtencionProfesional.Where(x => x.EsActivo != null && (!string.IsNullOrWhiteSpace(x.HoraInicio.ToString()) && !string.IsNullOrWhiteSpace(x.HoraFin.ToString()))).ToList();
                if (await _profesionalService.InsertUpdateHorarioProfesional(model))
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

        [HttpGet]
        [Route("BlockSchedule/{id}")]
        public async Task<IActionResult> BlockSchedule(int id)
        {
            try
            {
                ProfesionalViewModel model = _mapper.Map<ProfesionalViewModel>(await _profesionalService.GetProfesionalById(id));

                if (model != null)
                {
                    ViewBag.Title = "Bloquear horarios";
                    ViewBag.HorarioBloqueado = _profesionalService.GetBlockScheduleAll(id).Result.ToList();
                    ViewBag.Action = model.HorarioAtencionProfesional.Count > 0 ? Actions.Edit : Actions.New;
                    return View("BlockSchedule", model);
                }
                return NotFound();
            }
            catch
            {
                return BadRequest("Ha ocurrido un error del lado del servidor.");
            }
        }

        [HttpGet]
        [Route("GetBlockSchedule")]
        public async Task<IActionResult> GetBlockSchedule(int id)
        {
            try
            {
                return Ok(_profesionalService.GetBlockScheduleAll(id).Result.ToList());
            }
            catch
            {
                return BadRequest("Ha ocurrido un error del lado del servidor.");
            }
        }

        [HttpPost("BlockScheduleUpdate")]
        public async Task<IActionResult> BlockScheduleUpdate(DateTime startDate, DateTime endDate, int idProfessional, bool indTodasSemanas)
        {
            try
            {
                if (startDate != Convert.ToDateTime("01/01/1900") && endDate != Convert.ToDateTime("01/01/1900"))
                {
                    ProfesionalViewModel model = _mapper.Map<ProfesionalViewModel>(await _profesionalService.GetProfesionalById(idProfessional));
                    if (indTodasSemanas)
                    {
                        var from = DateTime.Today;
                        var to = Convert.ToDateTime("31/12/2030");
                        var dates = new List<DateTime>();
                        for (var dt = @from; dt <= to; dt = dt.AddDays(1))
                            dates.Add(dt);

                        var startDates = dates.Where(x => x.DayOfWeek == startDate.DayOfWeek).ToList();
                        var endDates = dates.Where(x => x.DayOfWeek == endDate.DayOfWeek).ToList();

                        startDates = startDates.Select(c => { c = c.Date + startDate.TimeOfDay; return c; }).ToList();
                        endDates = endDates.Select(c => { c = c.Date + endDate.TimeOfDay; return c; }).ToList();
                        if (await _profesionalService.BlockScheduleMassive(startDates, endDates, idProfessional))
                            return Ok(new { isCompleted = true, message = "Acción completada con éxito." });
                        else
                            return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada." });
                    }
                    else
                    {
                        if (await _profesionalService.BlockSchedule(startDate, endDate, idProfessional))
                            return Ok(new { isCompleted = true, message = "Acción completada con éxito." });
                        else
                            return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada." });
                    }
                }
                else
                {
                    return BadRequest(new { isCompleted = false, message = "Seleccion un horario" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada.", exception = ex });
            }
        }


        [HttpPost("UnBlockSchedule")]
        public async Task<IActionResult> UnBlockSchedule(long idBlockSchedule)
        {
            try
            {
                if (await _profesionalService.UnBlockSchedule(idBlockSchedule))
                    return Ok(new { isCompleted = true, message = "Acción completada con éxito." });
                else
                    return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada." });

            }
            catch (Exception ex)
            {
                return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada.", exception = ex });
            }
        }
    }
}
