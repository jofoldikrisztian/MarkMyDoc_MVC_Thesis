using MarkMyDoctor.Interfaces;
using MarkMyDoctor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
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

            try
            {
                ViewBag.toSearch = toSearch;
                ViewBag.Action = "SearchResult";

                var doctors = await unitOfWork.DoctorRepository.GetSearchResultAsync(toSearch, pageNumber);

                return View(doctors);
            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
                _logger.LogError("Hiba a művelet végrehajtása során: {0}", ex.Message);
                return RedirectToAction("NoResult", "Home");
            }

        }


    }
}
