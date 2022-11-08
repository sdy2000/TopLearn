using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.Security;
using TopLearn.Core.Servises.Interfaces;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.Web.Pages.Admin.Roles
{
    [PermissionChecker(8)]
    public class EditRoleModel : PageModel
    {
        private IPermissionService _permissionService;

        public EditRoleModel(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [BindProperty]
        public Role Role { get; set; }
        public void OnGet(int id)
        {
            ViewData["Permissions"] = _permissionService.GetAllPermissions();
            ViewData["SelectedPermission"] = _permissionService.PermissionRole(id);

            Role = _permissionService.GetRoleById(id);
        }

        public IActionResult OnPost(List<int> selectedPermission)
        {
            if (string.IsNullOrEmpty(Role.RoleTitel))
            {
                ViewData["SelectedPermission"] = _permissionService.PermissionRole(Role.RoleId);
                ViewData["Permissions"] = _permissionService.GetAllPermissions();
                return Page();

            }


            _permissionService.UpdateRole(Role);


            _permissionService.UpdatePermissionsRole(Role.RoleId, selectedPermission);

            return RedirectToPage("Index");
        }
    }
}
