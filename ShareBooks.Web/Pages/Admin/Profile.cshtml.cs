using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShareBooks.Core.Security;
using ShareBooks.Core.Services.Interfaces;
using ShareBooks.Core.ViewModels;

namespace ShareBooks.Web.Pages.Admin
{
    [UserRoleChecker]
    public class ProfileModel : PageModel
    {
        private IUserService _userService;

        public ProfileModel(IUserService userService)
        {
            _userService = userService;
        }

        public InformationUserViewModel InformationUserViewModel { get; set; }

        public void OnGet()
        {
            InformationUserViewModel = _userService.GetUserInformationByEmail(User.Identity.GetEmail());
        }
    }
}
