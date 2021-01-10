using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShareBooks.Core.Security;
using ShareBooks.Core.Services.Interfaces;
using ShareBooks.Core.ViewModels;

namespace ShareBooks.Web.Pages.Admin.Books
{
    [PermissionChecker(17)]
    public class IndexModel : PageModel
    {
        private IBookService _bookService;

        public IndexModel(IBookService bookService)
        {
            _bookService = bookService;
        }

        public List<ShowBookForAdminViewModel> BookList { get; set; }
        public void OnGet()
        {
            BookList = _bookService.GetBooksForAdmin();
        }
    }
}
