using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.DataLayer.Entities.Permissions;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.Core.Servises.Interfaces
{
    public interface IPermissionService
    {
        #region Roles

        List<Role> GetRoles();
        int AddRole(Role role);
        Role GetRoleById(int roleId);
        void UpdateRole(Role role);
        void DeleteRole(Role role);
        bool IsExistRoel(string roleTitle);
        void AddRolesToUser(List<int> roleIds, int userId);
        void EditRolesUser(List<int> roleIds, int userId);


        #endregion


        #region Permission

        List<Permission> GetAllPermissions();
        void AddPermissionsToRole(int roleId, List<int> selectedPermission);
        List<int> PermissionRole(int roleId);
        void UpdatePermissionsRole(int roleId, List<int> permission);
        bool CheckPermission(int permissionId, string userName);

        #endregion
    }
}
