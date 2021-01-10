using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShareBooks.Core.Convertors;
using ShareBooks.Core.Generators;
using ShareBooks.Core.Security;
using ShareBooks.Core.Services.Interfaces;
using ShareBooks.Core.ViewModels;
using ShareBooks.DataLayer.Context;
using ShareBooks.DataLayer.Entities.Books;
using ShareBooks.DataLayer.Entities.Publishers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ShareBooks.Core.Services
{
    public class BookService : IBookService
    {
        private ShareBooksContext _context;
        private IHostingEnvironment _environment;

        public BookService(ShareBooksContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public int AddBook(Book book, IFormFile imgBookUp, IFormFile bookFile)
        {
            book.CreateDate = DateTime.Now;
            book.BookImageName = "no-photo.png";


            if (imgBookUp != null && imgBookUp.IsImage())
            {
                string imagePath = "";

                book.BookImageName = CodeGenerator.ActiveCode() + Path.GetExtension(imgBookUp.FileName);
                imagePath = Path.Combine(_environment.WebRootPath, "book/images", book.BookImageName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    imgBookUp.CopyTo(stream);
                }

                #region Save Thumbnail Course Image

                string thumbPath = Path.Combine(_environment.WebRootPath, "book/thumbnail", book.BookImageName);
                ImageConvertors imgResizer = new ImageConvertors();
                imgResizer.Image_resize(imagePath, thumbPath, 200);

                #endregion

            }

            if (bookFile != null)
            {
                string filePath = "";

                book.BookFileName = CodeGenerator.ActiveCode() + Path.GetExtension(bookFile.FileName);
                filePath = Path.Combine(_environment.WebRootPath, "book/download", book.BookFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    bookFile.CopyTo(stream);
                }
            }

            _context.Books.Add(book);
            _context.SaveChanges();
            return book.BookId;
        }

        public void AddBookGroup(BookGroup group)
        {
            _context.BookGroups.Add(group);
            _context.SaveChanges();
        }

        public int AddPublisher(CreatePublisherViewModel createPublisherViewModel)
        {
            Publisher publisher = new Publisher();
            publisher.PublisherTitle = createPublisherViewModel.PublisherTitle;

            #region Save Avatar

            if (createPublisherViewModel.PublisherImageName != null)
            {
                string imagePath = "";

                //-------Upload New User Image --------//
                publisher.PublisherImageName = CodeGenerator.ActiveCode() + Path.GetExtension(createPublisherViewModel.PublisherImageName.FileName);
                imagePath = Path.Combine(_environment.WebRootPath, "images/publisher", publisher.PublisherImageName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    createPublisherViewModel.PublisherImageName.CopyTo(stream);
                }
            }

            #endregion

            return AddPublisherFromAdmin(publisher);
        }

        public int AddPublisherFromAdmin(Publisher publisher)
        {
            _context.Publishers.Add(publisher);
            _context.SaveChanges();
            return publisher.PublisherId;
        }

        public void EditPublisherFromAdmin(Publisher publisher, IFormFile imgPublisher)
        {
            var currentTitle = publisher.PublisherTitle;

            if (imgPublisher != null && imgPublisher.IsImage())
            {
                string imagePath = "";

                #region Remove Old Course Images

                if (publisher.PublisherImageName != "no-photo.png")
                {
                    string deletePath = Path.Combine(_environment.WebRootPath, "images/publisher", publisher.PublisherImageName);
                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }
                    //------Delete Thumb Course Image ------//
                    string deleteThumbPath = Path.Combine(_environment.WebRootPath, "images/publisher", publisher.PublisherImageName);
                    if (File.Exists(deleteThumbPath))
                    {
                        File.Delete(deleteThumbPath);
                    }
                }

                #endregion

                #region Add New Course Image

                publisher.PublisherImageName = CodeGenerator.ActiveCode() + Path.GetExtension(imgPublisher.FileName);
                imagePath = Path.Combine(_environment.WebRootPath, "images/publisher", publisher.PublisherImageName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    imgPublisher.CopyTo(stream);
                }



                string thumbPath = Path.Combine(_environment.WebRootPath, "images/publisher", publisher.PublisherImageName);
                ImageConvertors imgResizer = new ImageConvertors();
                imgResizer.Image_resize(imagePath, thumbPath, 200);


                #endregion

            }

            publisher.PublisherTitle = currentTitle;
            _context.Publishers.Update(publisher);
            _context.SaveChanges();

        }

        public List<BookGroup> GetAllGroups()
        {
            return _context.BookGroups.Include(g => g.BookGroups).ToList();
        }

        public List<Publisher> GetAllPublisher()
        {
            return _context.Publishers.Include(c=> c.Books).ToList();
        }

        public Book GetBookById(int bookId)
        {
            return _context.Books.Find(bookId);
        }

        public List<ShowBookForAdminViewModel> GetBooksForAdmin()
        {
            return _context.Books.Select(c => new ShowBookForAdminViewModel()
            {
                BookImageName = c.BookImageName,
                CreateDate = c.CreateDate,
                Publisher = c.Publisher.PublisherTitle,
                BookLevel = c.BookLevel.LevelTitle,
                BookFaTitle = c.BookFaTitle,
                BookId = c.BookId,
                IsSpecial = c.IsSpecial,
                BookLatinTitle = c.BookLatinTitle,
                GroupId = c.BookGroup.GroupTitle,
                SubGroupId = c.SubGroup.GroupTitle,
                SecondSubGroupId = c.SecondSubGroup.GroupTitle
            }).ToList();
        }

        public BookGroup GetGroupById(int groupId)
        {
            return _context.BookGroups.Find(groupId);
        }

        public List<SelectListItem> GetGroupsForManageBook()
        {
            return _context.BookGroups.Where(g => g.ParentId == null)
                .Select(g => new SelectListItem()
                {
                    Text = g.GroupTitle,
                    Value = g.GroupId.ToString()
                }).ToList();
        }

        public List<SelectListItem> GetLevelsForManageBook()
        {
            return _context.BookLevels.Select(l => new SelectListItem()
            {
                Value = l.LevelId.ToString(),
                Text = l.LevelTitle
            }).ToList();
        }

        public Publisher GetPublisherById(int publisherId)
        {
            return _context.Publishers.Find(publisherId);
        }

        public EditPublisherViewModel GetPublisherForShowInEditMode(int publisherId)
        {
            return _context.Publishers.Where(p => p.PublisherId == publisherId).Select(p => new EditPublisherViewModel()
            {
                PublisherTitle = p.PublisherTitle,
                AvatarName = p.PublisherImageName

            }).Single();
        }

        public List<SelectListItem> GetPublishersForManageBook()
        {
            return _context.Publishers.Select(c => new SelectListItem()
            {
                Value = c.PublisherId.ToString(),
                Text = c.PublisherTitle
            }).ToList();
        }

        public List<SelectListItem> GetSecondSubGroupsForManageBook(int subGroupId)
        {
            return _context.BookGroups.Where(g => g.ParentId == subGroupId)
                .Select(g => new SelectListItem()
                {
                    Text = g.GroupTitle,
                    Value = g.GroupId.ToString()
                }).ToList();
        }

        public List<SelectListItem> GetSubGroupsForManageBook(int groupId)
        {
            return _context.BookGroups.Where(g => g.ParentId == groupId)
                .Select(g => new SelectListItem()
                {
                    Text = g.GroupTitle,
                    Value = g.GroupId.ToString()
                }).ToList();
        }

        public void UpdateBook(Book book, IFormFile imgBook, IFormFile bookFile)
        {
            var currentDate = book.CreateDate;

            if (imgBook != null && imgBook.IsImage())
            {
                string imagePath = "";

                #region Remove Old Course Images

                if (book.BookImageName != "no-photo.png")
                {
                    string deletePath = Path.Combine(_environment.WebRootPath, "book/images", book.BookImageName);
                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }
                    //------Delete Thumb Course Image ------//
                    string deleteThumbPath = Path.Combine(_environment.WebRootPath, "book/thumbnail", book.BookImageName);
                    if (File.Exists(deleteThumbPath))
                    {
                        File.Delete(deleteThumbPath);
                    }
                }

                #endregion

                #region Add New Course Image

                book.BookImageName = CodeGenerator.ActiveCode() + Path.GetExtension(imgBook.FileName);
                imagePath = Path.Combine(_environment.WebRootPath, "book/images", book.BookImageName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    imgBook.CopyTo(stream);
                }



                string thumbPath = Path.Combine(_environment.WebRootPath, "book/thumbnail", book.BookImageName);
                ImageConvertors imgResizer = new ImageConvertors();
                imgResizer.Image_resize(imagePath, thumbPath, 200);


                #endregion

            }


            if (bookFile != null)
            {
                string filePath = "";

                string deleteFilePath = Path.Combine(_environment.WebRootPath, "book/download", book.BookFileName);
                if (File.Exists(deleteFilePath))
                {
                    File.Delete(deleteFilePath);
                }

                book.BookFileName = CodeGenerator.ActiveCode() + Path.GetExtension(bookFile.FileName);
                filePath = Path.Combine(_environment.WebRootPath, "book/download", book.BookFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    bookFile.CopyTo(stream);
                }
            }

            book.CreateDate = currentDate;
            _context.Books.Update(book);
            _context.SaveChanges();
        }

        public void UpdateBookGroup(BookGroup group)
        {
            _context.BookGroups.Update(group);
            _context.SaveChanges();
        }
    }
}
