using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShareBooks.Core.Convertors;
using ShareBooks.Core.Services.Interfaces;
using ShareBooks.Core.ViewModels;

namespace ShareBooks.Web.Pages.Admin.Users
{
    public class CreateUserModel : PageModel
    {
        private IUserService _userService;
        private IPermissionService _permissionService;

        public CreateUserModel(IUserService userService, IPermissionService permissionService)
        {
            _userService = userService;
            _permissionService = permissionService;
        }

        [BindProperty]
        public CreateUserViewModel CreateUserViewModel { get; set; }
        public void OnGet()
        {

            ViewData["Roles"] = _permissionService.GetRoles();
        }

        public IActionResult OnPost(List<int> selectedRoles)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Roles"] = _permissionService.GetRoles();
                return Page();
            }

            if (_userService.IsExsitEmail(FixedText.FixedEmail(CreateUserViewModel.Email)))
            {
                ModelState.AddModelError("CreateUserViewModel.Email", "ایمیل وارد شده تکراری می باشد");
                ViewData["Roles"] = _permissionService.GetRoles();
                return Page();
            }

            if (_userService.IsExsitMobile(CreateUserViewModel.Mobile))
            {
                ModelState.AddModelError("CreateUserViewModel.Mobile", "شماره موبایل وارد شده تکراری می باشد");
                ViewData["Roles"] = _permissionService.GetRoles();
                return Page();
            }

            int userId = _userService.AddUserFromAdmin(CreateUserViewModel);
            _permissionService.AddRolesToUser(selectedRoles, userId);

            return RedirectToPage("Index");
        }
    }
}
