using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShareBooks.Core.Services.Interfaces;

namespace ShareBooks.Web.Controllers
{
    public class HomeController : Controller
    {
        private IUserService _userService;
        private IHostingEnvironment environment;
        private IBookService _bookService;

        public HomeController(IUserService userService, IHostingEnvironment environment, IBookService bookService)
        {
            _userService = userService;
            this.environment = environment;
            _bookService = bookService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("file-upload")]
        public IActionResult UploadImage(IFormFile upload, string CKEditorFuncNum, string CKEditor, string langCode)
        {
            if (upload.Length <= 0) return null;

            var fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName).ToLower();

            var path = Path.Combine(
                environment.WebRootPath, "upload",
                fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                upload.CopyTo(stream);

            }



            var url = $"{"/upload/"}{fileName}";


            return Json(new { uploaded = true, url });
        }

        public IActionResult GetSubGroups(int id)
        {
            List<SelectListItem> list = new List<SelectListItem>()
            {
                new SelectListItem(){Text = "انتخاب کنید" , Value = ""}
            };
            list.AddRange(_bookService.GetSubGroupsForManageBook(id));
            return Json(new SelectList(list, "Value", "Text"));
        }

        public IActionResult GetSecondSubGroups(int id)
        {
            List<SelectListItem> list = new List<SelectListItem>()
            {
                new SelectListItem(){Text = "انتخاب کنید" , Value = ""}
            };
            list.AddRange(_bookService.GetSecondSubGroupsForManageBook(id));
            return Json(new SelectList(list, "Value", "Text"));
        }
    }
}
