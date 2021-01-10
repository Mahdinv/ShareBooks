using ShareBooks.DataLayer.Entities.Permissions;
using ShareBooks.DataLayer.Entities.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShareBooks.Core.Services.Interfaces
{
    public interface IPermissionService
    {
        List<Role> GetRoles();
        void AddRolesToUser(List<int> roleIds, int userId);
        void EditRolesUser(int userId, List<int> rolesId);

        List<string> GetUserRoles(int userId);

        void RemoveRolesUser(int userId);

        List<Permission> GetAllPermissions();
        int AddRole(Role role);
        void AddPermissionsToRole(int roleId, List<int> permissions);

        Role GetRoleById(int roleId);
        List<int> PermissionsRole(int roleId);
        void UpdateRole(Role role);
        void UpdatePermissionsRole(int roleId, List<int> permissions);
        void DeleteRole(Role role);

        bool CheckUserIsRole(string email);
        bool CheckPermission(int permissionId, string email);
    }
}
