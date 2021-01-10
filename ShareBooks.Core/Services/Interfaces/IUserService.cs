using ShareBooks.Core.ViewModels;
using ShareBooks.DataLayer.Entities.Site;
using ShareBooks.DataLayer.Entities.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShareBooks.Core.Services.Interfaces
{
    public interface IUserService
    {
        bool IsExsitEmail(string email);
        bool IsExsitMobile(string mobile);

        int AddUser(User user);

        bool ActivateUser(string code);

        User LoginUser(LoginViewModel login);

        Setting GetSetting();

        User GetUserByEmail(string email);

        User GetUserByActiveCode(string activeCode);
        void UpdateUser(User user);


        UsersForAdminViewModel GetUsers(int pageId = 1, int take = 10, string filterByEmail = "", string filterByMobile = "");

        int AddUserFromAdmin(CreateUserViewModel model);
        User GetUserById(int userId);
        EditUserViewModel GetUserForShowInEditMode(int userId);
        void EditUserFromAdmin(EditUserViewModel editUser);

        UsersForAdminViewModel GetDeleteUsers(int pageId = 1, int take = 10, string filterByEmail = "", string filterByMobile = "");

        InformationUserViewModel GetUserInformation(int userId);
        InformationUserViewModel GetUserInformationByEmail(string email);

        void DeleteUser(int userId);

        EditProfileViewModel GetDataForEditProfileUser(string email);
        void EditProfile(string email, EditProfileViewModel profile);

        bool CompareOldPassword(string email, string oldPassword);
        void ChangeUserPassword(string email, string newPassword);

        SideBarAdminPanelViewModel GetSideBarAdminPanelData(string email); //GetDeleteUsers
    }
}
