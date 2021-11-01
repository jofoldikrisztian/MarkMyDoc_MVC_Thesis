using MarkMyDoctor.Interfaces;
using MarkMyDoctor.Models.ViewModels;
using MarkMyDoctor.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MarkMyDoctor.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAppEmailSender emailSender;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, IAppEmailSender emailSender)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            this.emailSender = emailSender;
        }

        public IActionResult Index()
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


        public IActionResult Contact()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Contact(ContactViewModel contact)
        {
            if (ModelState.IsValid)
            {
                await emailSender.SendEmailToStaffAsync(contact.Name, contact.Email, contact.Message);
                contact.StatusMessage = "Köszönjük az üzenetet, hamarosan jelentkezünk!";
                return View(contact);

            }

            return View();
        }

    }
}
