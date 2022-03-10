using BarCejas.Data.Interfaces;
using BarCejas.Entities;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarCejas.App.Controllers
{
    public class FrequentQuestionController : Controller
    {

        private readonly IFrequentQuestion _service;

        public FrequentQuestionController(IFrequentQuestion service)
        {
            _service = service;
        }
        public IActionResult Index()
        {
            IEnumerable<PreguntasFrecuentes> FrequentQuestions;
            FrequentQuestions = _service.GetFrequentAllActive().Result.OrderBy(x => x.Orden).ToList(); ;
            return View(FrequentQuestions);
        }
    }
}
