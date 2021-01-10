using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShareBooks.Core.Security;

namespace ShareBooks.Web.Pages.Admin
{
    [UserRoleChecker]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
