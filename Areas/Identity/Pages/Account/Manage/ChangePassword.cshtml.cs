using MarkMyDoctor.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
namespace MarkMyDoctor.Areas.Identity.Pages.Account.Manage
{
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<ChangePasswordModel> _logger;

        public ChangePasswordModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<ChangePasswordModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "A jelenlegi jelszó megadása kötelező!")]
            [DataType(DataType.Password)]
            [Display(Name = "Jelenlegi jelszó")]
            public string OldPassword { get; set; }

            [Required(ErrorMessage = "Az új jelszó megadása kötelező!")]
            [StringLength(100, ErrorMessage = "Az {0}-nak legalább {2} és maximum {1} karakter hosszúnak kell lennie.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Új jelszó")]
            public string NewPassword { get; set; }

            [Required(ErrorMessage = "Az új jelszó megerősítése kötelező!")]
            [DataType(DataType.Password)]
            [Display(Name = "Jelszó megerősítése")]
            [Compare("NewPassword", ErrorMessage = "Az új jelszónak, és a megerősítő jelszónak egyeznie kell!")]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Az '{_userManager.GetUserId(User)}'-val rendelkező felhazsnálót nem sikerült megtalálni.");
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                return RedirectToPage("./SetPassword");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Az '{_userManager.GetUserId(User)}'-val rendelkező felhazsnálót nem sikerült megtalálni.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            _logger.LogInformation("A felhasználó sikeresen megváltoztatta a jelszavát");
            StatusMessage = "A jelszavad megváltozott.";

            return RedirectToPage();
        }
    }
}
