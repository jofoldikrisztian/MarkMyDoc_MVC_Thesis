using MarkMyDoctor.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MarkMyDoctor.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
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
            return Json(_unitOfWork.DoctorRepository.GetAutoCompleteSearchResults(toSearch));
        }

    }
}
