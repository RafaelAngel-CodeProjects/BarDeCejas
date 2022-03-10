using BarCejas.Data.Interfaces;
using BarCejas.Data.Services;
using BarCejas.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarCejas.App.Controllers
{
    [AllowAnonymous]
    public class NewnessController : Controller
    {
        private readonly INewnessService _service;
        public NewnessController(INewnessService service, IConfiguration configuration) //IMapper mapper, ILogger<ManagerNewnessController> logger)
        {
            _service = service;
          
        }
        public IActionResult Index()
        {
            IEnumerable<Novedades> Newness;
            Newness = _service.GetAll().OrderByDescending(x=>x.Fecha).Take(6).ToList();
            return View(Newness);
        }

        public async Task<IActionResult> ProfileNewness(long Id = 0) {

            Novedades obj;
            try
            {
                obj = await _service.GetNewnessById(Id);
            }
            catch (Exception ex)
            {

                throw;
            }
            return View(obj);
        }
    }
}
