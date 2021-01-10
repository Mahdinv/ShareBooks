using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShareBooks.Core.Services.Interfaces;
using ShareBooks.DataLayer.Entities.Publishers;

namespace ShareBooks.Web.Pages.Admin.Publishers
{
    public class IndexModel : PageModel
    {
        private IBookService _bookService;

        public IndexModel(IBookService bookService)
        {
            _bookService = bookService;
        }

        public List<Publisher> Publishers { get; set; }
        public void OnGet()
        {
            Publishers = _bookService.GetAllPublisher();
        }
    }
}
