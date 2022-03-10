using BarCejas.App.Utils;
using BarCejas.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using BarCejas.Entities.Enumerations;
using BarCejas.Entities;
using BarCejas.App.Models;
using OrderViewModel = BarCejas.App.Areas.Admin.Models.OrderViewModel;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;
using BarCejas.App.Areas.Admin.Models;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace BarCejas.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[Controller]")]
    [Authorize(Roles = "Administrador, Profesional")]
    public class GestorShiftsController : BaseController
    {
        private readonly IServicioService _servicioService;
        private readonly IModalidadPagoService _modalidadPagoService;
        private readonly IFormaPagoService _formaPagoService;
        private readonly IContactoLocalService _contactoLocalService;
        private readonly IProfesionalService _profesionalService;
        private readonly IOrderService _orderService;
        private readonly IUsuarioService _usuarioService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly string _imgRuta;

        public GestorShiftsController(IServicioService servicioService, IModalidadPagoService modalidadPagoService, IFormaPagoService formaPagoService, IContactoLocalService contactoLocalService, IProfesionalService profesionalService, IOrderService orderService, IUsuarioService usuarioService, IUnitOfWork unitOfWork, IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _servicioService = servicioService;
            _modalidadPagoService = modalidadPagoService;
            _formaPagoService = formaPagoService;
            _contactoLocalService = contactoLocalService;
            _profesionalService = profesionalService;
            _orderService = orderService;
            _usuarioService = usuarioService;
            _unitOfWork = unitOfWork;
            _hostingEnvironment = hostingEnvironment;
            _imgRuta = configuration.GetValue<string>("RutaImg");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            Usuario user = GetUserIdentity();

            ViewBag.ModalidadesPago = Util.GetItemsByEnumMedioPago();
            ViewBag.EstatusOrden = Util.GetItemsByEnumEstatusOrden();
            ViewBag.EstatusPago = Util.GetItemsByEnumEstatusPago();
            ViewBag.Servicios = Util.GetServicioSelectListItems(_servicioService.GetServicioAll());
            ViewBag.Profesionales = Util.GetProfesionalSelectListItems(await _profesionalService.GetProfesionalAllActive());

            ViewData["IsAdmin"] = user.IdTipoUsuario == (int)RoleType.Profesional ? false : true;


            List<OrdenesVigentes> ordenes = await _orderService.GetCurrentOrders(-1, -1, -1, -1, -1, -1, DateTime.Now, DateTime.Now.AddMonths(1));

            return View(ordenes.AsEnumerable());
        }

        [HttpPost]
        [Route("SearchTurns")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> List([FromForm] OrdenFilterViewModel filer)
        {
            try
            {
                Usuario user = GetUserIdentity();
                ViewData["IsAdmin"] = user.IdTipoUsuario == (int)RoleType.Profesional ? false : true;

                filer.FechaInicio = filer.FechaInicio is null || (filer.FechaInicio != null && DateTime.Compare(DateTime.Parse(filer.FechaInicio.ToString()), DateTime.MinValue) == 0) ? DateTime.Now : filer.FechaInicio;
                filer.FechaFin = filer.FechaFin is null || (filer.FechaFin != null && DateTime.Compare(DateTime.Parse(filer.FechaFin.ToString()), DateTime.MinValue) == 0) ? DateTime.Now.AddMonths(1) : filer.FechaFin;
                filer.IdProfesional = filer.IdProfesional > 0 ? filer.IdProfesional : -1;

                List<OrdenesVigentes> ordenes = await _orderService.GetCurrentOrders(-1, filer.IdServicio > 0 ? filer.IdServicio : -1, filer.IdProfesional, filer.IdModalidadPago > 0 ? filer.IdModalidadPago : -1, filer.IdEstatusOrden > 0 ? filer.IdEstatusOrden : -1, filer.IdEstatusPago > 0 ? filer.IdEstatusPago : -1, filer.FechaInicio, filer.FechaFin);
                return PartialView("_TurnosList", ordenes.Take(20));
            }
            catch (Exception ex)
            {
                return BadRequest(new { isCompleted = false, message = "Ha ocurrido un error interno en el servidor.", exception = ex });
            }
        }

        [HttpGet]
        [Route("New")]
        public IActionResult New()
        {
            try
            {
                ViewBag.Servicios = Util.GetServicioSelectListItems(_servicioService.GetServicioAll());
                ViewBag.ModalidadesPago = Util.GetItemsByEnumMedioPago();
                ViewBag.FormasPago = Util.GetItemsByEnumFormaPago();
                return PartialView("_AddOrEdit", new OrderViewModel());
            }
            catch (Exception ex)
            {
                return BadRequest(new { isCompleted = false, message = "Ha ocurrido un error interno en el servidor.", exception = ex });
            }
        }

        [HttpPost]
        [Route("Registre")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registre([FromForm] OrderViewModel model)
        {
            try
            {
                if (model != null)
                {
                    if (model.IdServicio == 0)
                        return BadRequest(new { isCompleted = false, message = "Por favor seleccione el servicio." });
                    if (model.IdContactoLocal == 0)
                        return BadRequest(new { isCompleted = false, message = "Por favor seleccione el local." });
                    if (model.IdProfesional == 0)
                        return BadRequest(new { isCompleted = false, message = "Por favor seleccione el profesional de servicio." });
                    if (model.IdCliente == 0)
                        return BadRequest(new { isCompleted = false, message = "Por favor seleccione el cliente." });
                    if (DateTime.Compare(model.FechaCita, DateTime.Now.Date) < 0)
                        return BadRequest(new { isCompleted = false, message = "Por favor seleccione una fecha valida." });
                    if (string.IsNullOrEmpty(model.Hora))
                        return BadRequest(new { isCompleted = false, message = "Por favor seleccione la hora del turno." });
                    if (model.IdFormaPago == 0)
                        return BadRequest(new { isCompleted = false, message = "Por favor seleccione la forma de pago." });
                    if (model.IdModalidadPago == 0)
                        return BadRequest(new { isCompleted = false, message = "Por favor seleccione la modalidad de pago." });
                    Orden orden = new Orden()
                    {
                        IdCliente = model.IdCliente,
                        IdContactoLocal = model.IdContactoLocal,
                        IdFormaPago = model.IdFormaPago,
                        IdModalidadPago = model.IdModalidadPago,
                        FechaDeCreacion = DateTime.Now,
                        IdEstatus = (int)EstatusOrden.pendiente,
                        IdEstatusPago = (int)EstatusPago.pendiente
                    };
                    orden = await _orderService.InsertOrder(orden);
                    if (orden.Id > 0)
                    {
                        List<OrdenItem> items = new List<OrdenItem>();
                        OrdenItem ordenItem = new OrdenItem()
                        {
                            FechaDeCita = model.FechaCita,
                            Hora = model.Hora,
                            IdOrden = orden.Id,
                            IdProfesional = model.IdProfesional,
                            IdServicio = (int)model.IdServicio,
                            Monto = model.Monto
                        };

                        items.Add(ordenItem);

                        if (await _orderService.InsertOrderItem(items))
                        {
                            Usuario user = GetUserIdentity();
                            ViewData["IsAdmin"] = user.IdTipoUsuario == (int)RoleType.Profesional ? false : true;

                            List<OrdenesVigentes> ordenes = await _orderService.GetCurrentOrders(-1, -1, -1, -1, -1, -1, DateTime.Now, DateTime.Now.AddMonths(1));
                            return PartialView("_TurnosList", ordenes.Take(20));
                        }
                        else
                            return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada." });
                    }
                    else
                        return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada." });
                }
                return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { isCompleted = false, message = "Ha ocurrido un error interno en el servidor.", exception = ex });
            }
        }

        [HttpGet]
        [Route("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                if (id > 0)
                {
                    List<OrdenesVigentes> ordenes = await _orderService.GetCurrentOrders(id, -1, -1, -1, -1, -1, null, null);
                    if (ordenes.Count > 0)
                    {
                        var orden = ordenes.FirstOrDefault();
                        var Profesionales = await _profesionalService.GetProfesionalAll();
                        ViewBag.Action = Actions.Edit;
                        ViewBag.Profesionales = Util.GetProfesionalSelectListItems(Profesionales.Where(x => (bool)x.EsActivo && x.IdContactoLocal == orden.IdContacto && x.ServicioProfesional.Any(y => y.IdServicio == orden.IdServicio)));
                        ViewBag.ContactoLocales = Util.GetContactoLocalSelectListItems(_contactoLocalService.GetContactoLocalAll());
                        ViewBag.Servicios = Util.GetServicioSelectListItems(_servicioService.GetServicioAll());
                        ViewBag.ModalidadesPago = Util.GetItemsByEnumMedioPago();
                        ViewBag.FormasPago = Util.GetItemsByEnumFormaPago();

                        return PartialView("_AddOrEdit", new OrderViewModel(orden));
                    }
                }
                return BadRequest(new { isCompleted = false, message = "Registro no encontrado." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { isCompleted = false, message = "Ha ocurrido un error interno en el servidor.", exception = ex });
            }
        }

        [HttpGet]
        [Route("reschedule/{id}")]
        public async Task<IActionResult> reschedule(int id)
        {
            try
            {
                if (id > 0)
                {
                    List<OrdenesVigentes> ordenes = await _orderService.GetCurrentOrders(id, -1, -1, -1, -1, -1, null, null);
                    if (ordenes.Count > 0)
                    {
                        var orden = ordenes.FirstOrDefault();
                        var Profesionales = await _profesionalService.GetProfesionalAll();
                        ViewBag.Action = Actions.Edit;
                        ViewBag.Profesionales = Util.GetProfesionalSelectListItems(Profesionales.Where(x => (bool)x.EsActivo && x.IdContactoLocal == orden.IdContacto && x.ServicioProfesional.Any(y => y.IdServicio == orden.IdServicio)));
                        ViewBag.ContactoLocales = Util.GetContactoLocalSelectListItems(_contactoLocalService.GetContactoLocalAll());
                        ViewBag.Servicios = Util.GetServicioSelectListItems(_servicioService.GetServicioAll());
                        ViewBag.ModalidadesPago = Util.GetItemsByEnumMedioPago();
                        ViewBag.FormasPago = Util.GetItemsByEnumFormaPago();
                        ViewBag.Reprogramar = true;

                        return PartialView("_AddOrEdit", new OrderViewModel(orden));
                    }
                }
                return BadRequest(new { isCompleted = false, message = "Registro no encontrado." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { isCompleted = false, message = "Ha ocurrido un error interno en el servidor.", exception = ex });
            }
        }

        [HttpPost]
        [Route("Update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] OrderViewModel model)
        {
            try
            {
                if (model != null)
                {
                    if (model.IdServicio == 0)
                        return BadRequest(new { isCompleted = false, message = "Por favor seleccione el servicio." });
                    if (model.IdContactoLocal == 0)
                        return BadRequest(new { isCompleted = false, message = "Por favor seleccione el local." });
                    if (model.IdProfesional == 0)
                        return BadRequest(new { isCompleted = false, message = "Por favor seleccione el profesional de servicio." });
                    if (model.IdCliente == 0)
                        return BadRequest(new { isCompleted = false, message = "Por favor seleccione el cliente." });
                    if (DateTime.Compare(model.FechaCita, DateTime.Now.Date) < 0)
                        return BadRequest(new { isCompleted = false, message = "Por favor seleccione una fecha valida." });
                    if (string.IsNullOrEmpty(model.Hora))
                        return BadRequest(new { isCompleted = false, message = "Por favor seleccione la hora del turno." });
                    if (model.IdFormaPago == 0)
                        return BadRequest(new { isCompleted = false, message = "Por favor seleccione la forma de pago." });
                    if (model.IdModalidadPago == 0)
                        return BadRequest(new { isCompleted = false, message = "Por favor seleccione la modalidad de pago." });
                    OrdenItem ordenItem = new OrdenItem()
                    {
                        Id = model.IdOrdenItem,
                        Hora = model.Hora,
                        IdOrden = model.IdOrden,
                        FechaDeCita = model.FechaCita,
                        IdProfesional = model.IdProfesional,
                    };

                    if (await _orderService.UpdateOrderItem(ordenItem))
                    {
                        Usuario user = GetUserIdentity();
                        ViewData["IsAdmin"] = user.IdTipoUsuario == (int)RoleType.Profesional ? false : true;

                        List<OrdenesVigentes> ordenes = await _orderService.GetCurrentOrders(-1, -1, -1, -1, -1, -1, DateTime.Now, DateTime.Now.AddMonths(1));
                        return PartialView("_TurnosList", ordenes.Take(20));
                    }
                    else
                        return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada." });
                }
                return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { isCompleted = false, message = "Ha ocurrido un error interno en el servidor.", exception = ex });
            }
        }

        [HttpGet]
        [Route("OnChangeService")]
        public async Task<IActionResult> OnChangeService(long id)
        {
            try
            {
                if (id > 0)
                {
                    Servicio model = await _servicioService.GetServicioById(id);
                    if (model != null)
                    {
                        IEnumerable<ContactoLocal> contactoLocal = await _contactoLocalService.GetContactoLocalAllActive();
                        contactoLocal = contactoLocal.Where(x => x.ServicioContactoLocal.Any(y => y.IdServicio == id && (bool)y.EsActivo));
                        if (contactoLocal != null && contactoLocal.Count() > 0)
                            return Ok(new { isCompleted = true, priceList = model.PrecioLista, promotionalPrice = model.PrecioPromocioal, contactItems = Util.GetContactoLocalSelectListItems(contactoLocal) });

                        return BadRequest(new { isCompleted = false, message = "El servicio seleccionado no se encuantra asociado a ningún local." });
                    }
                }

                return BadRequest(new { isCompleted = false, message = "Registro no encontrado." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada.", exception = ex });
            }
        }

        [HttpGet]
        [Route("OnChangeContactLocal")]
        public async Task<IActionResult> OnChangeContactLocal(int id, long idService)
        {
            try
            {
                if (id > 0)
                {
                    IEnumerable<Profesional> profesionals = await _profesionalService.GetProfesionalAll();
                    profesionals = profesionals.Where(x => x.IdContactoLocal == id && (bool)x.EsActivo).Where(x => x.ServicioProfesional.Any(y => y.IdServicio == idService && (bool)y.EsActivo));
                    if (profesionals != null && profesionals.Count() > 0)
                        return Ok(new { isCompleted = true, professionalsItems = Util.GetProfesionalSelectListItems(profesionals) });

                    return BadRequest(new { isCompleted = false, message = "No existen profesionales asociados a ese servicio para ese local." });
                }

                return BadRequest(new { isCompleted = false, message = "Registro no encontrado." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada.", exception = ex });
            }
        }

        [HttpGet]
        [Route("OnChangeDateService")]
        public IActionResult OnChangeDateService(DateTime date, int idService, int idContact, int idProfessional = 0)
        {
            try
            {
                if (DateTime.Compare(date, DateTime.Now.Date) < 0)
                    return BadRequest(new { isCompleted = false, message = "La fecha suministrada no es valida." });

                if (idService > 0 && idContact > 0)
                {
                    ProfessionalHoursViewModel professionalHours = ProfessionalScheduleSearch(idService, 0, 0, idContact, idProfessional, date);
                    List<SelectListItem> hours = new List<SelectListItem>();

                    if (professionalHours.profesionals.Count() > 0)
                    {
                        foreach (var item in professionalHours.profesionals)
                        {
                            if (item.HorarioAtencionProfesional.Count > 0)
                            {
                                int DiaSemana = (int)professionalHours.serviceDate.DayOfWeek == 0 ? (int)professionalHours.serviceDate.DayOfWeek + 7 : (int)professionalHours.serviceDate.DayOfWeek;

                                if (item.HorarioAtencionProfesional.Count(x => x.IdProfesional.Equals(item.Id) && DiaSemana == (x.IdDia)) > 0)
                                {
                                    for (int i = 0; i < item.HorarioAtencionProfesional.First(x => x.IdProfesional.Equals(item.Id) && DiaSemana == (x.IdDia)).HoraFin.Subtract(item.HorarioAtencionProfesional.First(x => x.IdProfesional.Equals(item.Id)).HoraInicio).TotalMinutes; i = i + (int)ViewBag.Duracion)
                                    {
                                        string HourDay = item.HorarioAtencionProfesional.First(x => x.IdProfesional.Equals(item.Id)).HoraInicio.Add(TimeSpan.FromMinutes(i)).ToString(@"hh\:mm").Trim();
                                        if (DateTime.Compare(date, DateTime.Now) > 0)
                                            hours.Add(new SelectListItem() { Text = HourDay, Value = HourDay });
                                        else
                                            if (TimeSpan.Compare(TimeSpan.Parse(HourDay), DateTime.Now.TimeOfDay) > 0)
                                            hours.Add(new SelectListItem() { Text = HourDay, Value = HourDay });

                                    }
                                }
                            }
                        }
                    }
                    if (hours.Count > 0)
                        return Ok(new { isCompleted = true, hours });

                    return BadRequest(new { isCompleted = false, message = "No existen horarios disponibles para esta fecha." });

                }

                return BadRequest(new { isCompleted = false, message = "Registro no encontrado." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada.", exception = ex });
            }
        }

        [HttpGet]
        [Route("CancelShift")]
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
                return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada.", exception = ex });
            }
        }

        [HttpPost("OnChangeEstatus")]
        public async Task<IActionResult> OnChangeEstatus(int IdOrden)
        {
            try
            {
                if (IdOrden > 0)
                    if (await _orderService.ChangeStatusOrder(IdOrden, (int)EstatusOrden.Anulada))
                    {
                        Usuario user = GetUserIdentity();
                        ViewData["IsAdmin"] = user.IdTipoUsuario == (int)RoleType.Profesional ? false : true;

                        List<OrdenesVigentes> ordenes = await _orderService.GetCurrentOrders(-1, -1, -1, -1, -1, -1, DateTime.Now, DateTime.Now.AddMonths(1));
                        return PartialView("_TurnosList", ordenes.Take(20));
                    }
                    else
                        return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada." });

                return BadRequest(new { isCompleted = false, message = "Registro no encontrado." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada.", exception = ex });
            }
        }

        [HttpGet]
        [Route("PrintFormService")]
        public async Task<FileResult> PrintFormService(int id)
        {
            try
            {
                if (id == 0)
                    return null;

                Servicio model = await _servicioService.GetServicioById(id);
                if (model != null)
                {
                    model.RutaFormulario = model.RutaFormulario.Replace('~', ' ').TrimStart();
                    if (System.IO.File.Exists($"{_hostingEnvironment.WebRootPath}{model.RutaFormulario}"))
                    {
                        byte[] bytes = System.IO.File.ReadAllBytes($"{_hostingEnvironment.WebRootPath}{model.RutaFormulario}");

                        string[] file = model.RutaFormulario.Split('/');

                        return File(bytes, "application/octet-stream", file[file.Length - 1]);
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        [HttpGet]
        [Route("Voucher")]
        public IActionResult Voucher(int id)
        {
            try
            {
                if (id == 0)
                    return BadRequest(new { isCompleted = false, message = "Debe seleccionar un turno." });

                Orden orden = _orderService.GetOrdersById(id);
                if (orden != null)
                {
                    return PartialView("_VoucherModal", orden);
                }
                return BadRequest(new { isCompleted = false, message = "El turno seleccionado no es un turno válido." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada.", exception = ex });
            }
        }

        [HttpGet]
        [Route("RecordPayment")]
        public IActionResult RecordPayment(int id)
        {
            try
            {
                ViewBag.FormasPago = Util.GetItemsByEnumFormaPago((int)FormaDePago.Mercado_Pago);
                return PartialView("_RecordPaymentModal");
            }
            catch (Exception ex)
            {
                return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada.", exception = ex });
            }
        }

        [HttpPost("RecordPayment")]
        public async Task<IActionResult> RecordPayment(int Id, int IdFormaPago, IFormFile ArchivoComprobante)
        {
            try
            {
                if (Id > 0 && IdFormaPago > 0)
                {
                    if (IdFormaPago != (int)FormaDePago.Efectivo)
                        if (ArchivoComprobante is null)
                            return BadRequest(new { isCompleted = false, message = "Debe subminstrar el comprobante pago." });

                    string voucher = string.Empty;
                    if (ArchivoComprobante != null && ArchivoComprobante.Length > 0)
                        voucher = await SaveImgAsync(ArchivoComprobante, _imgRuta + "Vouchers");

                    if (await _orderService.RecordPayment(Id, IdFormaPago, voucher))
                    {
                        Usuario user = GetUserIdentity();
                        ViewData["IsAdmin"] = user.IdTipoUsuario == (int)RoleType.Profesional ? false : true;

                        List<OrdenesVigentes> ordenes = await _orderService.GetCurrentOrders(-1, -1, -1, -1, -1, -1, DateTime.Now, DateTime.Now.AddMonths(1));
                        return PartialView("_TurnosList", ordenes.Take(20));
                    }
                    else
                        return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada." });
                }

                return BadRequest(new { isCompleted = false, message = "Seleccione la forma de pago." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada.", exception = ex });
            }
        }

        [HttpGet]
        [Route("Clients")]
        public IActionResult ClientModal()
        {
            try
            {
                return PartialView("_ClientModal");
            }
            catch (Exception ex)
            {
                return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada.", exception = ex });
            }
        }

        [HttpGet]
        [Route("SearchClient")]
        public IActionResult SearchClient([FromQuery] string nombre, [FromQuery] string apellido, [FromQuery] string dni)
        {
            try
            {
                IEnumerable<Usuario> clients = _usuarioService.GetUsuarioByTipo((int)RoleType.Cliente);
                if (!string.IsNullOrEmpty(nombre))
                    clients = clients.Where(x => !string.IsNullOrEmpty(x.Nombre) ? x.Nombre.ToUpper().Contains(nombre.ToUpper()) : false);
                if (!string.IsNullOrEmpty(apellido))
                    clients = clients.Where(x => !string.IsNullOrEmpty(x.Apellido) ? x.Apellido.Contains(apellido) : false);
                if (!string.IsNullOrEmpty(dni))
                    clients = clients.Where(x => !string.IsNullOrEmpty(x.Dni) ? x.Dni.Contains(dni) : false);

                return PartialView("_ClientList", clients.Take(10));
            }
            catch (Exception ex)
            {
                return BadRequest(new { isCompleted = false, message = "La acción no pudo ser completada.", exception = ex });
            }
        }

        private ProfessionalHoursViewModel ProfessionalScheduleSearch(int serviceId, int packageId, int orderId, int sucursalId, int professionalId, DateTime ServiceDate)
        {
            IEnumerable<Profesional> professionals = _profesionalService.GetProfesionalAll().Result.Where(p => p.EsActivo == true);
            IEnumerable<HorarioAtencionProfesional> horarios = _unitOfWork.horarioAtencionProfesionalRepository.GetAll().Where(hp => hp.EsActivo == true);
            IEnumerable<ContactoLocal> contactoLocals = _contactoLocalService.GetContactoLocalAll().Where(cl => cl.EsActivo == true);
            IEnumerable<OrdenItem> ordenItems = _orderService.GetAllOrderItems().Where(x => x.FechaDeCita.Date == ServiceDate.Date && x.IdServicio == serviceId);
            IEnumerable<HorarioBloqueado> horarioBloqueados = _profesionalService.GetBlockScheduleBySchedule(ServiceDate).Result;

            //Busqueda por sucursal y por profesional
            professionals = sucursalId > 0 ? professionals.Where(p => p.IdContactoLocal == sucursalId) : professionals;
            professionals = professionalId > 0 ? professionals.Where(p => p.Id == professionalId) : professionals;

            //HorariosBloqueados
            ordenItems = professionalId > 0 ? ordenItems.Where(oi => oi.IdProfesional == professionalId) : ordenItems;
            horarioBloqueados = professionalId > 0 ? horarioBloqueados.Where(hb => hb.IdProfesional == professionalId) : horarioBloqueados;

            Entities.Servicio servicio = _servicioService.GetServicioById(serviceId).Result;
            ViewBag.Duracion = servicio.Duracion;

            //Propiedades internas
            professionals = professionals.Select(x => new Entities.Profesional
            {
                Descripcion = x.Descripcion,
                EsActivo = x.EsActivo,
                HorarioAtencionProfesional = horarios.Where(h => h.IdProfesional.Equals(x.Id) && h.EsActivo == true).ToList(),
                Id = x.Id,
                IdContactoLocal = x.IdContactoLocal,
                IdContactoLocalNavigation = x.IdContactoLocalNavigation,
                IdUsuario = x.IdUsuario,
                IdUsuarioNavigation = x.IdUsuarioNavigation,
                ServicioProfesional = servicio.ServicioProfesional.Where(sps => sps.IdProfesional.Equals(x.Id)).ToList()
            });

            ProfessionalHoursViewModel professionalsViewModel = new ProfessionalHoursViewModel
            {
                profesionals = professionals.Where(p => p.ServicioProfesional.Count > 0 && p.EsActivo == true),
                sucursales = contactoLocals,
                serviceId = serviceId,
                serviceDate = ServiceDate,
                packageId = packageId,
                orderId = orderId,
                blockedTurns = ordenItems.Select(oi => new BlockedTurnsViewModel
                {
                    Hour = oi.Hora,
                    professionalId = oi.IdProfesional
                }),
                blockedHours = horarioBloqueados.Select(hb => new BlockedHoursViewModel
                {
                    professionalId = hb.IdProfesional,
                    InitialHour = hb.HoraInicio,
                    FinalHour = hb.HoraFin
                })
            };

            return professionalsViewModel;
        }
    }
}
