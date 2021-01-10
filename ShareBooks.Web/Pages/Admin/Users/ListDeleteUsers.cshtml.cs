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
    [PermissionChecker(5)]
    public class ListDeleteUsersModel : PageModel
    {
        private IUserService _userService;

        public ListDeleteUsersModel(IUserService userService)
        {
            _userService = userService;
        }

        public UsersForAdminViewModel UsersForAdminViewModel { get; set; }
        public void OnGet(int pageId = 1, int take = 5, string filterByEmail = "", string filterByMobile = "")
        {
            if (pageId > 1)
            {
                ViewData["Take"] = (pageId - 1) * take + 1;
            }
            else
            {
                ViewData["Take"] = take;
            }

            ViewData["FilterEmail"] = filterByEmail;
            ViewData["FilterMobile"] = filterByMobile;
            ViewData["PageID"] = (pageId - 1) * take + 1;
            UsersForAdminViewModel = _userService.GetDeleteUsers(pageId, take, filterByEmail, filterByMobile);

        }
    }
}
