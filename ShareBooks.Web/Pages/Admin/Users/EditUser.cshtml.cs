using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShareBooks.Core.Security;
using ShareBooks.Core.Services.Interfaces;
using ShareBooks.Core.ViewModels;

namespace ShareBooks.Web.Pages.Admin.Users
{
    [PermissionChecker(3)]
    public class EditUserModel : PageModel
    {
        private IUserService _userService;
        private IPermissionService _permissionService;

        public EditUserModel(IUserService userService, IPermissionService permissionService)
        {
            _userService = userService;
            _permissionService = permissionService;
        }

        [BindProperty]
        public EditUserViewModel EditUserViewModel { get; set; }
        public IActionResult OnGet(int id)
        {
            if (_userService.GetUserById(id) != null)
            {
                ViewData["Roles"] = _permissionService.GetRoles();
                EditUserViewModel = _userService.GetUserForShowInEditMode(id);
                return Page();
            }
            else
            {
                return RedirectToPage("Index");
            }
        }


        public IActionResult OnPost(List<int> selectedRoles)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Roles"] = _permissionService.GetRoles();
                return Page();
            }

            _userService.EditUserFromAdmin(EditUserViewModel);
            _permissionService.EditRolesUser(EditUserViewModel.UserId, selectedRoles);

            return RedirectToPage("Index");
        }
    }
}
