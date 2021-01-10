using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShareBooks.Core.Security;
using ShareBooks.Core.Services.Interfaces;
using ShareBooks.Core.ViewModels;
using ShareBooks.DataLayer.Entities.Publishers;

namespace ShareBooks.Web.Pages.Admin.Publishers
{
    [PermissionChecker(11)]
    public class CreatePublisherModel : PageModel
    {
        private IBookService _bookService;

        public CreatePublisherModel(IBookService bookService)
        {
            _bookService = bookService;
        }

        [BindProperty]
        public CreatePublisherViewModel CreatePublisherViewModel { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            _bookService.AddPublisher(CreatePublisherViewModel);

            return RedirectToPage("Index");
        }
    }
}
