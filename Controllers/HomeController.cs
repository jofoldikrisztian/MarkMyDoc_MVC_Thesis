using MarkMyDoctor.Interfaces;
using MarkMyDoctor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace MarkMyDoctor.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            this.unitOfWork = unitOfWork;
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

        public IActionResult NoResult()
        {
            return View();
        }

        public IActionResult SomethingWentWrong()
        {
            return View();
        }


        [HttpPost]
        public JsonResult AutoComplete(string toSearch)
        {
            return Json(unitOfWork.DoctorRepository.GetAutoCompleteSearchResults(toSearch));
        }

    }
}
