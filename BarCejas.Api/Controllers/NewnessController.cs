using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarCejas.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NewnessController : ControllerBase
    {
        public IActionResult Index()
        {
            return null;
        }
    }
}
