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
    public class SearchController : Controller
    {

        private readonly ILogger<SearchController> _logger;
        public IDoctorService DoctorService { get; }

        public SearchController(ILogger<SearchController> logger, IDoctorService doctorService)
        {
            _logger = logger;
            DoctorService = doctorService;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> SearchResult(string toSearch, int pageNumber = 1)
        {

            ViewBag.toSearch = toSearch;

            var doctors = await DoctorService.GetDoctorSearchResult(toSearch, pageNumber);

            if (doctors.Count() > 0)
            {
                return View(doctors);
            }
            else
            {
                return View(await DoctorService.GetAllDoctorAsync(pageNumber));
            }


            //if (DoctorService.IsValidCity(toSearch))
            //{
            //    return View(await DoctorService.GetDoctorsByCityAsync(toSearch, pageNumber));
            //}

            //if (DoctorService.IsValidDoctor(toSearch))
            //{
            //    return View(await DoctorService.GetDoctorsByNameAsync(toSearch, pageNumber));
            //}

            //if (DoctorService.IsValidSpeciality(toSearch))
            //{
            //    return View(await DoctorService.GetDoctorsBySpeciality(toSearch, pageNumber));
            //}

        }
    }
}
