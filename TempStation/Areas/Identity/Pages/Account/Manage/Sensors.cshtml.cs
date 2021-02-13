using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TempStation.Data.Models;

namespace TempStation.Areas.Identity.Pages.Account.Manage
{
    public class SensorsModel : PageModel
    {
        private readonly UserManager<TempStationUser> _userManager;
        private readonly SignInManager<TempStationUser> _signInManager;

        public SensorsModel(
            UserManager<TempStationUser> userManager,
            SignInManager<TempStationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            return Page();
        }
    }
}
