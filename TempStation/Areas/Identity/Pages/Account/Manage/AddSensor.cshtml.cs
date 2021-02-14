using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using TempStation.Data.Models;
using TempStation.Services.Data.Contracts;

namespace TempStation.Areas.Identity.Pages.Account.Manage
{
    public class AddSensorModel : PageModel
    {
        private readonly UserManager<TempStationUser> _userManager;
        private readonly IUserSensorsService _userSensorsService;

        public AddSensorModel(
            UserManager<TempStationUser> userManager,
            IUserSensorsService userSensorsService)
        {
            _userManager = userManager;
            _userSensorsService = userSensorsService;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            public string Name { get; set; }

            [Required]
            [Display(Name = "MAC Address")]
            [StringLength(17, MinimumLength = 17, ErrorMessage = "The {0} field must be {1} characters.")]
            public string MacAddress { get; set; }
        }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _userSensorsService.Add(new UserSensor
            {
                MacAddress = Input.MacAddress.ToLowerInvariant(),
                Name = Input.Name,
                TempStationUserId = user.Id
            });

            StatusMessage = "The sensor has been added.";
            return RedirectToPage();
        }
    }
}
