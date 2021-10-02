using MarkMyDoctor.Data;
using MarkMyDoctor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MarkMyDoctor.Controllers
{
    public class SearchController : Controller
    {

        private readonly ILogger<SearchController> _logger;
        private readonly IUnitOfWork unitOfWork;

        public SearchController(ILogger<SearchController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            this.unitOfWork = unitOfWork;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> SearchResult(string toSearch, int pageNumber = 1)
        {

            ViewBag.toSearch = toSearch;
            ViewBag.Action = "SearchResult";

            var doctors = await unitOfWork.DoctorRepository.GetSearchResultAsync(toSearch, pageNumber);

            if (doctors.Count() > 0)
            {
                return View(doctors);
            }
            else
            {
                return RedirectToAction("NoResult", "Home");
            }
        }

       
    }
}
