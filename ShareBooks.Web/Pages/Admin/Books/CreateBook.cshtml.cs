using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShareBooks.Core.Security;
using ShareBooks.Core.Services.Interfaces;
using ShareBooks.DataLayer.Entities.Books;

namespace ShareBooks.Web.Pages.Admin.Books
{
    [PermissionChecker(18)]
    public class CreateBookModel : PageModel
    {
        private IBookService _bookService;

        public CreateBookModel(IBookService bookService)
        {
            _bookService = bookService;
        }


        [BindProperty]
        public Book Book { get; set; }
        public void OnGet()
        {
            var groups = _bookService.GetGroupsForManageBook();
            ViewData["Groups"] = new SelectList(groups, "Value", "Text");

            var subGroups = _bookService.GetSubGroupsForManageBook(int.Parse(groups.First().Value));
            ViewData["SubGroups"] = new SelectList(subGroups, "Value", "Text");

            var secondSubGroups = _bookService.GetSecondSubGroupsForManageBook(int.Parse(subGroups.First().Value));
            ViewData["SecondSubGroups"] = new SelectList(secondSubGroups, "Value", "Text");

            var levels = _bookService.GetLevelsForManageBook();
            ViewData["Levels"] = new SelectList(levels, "Value", "Text");

            var publishers = _bookService.GetPublishersForManageBook();
            ViewData["Publishers"] = new SelectList(publishers, "Value", "Text");

        }

        public IActionResult OnPost(IFormFile bookFile, IFormFile imgBookUp)
        {

            if (!ModelState.IsValid)
            {
                var groups = _bookService.GetGroupsForManageBook();
                ViewData["Groups"] = new SelectList(groups, "Value", "Text");

                var subGroups = _bookService.GetSubGroupsForManageBook(int.Parse(groups.First().Value));
                ViewData["SubGroups"] = new SelectList(subGroups, "Value", "Text");

                var secondSubGroups = _bookService.GetSecondSubGroupsForManageBook(int.Parse(subGroups.First().Value));
                ViewData["SecondSubGroups"] = new SelectList(secondSubGroups, "Value", "Text");

                var levels = _bookService.GetLevelsForManageBook();
                ViewData["Levels"] = new SelectList(levels, "Value", "Text");

                var publishers = _bookService.GetPublishersForManageBook();
                ViewData["Publishers"] = new SelectList(publishers, "Value", "Text");
                return Page();
            }

            _bookService.AddBook(Book, imgBookUp, bookFile);
            TempData["InsertBook"] = true;
            return RedirectToPage("Index");
        }
    }
}
