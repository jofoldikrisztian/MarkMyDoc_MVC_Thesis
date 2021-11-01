using MarkMyDoctor.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Text.RegularExpressions;
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

        public async Task<IActionResult> SearchResult(bool byName, bool byCity, bool bySpeciality, string toSearch, int pageNumber = 1)
        {

            try
            {
                ViewBag.toSearch = toSearch;
                ViewBag.Action = "SearchResult";

       
                    if (toSearch.Contains("dr.", StringComparison.OrdinalIgnoreCase))
                    {
                        toSearch = Regex.Replace(toSearch, @"\A\bDr\b.?", "", RegexOptions.IgnoreCase).Trim();
                    }
                

                var doctors = await unitOfWork.DoctorRepository.GetSearchResultAsync(toSearch, pageNumber, byName, byCity, bySpeciality);

                return View(doctors);
            }
            catch (Exception ex)
            {
                unitOfWork.Dispose();
                _logger.LogError("Hiba a művelet végrehajtása során: {0}", ex.Message);
                return RedirectToAction("NoResult", "Home");
            }

        }


    }
}
