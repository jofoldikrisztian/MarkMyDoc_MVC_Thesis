using MarkMyDoctor.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarkMyDoctor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutoCompleteApiController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public AutoCompleteApiController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        [Produces("application/json")]
        [HttpGet("search")]
        public async Task<IActionResult> Search()
        {
            try
            {
                string term = HttpContext.Request.Query["term"].ToString();
                var search = unitOfWork.DoctorRepository.GetAutoCompleteSearchResults(term);

                return Ok(search);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
