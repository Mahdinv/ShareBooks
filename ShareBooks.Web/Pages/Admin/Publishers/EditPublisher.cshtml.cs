using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShareBooks.Core.Security;
using ShareBooks.Core.Services.Interfaces;
using ShareBooks.Core.ViewModels;
using ShareBooks.DataLayer.Entities.Publishers;

namespace ShareBooks.Web.Pages.Admin.Publishers
{
    [PermissionChecker(12)]
    public class EditPublisherModel : PageModel
    {
        private IBookService _bookService;

        public EditPublisherModel(IBookService bookService)
        {
            _bookService = bookService;
        }

        [BindProperty]
        public Publisher Publisher { get; set; }

        public void OnGet(int id)
        {
            Publisher = _bookService.GetPublisherById(id);
        }

        public IActionResult OnPost(IFormFile imgPublisherUp)
        {
            if (!ModelState.IsValid)
                return Page();

            _bookService.EditPublisherFromAdmin(Publisher, imgPublisherUp);
            TempData["UpdatePublisher"] = true;
            return RedirectToPage("Index");
        }
    }
}
