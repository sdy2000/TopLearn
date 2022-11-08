using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.Security;
using TopLearn.Core.Servises.Interfaces;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.Web.Pages.Admin.Roles
{
    [PermissionChecker(7)]
    public class CreateRoleModel : PageModel
    {
        private IPermissionService _permissionService;

        public CreateRoleModel(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [BindProperty]
        public Role Role { get; set; }
        public void OnGet()
        {
            ViewData["Permissions"] = _permissionService.GetAllPermissions();
        }

        public IActionResult OnPost(List<int> selectedPermission)
        {
            if (string.IsNullOrEmpty(Role.RoleTitel))
            {
                ViewData["Permissions"] = _permissionService.GetAllPermissions();
                return Page();

            }

            if (_permissionService.IsExistRoel(Role.RoleTitel))
            {
                ViewData["Permissions"] = _permissionService.GetAllPermissions();
                ModelState.AddModelError("Role.RoleTitel", "نقش وارد شده موجود می باشد");
                return Page();

            }


            Role.IsDelete = false;
            int roleId = _permissionService.AddRole(Role);


            _permissionService.AddPermissionsToRole(Role.RoleId, selectedPermission);


            return RedirectToPage("Index");
        }
    }
}
