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
    [PermissionChecker(16)]
    public class EditGroupModel : PageModel
    {
        private IBookService _bookService;

        public EditGroupModel(IBookService bookService)
        {
            _bookService = bookService;
        }

        [BindProperty]
        public BookGroup BookGroup { get; set; }
        public void OnGet(int id)
        {

            BookGroup = _bookService.GetGroupById(id);
        }


        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            _bookService.UpdateBookGroup(BookGroup);

            return RedirectToPage("Index");
        }
    }
}
