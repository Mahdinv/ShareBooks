using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ShareBooks.Core.Convertors;
using ShareBooks.Core.Generators;
using ShareBooks.Core.Senders;
using ShareBooks.Core.Services.Interfaces;
using ShareBooks.Core.ViewModels;
using ShareBooks.DataLayer.Entities.Users;

namespace ShareBooks.Web.Controllers
{
    public class AccountController : Controller
    {
        private IUserService _userService;
        private IViewRenderService _viewRender;

        public AccountController(IUserService userService, IViewRenderService viewRender)
        {
            _userService = userService;
            _viewRender = viewRender;
        }


        #region Register

        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [Route("Register")]
        [HttpPost]
        public IActionResult Register(RegisterViewModel register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }

            if (_userService.IsExsitMobile(register.Mobile))
            {
                ModelState.AddModelError("Mobile", "شماره موبایل وارد شده تکراری است");
                return View(register);
            }
            if (_userService.IsExsitEmail(FixedText.FixedEmail(register.Email)))
            {
                ModelState.AddModelError("Email", "ایمیل وارد شده تکراری است");
                return View(register);
            }

            DataLayer.Entities.Users.User user = new User()
            {
                Mobile = register.Mobile,
                Email = FixedText.FixedEmail(register.Email),
                LastName = register.LastName,
                Password = HashGenerator.MD5Encoding(register.Password),
                FirstName = register.FirstName,
                ActiveCode = CodeGenerator.ActiveCode(),
                CreateDate = DateTime.Now,
                IsActive = false,
                UserAvatar = "default.png",
            };

            _userService.AddUser(user);

            //Send SMS (bade sabtnam kardan)
            try
            {
                MessageSender sender = new MessageSender();
                sender.SMS(register.Mobile, "به فروشگاه اینترنتی خوش آمدید" + Environment.NewLine + "کد فعالسازی :" + user.ActiveCode); /*Environment.NewLine => mire khat bad*/
            }
            catch
            {

            }

            return View("SuccessRegister", model: user);
        }

        #endregion

        #region Activate

        [Route("Activate")]
        public IActionResult Activate()
        {
            return View();
        }

        [Route("Activate")]
        [HttpPost]
        public IActionResult Activate(ActivateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {

                if (_userService.ActivateUser(viewModel.ActiveCode)) /*migim age activateCode ma dorost bood bere be safe login vagarane error bede*/
                {
                    //Go To Login
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    ModelState.AddModelError("ActiveCode", "کد فعالسازی شما معتبر نیست");
                }

            }

            return View(viewModel);
        }

        #endregion

        #region Login

        [Route("Login")]
        public IActionResult Login(bool EditProfile = false, bool permission = true)
        {
            ViewBag.EditProfile = EditProfile;
            ViewBag.Permission = permission;
            return View();
        }

        [Route("Login")]
        [HttpPost]
        public IActionResult Login(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var user = _userService.LoginUser(login);
            if (user != null)
            {
                if (user.IsActive)
                {
                    var claim = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                        new Claim(ClaimTypes.Name,user.FirstName + " " + user.LastName),
                        new Claim(ClaimTypes.Email,user.Email),

                    };
                    var identity = new ClaimsIdentity(claim, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    var properties = new AuthenticationProperties
                    {
                        IsPersistent = login.RememberMe
                    };

                    HttpContext.SignInAsync(principal, properties);
                    ViewBag.IsSuccess = true;
                    return View();
                }
                else
                {
                    ModelState.AddModelError("Email", "حساب کاربری شما غیر فعال می باشد");
                }
            }
            ModelState.AddModelError("Email", "کاربری با این مشخصات یافت نشد");
            return View();
        }

        #region LogOut
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Login");
        }


        #endregion

        #endregion

        #region ForgotPassword

        [Route("ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [Route("ForgotPassword")]
        public IActionResult ForgotPassword(ForgotPasswordViewModel forgot)
        {
            if (!ModelState.IsValid)
                return View(forgot);
            string fixedEmail = FixedText.FixedEmail(forgot.Email);
            DataLayer.Entities.Users.User user = _userService.GetUserByEmail(fixedEmail);
            if (user == null)
            {
                ModelState.AddModelError("Email", "کاربری یافت نشد");
                return View(forgot);
            }

            string bodyEmail = _viewRender.RenderToStringAsync("_ForgotPassword", user);
            SendEmail.Send(user.Email, "بازیابی کلمه عبور", bodyEmail);
            ViewBag.IsSuccess = true;
            return View();
        }
        #endregion

        #region Reset Password


        public IActionResult ResetPassword(string id)
        {
            return View(new ResetPasswordViewModel()
            {
                ActiveCode = id,
            });
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel reset)
        {
            if (!ModelState.IsValid)
                return View(reset);

            DataLayer.Entities.Users.User user = _userService.GetUserByActiveCode(reset.ActiveCode);
            if (user == null)
                return NotFound();

            string hashNewPassword = HashGenerator.MD5Encoding(reset.Password);
            user.Password = hashNewPassword;
            _userService.UpdateUser(user);
            return Redirect("/Login");


        }

        #endregion
    }
}
