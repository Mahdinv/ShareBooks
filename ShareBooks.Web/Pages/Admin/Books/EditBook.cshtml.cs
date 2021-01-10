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
    [PermissionChecker(19)]
    public class EditBookModel : PageModel
    {
        private IBookService _bookService;

        public EditBookModel(IBookService bookService)
        {
            _bookService = bookService;
        }

        [BindProperty]
        public Book Book { get; set; }



        public void OnGet(int id)
        {
            Book = _bookService.GetBookById(id);

            var groups = _bookService.GetGroupsForManageBook();
            ViewData["Groups"] = new SelectList(groups, "Value", "Text", Book.GroupId);

            List<SelectListItem> subGroupsList = new List<SelectListItem>()
            {
                new SelectListItem(){Text = "انتخاب کنید",Value = ""}
            };

            subGroupsList.AddRange(_bookService.GetSubGroupsForManageBook(Book.GroupId));
            string selectedSubGroup = null;
            if (Book.SubGroup != null)
            {
                selectedSubGroup = Book.SubGroup.ToString();
            }
            ViewData["SubGroups"] = new SelectList(subGroupsList, "Value", "Text", selectedSubGroup);


            List<SelectListItem> secondSubGroupsList = new List<SelectListItem>()
            {
                new SelectListItem(){Text = "انتخاب کنید",Value = ""}
            };

            secondSubGroupsList.AddRange(_bookService.GetSecondSubGroupsForManageBook(Book.SubGroupId ?? 0));
            string selectedSecondSubGroup = null;
            if (Book.SecondSubGroup != null)
            {
                selectedSecondSubGroup = Book.SecondSubGroup.ToString();
            }

            ViewData["SecondSubGroups"] = new SelectList(secondSubGroupsList, "Value", "Text", selectedSecondSubGroup);

            var levels = _bookService.GetLevelsForManageBook();
            ViewData["Levels"] = new SelectList(levels, "Value", "Text", Book.LevelId);

            var publishers = _bookService.GetPublishersForManageBook();
            ViewData["Publishers"] = new SelectList(publishers, "Value", "Text", Book.PublisherId);
        }

        public IActionResult OnPost(IFormFile bookFile, IFormFile imgBookUp)
        {
            if (!ModelState.IsValid)
            {
                var groups = _bookService.GetGroupsForManageBook();
                ViewData["Groups"] = new SelectList(groups, "Value", "Text", Book.GroupId);

                var subGroups = _bookService.GetSubGroupsForManageBook(int.Parse(groups.First().Value));
                ViewData["SubGroups"] = new SelectList(subGroups, "Value", "Text", Book.SubGroupId ?? 0);

                var secondSubGroups = _bookService.GetSecondSubGroupsForManageBook(int.Parse(subGroups.First().Value));
                ViewData["SecondSubGroups"] = new SelectList(secondSubGroups, "Value", "Text", Book.SecondSubGroupId ?? 0);

                var levels = _bookService.GetLevelsForManageBook();
                ViewData["Levels"] = new SelectList(levels, "Value", "Text", Book.LevelId);

                var publishers = _bookService.GetPublishersForManageBook();
                ViewData["Publishers"] = new SelectList(publishers, "Value", "Text", Book.PublisherId);
                return Page();
            }


            _bookService.UpdateBook(Book, imgBookUp, bookFile);
            TempData["UpdateBook"] = true;
            return RedirectToPage("Index");
        }
    }
}
