using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShareBooks.Core.Security;
using ShareBooks.Core.Services.Interfaces;
using ShareBooks.DataLayer.Entities.Books;

namespace ShareBooks.Web.Pages.Admin.BookGroups
{
    [PermissionChecker(15)]
    public class CreateGroupModel : PageModel
    {
        private IBookService _bookService;

        public CreateGroupModel(IBookService bookService)
        {
            _bookService = bookService;
        }

        [BindProperty] //two way binding
        public BookGroup BookGroup { get; set; }
        public void OnGet(int? id)
        {
            BookGroup = new BookGroup()
            {
                ParentId = id
            };
        }

        public IActionResult OnPost()
        {

            if (!ModelState.IsValid)
                return Page();

            _bookService.AddBookGroup(BookGroup);
            return RedirectToPage("Index");

        }
    }
}
