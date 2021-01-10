using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShareBooks.Core.Security;
using ShareBooks.Core.Services.Interfaces;
using ShareBooks.Core.ViewModels;

namespace ShareBooks.Web.Pages.Admin
{
    [UserRoleChecker]
    public class EditProfileModel : PageModel
    {
        private IUserService _userService;

        public EditProfileModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public EditProfileViewModel EditProfileViewModel { get; set; }


        public void OnGet()
        {
            EditProfileViewModel = _userService.GetDataForEditProfileUser(User.Identity.GetEmail());
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            _userService.EditProfile(User.Identity.GetEmail(), EditProfileViewModel);

            //--------LogOut User-------------//

            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Login?EditProfile=true");
        }

    }
}
