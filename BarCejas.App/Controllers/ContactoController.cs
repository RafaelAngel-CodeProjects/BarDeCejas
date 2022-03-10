using BarCejas.App.Areas.Admin.Models;
using BarCejas.App.Models;
using BarCejas.Data.Interfaces;
using BarCejas.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarCejas.App.Controllers
{
    public class ContactoController : Controller
    {
        private readonly IContactoLocalService _serviceContacto;
        private readonly IDiaService _serviceDia;

        public ContactoController(IContactoLocalService serviceContacto, IDiaService serviceDia)
        {
            _serviceContacto = serviceContacto;
            _serviceDia = serviceDia;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<ContactoLocal> lContact;
            lContact = await _serviceContacto.GetContactoLocalAllActive();
            List<Dia> lDia = _serviceDia.GetDiasAll().OrderBy(x => x.Id).ToList();

            List<ContactoViewModel> vm = new List<ContactoViewModel>();
            lContact.OrderBy(x => x.Orden).ToList().ForEach(x => vm.Add(new ContactoViewModel(x, lDia)));

            return View(vm);
        }
    }
}
