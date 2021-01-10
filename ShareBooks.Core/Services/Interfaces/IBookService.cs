using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShareBooks.Core.ViewModels;
using ShareBooks.DataLayer.Entities.Books;
using ShareBooks.DataLayer.Entities.Publishers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShareBooks.Core.Services.Interfaces
{
    public interface IBookService
    {
        List<BookGroup> GetAllGroups();
        void AddBookGroup(BookGroup @group);
        BookGroup GetGroupById(int groupId);
        void UpdateBookGroup(BookGroup @group);

        List<Publisher> GetAllPublisher();

        List<ShowBookForAdminViewModel> GetBooksForAdmin();

        List<SelectListItem> GetGroupsForManageBook();
        List<SelectListItem> GetSubGroupsForManageBook(int groupId);
        List<SelectListItem> GetSecondSubGroupsForManageBook(int subGroupId);

        List<SelectListItem> GetLevelsForManageBook();

        List<SelectListItem> GetPublishersForManageBook();

        int AddBook(Book book, IFormFile imgBookUp, IFormFile bookFile);

        Book GetBookById(int bookId);

        void UpdateBook(Book book, IFormFile imgBook, IFormFile bookFile);

        int AddPublisher(CreatePublisherViewModel createPublisherViewModel);
        int AddPublisherFromAdmin(Publisher publisher);

        Publisher GetPublisherById(int publisherId);

        EditPublisherViewModel GetPublisherForShowInEditMode(int publisherId);

        void EditPublisherFromAdmin(Publisher publisher, IFormFile imgPublisher);
    }
}
