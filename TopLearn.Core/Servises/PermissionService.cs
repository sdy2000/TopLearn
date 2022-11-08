using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.Core.Servises.Interfaces;
using TopLearn.DataLayer.Context;
using TopLearn.DataLayer.Entities.Permissions;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.Core.Servises
{
    public class PermissionService : IPermissionService
    {
        private TopLearnContext _context;

        public PermissionService(TopLearnContext context)
        {
            _context = context;
        }


        #region Roles

        public List<Role> GetRoles()
        {
            return _context.Roles.ToList();
        }

        public int AddRole(Role role)
        {
            _context.Roles.Add(role);
            _context.SaveChanges();
            return role.RoleId;
        }

        public Role GetRoleById(int roleId)
        {
            return _context.Roles.Find(roleId);
        }

        public void UpdateRole(Role role)
        {
            _context.Roles.Update(role);
            _context.SaveChanges();
        }

        public void DeleteRole(Role role)
        {
            role.IsDelete = true;
            UpdateRole(role);
        }


        public bool IsExistRoel(string roleTitle)
        {
            return _context.Roles.Any(r => r.RoleTitel == roleTitle);
        }


        public void AddRolesToUser(List<int> roleIds, int userId)
        {
            foreach (int roleId in roleIds)
            {
                _context.UserRoles.Add(new UserRole()
                {
                    RoleId = roleId,
                    UsreId = userId
                });
            }
            _context.SaveChanges();
        }

        public void EditRolesUser(List<int> roleIds, int userId)
        {
            _context.UserRoles
                .Where(r => r.UsreId == userId)
                .ToList()
                .ForEach(r => _context.UserRoles.Remove(r));

            AddRolesToUser(roleIds, userId);
        }

        #endregion




        #region Permission

       public  List<Permission> GetAllPermissions()
        {
            return _context.Permission.ToList();
        }
        public void AddPermissionsToRole(int roleId, List<int> selectedPermission)
        {
            foreach(var p in selectedPermission)
            {
                _context.RolePermission.Add(new RolePermission()
                {
                    PermissionId=p,
                    RoleId=roleId
                });
            }
            _context.SaveChanges();
        }

       public List<int> PermissionRole(int roleId)
        {
            return _context.RolePermission
                .Where(r => r.RoleId == roleId)
                .Select(r => r.PermissionId)
                .ToList();

        }

        public void UpdatePermissionsRole(int roleId, List<int> permissions)
        {
            _context.RolePermission
                .Where(r => r.RoleId == roleId)
                .ToList()
                .ForEach(p => _context.RolePermission.Remove(p));

            AddPermissionsToRole(roleId, permissions);
        }

        public bool CheckPermission(int permissionId, string userName)
        {
            int userId = _context.Users.Single(u => u.UserName == userName).UserId;

            List<int> userRole = _context.UserRoles
                .Where(r => r.UsreId == userId)
                .Select(r => r.RoleId)
                .ToList();

            if (!userRole.Any())
                return false;

            List<int> rolesPermission = _context.RolePermission
                .Where(p => p.PermissionId == permissionId)
                .Select(p => p.RoleId)
                .ToList();


            return rolesPermission.Any(p=>userRole.Contains(p));
        }



        #endregion

    }
}
