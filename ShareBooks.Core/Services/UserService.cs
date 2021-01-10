using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using ShareBooks.Core.Convertors;
using ShareBooks.Core.Generators;
using ShareBooks.Core.Services.Interfaces;
using ShareBooks.Core.ViewModels;
using ShareBooks.DataLayer.Context;
using ShareBooks.DataLayer.Entities.Site;
using ShareBooks.DataLayer.Entities.Users;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ShareBooks.Core.Services
{
    public class UserService : IUserService
    {
        private ShareBooksContext _context;
        private IHostingEnvironment _environment;
        private IPermissionService _permissionService;

        public UserService(ShareBooksContext context, IHostingEnvironment environment, IPermissionService permissionService)
        {
            _context = context;
            _environment = environment;
            _permissionService = permissionService;
        }

        public bool ActivateUser(string code)
        {
            User user = _context.Users.FirstOrDefault(u => u.IsActive == false && u.ActiveCode == code);

            if (user != null)
            {
                user.IsActive = true;
                user.ActiveCode = CodeGenerator.ActiveCode();

                _context.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }

        public int AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user.UserId;
        }

        public int AddUserFromAdmin(CreateUserViewModel model)
        {
            User user = new User();
            user.Email = model.Email;
            user.ActiveCode = CodeGenerator.ActiveCode();
            user.CreateDate = DateTime.Now;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.IsActive = model.IsActive;
            user.Mobile = model.Mobile;
            user.Password = HashGenerator.MD5Encoding(model.Password);

            #region Save Avatar

            if (model.UserAvatar != null)
            {
                string imagePath = "";

                //-------Upload New User Image --------//
                user.UserAvatar = CodeGenerator.ActiveCode() + Path.GetExtension(model.UserAvatar.FileName);
                imagePath = Path.Combine(_environment.WebRootPath, "UserAvatar", user.UserAvatar);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    model.UserAvatar.CopyTo(stream);
                }
            }

            #endregion

            return AddUser(user);
        }

        public void ChangeUserPassword(string email, string newPassword)
        {
            var user = GetUserByEmail(email);
            user.Password = HashGenerator.MD5Encoding(newPassword);
            UpdateUser(user);
        }

        public bool CompareOldPassword(string email, string oldPassword)
        {
            string hashOldPassword = HashGenerator.MD5Encoding(oldPassword);
            return _context.Users.Any(u => u.Email == email && u.Password == hashOldPassword);
        }

        public void DeleteUser(int userId)
        {
            var user = GetUserById(userId);
            user.IsDelete = true;
            UpdateUser(user);
            _permissionService.RemoveRolesUser(user.UserId);
        }

        public void EditProfile(string email, EditProfileViewModel profile)
        {
            if (profile.UserAvatar != null)
            {
                string imagePath = "";
                if (profile.AvatarName != "default.png")
                {
                    //------Delete User Image --------//
                    imagePath = Path.Combine(_environment.WebRootPath, "UserAvatar", profile.AvatarName);
                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }

                }
                //-------Upload New User Image --------//
                profile.AvatarName = CodeGenerator.ActiveCode() + Path.GetExtension(profile.UserAvatar.FileName);
                imagePath = Path.Combine(_environment.WebRootPath, "UserAvatar", profile.AvatarName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    profile.UserAvatar.CopyTo(stream);
                }
            }

            var user = GetUserByEmail(email);
            user.FirstName = profile.FirstName;
            user.LastName = profile.LastName;
            user.Mobile = profile.Mobile;
            user.UserAvatar = profile.AvatarName;

            UpdateUser(user);
        }

        public void EditUserFromAdmin(EditUserViewModel editUser)
        {
            var user = GetUserById(editUser.UserId);
            user.FirstName = editUser.FirstName;
            user.LastName = editUser.LastName;
            user.Mobile = editUser.Mobile;
            user.IsActive = editUser.IsActive;

            if (!string.IsNullOrEmpty(editUser.Password))
            {
                user.Password = HashGenerator.MD5Encoding(editUser.Password);
            }

            if (editUser.UserAvatar != null)
            {
                string imagePath = "";
                if (editUser.AvatarName != "default.png")
                {
                    //------Delete User Image --------//
                    imagePath = Path.Combine(_environment.WebRootPath, "UserAvatar", editUser.AvatarName);
                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }

                }
                //-------Upload New User Image --------//
                editUser.AvatarName = CodeGenerator.ActiveCode() + Path.GetExtension(editUser.UserAvatar.FileName);
                imagePath = Path.Combine(_environment.WebRootPath, "UserAvatar", editUser.AvatarName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    editUser.UserAvatar.CopyTo(stream);
                }
            }

            user.UserAvatar = editUser.AvatarName;
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public EditProfileViewModel GetDataForEditProfileUser(string email)
        {
            return _context.Users.Where(u => u.Email == email).Select(u => new EditProfileViewModel()
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                Mobile = u.Mobile,
                AvatarName = u.UserAvatar,
                Email = u.Email
            }).Single();
        }

        public UsersForAdminViewModel GetDeleteUsers(int pageId = 1, int take = 10, string filterByEmail = "", string filterByMobile = "")
        {
            IQueryable<User> result = _context.Users.IgnoreQueryFilters().Where(u => u.IsDelete);  //lazyLoad;

            if (!string.IsNullOrEmpty(filterByEmail))
            {
                result = result.Where(u => u.Email.Contains(filterByEmail));
            }

            if (!string.IsNullOrEmpty(filterByMobile))
            {
                result = result.Where(u => u.Mobile == filterByMobile);
            }

            int takeData = take;
            int skip = (pageId - 1) * takeData;

            UsersForAdminViewModel list = new UsersForAdminViewModel();
            list.CurrentPage = pageId;
            list.PageCount = (int)Math.Ceiling(result.Count() / (double)takeData);
            list.Users = result.OrderByDescending(u => u.CreateDate).Skip(skip).Take(takeData).ToList();
            list.UserCounts = _context.Users.Count();

            return list;
        }

        public Setting GetSetting()
        {
            return _context.Settings.FirstOrDefault();
        }

        public User GetUserByActiveCode(string activeCode)
        {
            return _context.Users.SingleOrDefault(u => u.ActiveCode == activeCode);
        }

        public User GetUserByEmail(string email)
        {
            return _context.Users.SingleOrDefault(u => u.Email == email);
        }

        public User GetUserById(int userId)
        {
            return _context.Users.Find(userId);
        }

        public EditUserViewModel GetUserForShowInEditMode(int userId)
        {
            return _context.Users.Where(u => u.UserId == userId).Select(u => new EditUserViewModel()
            {
                Email = u.Email,
                AvatarName = u.UserAvatar,
                FirstName = u.FirstName,
                IsActive = u.IsActive,
                LastName = u.LastName,
                Mobile = u.Mobile,
                UserId = u.UserId,
                UserRoles = u.UserRoles.Select(r => r.RoleId).ToList(),

            }).Single();
        }

        public InformationUserViewModel GetUserInformation(int userId)
        {
            var user = GetUserById(userId);
            InformationUserViewModel information = new InformationUserViewModel();
            information.Email = user.Email;
            information.FirstName = user.FirstName;
            information.LastName = user.LastName;
            information.Mobile = user.Mobile;
            information.RegisterDate = user.CreateDate;
            information.UserAvatar = user.UserAvatar;
            return information;
        }

        public InformationUserViewModel GetUserInformationByEmail(string email)
        {
            var user = GetUserByEmail(email);
            InformationUserViewModel information = new InformationUserViewModel();
            information.Email = user.Email;
            information.FirstName = user.FirstName;
            information.LastName = user.LastName;
            information.Mobile = user.Mobile;
            information.RegisterDate = user.CreateDate;
            information.UserAvatar = user.UserAvatar;
            return information;
        }

        public UsersForAdminViewModel GetUsers(int pageId = 1, int take = 10, string filterByEmail = "", string filterByMobile = "")
        {
            IQueryable<User> result = _context.Users;  //lazyLoad;

            if (!string.IsNullOrEmpty(filterByEmail))
            {
                result = result.Where(u => u.Email.Contains(filterByEmail));
            }

            if (!string.IsNullOrEmpty(filterByMobile))
            {
                result = result.Where(u => u.Mobile == filterByMobile);
            }

            int takeData = take;
            int skip = (pageId - 1) * takeData;

            UsersForAdminViewModel list = new UsersForAdminViewModel();
            list.CurrentPage = pageId;
            list.PageCount = (int)Math.Ceiling(result.Count() / (double)takeData);
            list.Users = result.OrderByDescending(u => u.CreateDate).Skip(skip).Take(takeData).ToList();
            list.UserCounts = _context.Users.Count();

            return list;

        }

        public bool IsExsitEmail(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public bool IsExsitMobile(string mobile)
        {
            return _context.Users.Any(u => u.Mobile == mobile);
        }

        public User LoginUser(LoginViewModel login)
        {
            string password = HashGenerator.MD5Encoding(login.Password);
            string email = FixedText.FixedEmail(login.Email);
            return _context.Users.SingleOrDefault(u => u.Email == email && u.Password == password);
        }

        public void UpdateUser(User user)
        {
            _context.Update(user);
            _context.SaveChanges();
        }
    }
}
