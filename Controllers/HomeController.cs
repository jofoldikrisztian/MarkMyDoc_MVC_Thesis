using MarkMyDoctor.Data;
using MarkMyDoctor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MarkMyDoctor.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private IDoctorService DoctorService { get; }

        public HomeController(ILogger<HomeController> logger, IDoctorService doctorService)
        {
            _logger = logger;
            DoctorService = doctorService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public JsonResult Search(string toSearch)
        {
            return Json(DoctorService.GetSearchResults(toSearch));
        }
    }
}
