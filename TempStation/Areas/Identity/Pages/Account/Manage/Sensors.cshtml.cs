using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TempStation.Data.Models;
using TempStation.Services.Data.Contracts;

namespace TempStation.Areas.Identity.Pages.Account.Manage
{
    public class SensorsModel : PageModel
    {
        private readonly UserManager<TempStationUser> _userManager;
        private readonly IUserSensorsService _userSensorsService;

        public SensorsModel(
            UserManager<TempStationUser> userManager,
            IUserSensorsService userSensorsService)
        {
            _userManager = userManager;
            _userSensorsService = userSensorsService;
        }

        [BindProperty]
        public List<UserSensor> SensorListData { get; set; }

        private void Load(TempStationUser user)
        {
            SensorListData = _userSensorsService.AllByUserId(user.Id).ToList();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Load(user);

            return Page();
        }

        public async Task<IActionResult> OnPostDelete(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var userSensor = _userSensorsService.GetBySensorId(id);

            if (userSensor == null)
            {
                return NotFound($"Unable to find user sensor with ID '{id}'.");
            }

            if (userSensor.TempStationUserId != user.Id)
            {
                return NotFound($"This sensor does not belong to you.");
            }

            await _userSensorsService.DeleteById(id);

            return RedirectToPage("Sensors");
        }
    }
}
