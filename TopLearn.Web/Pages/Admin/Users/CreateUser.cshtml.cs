using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.DTOs;
using TopLearn.Core.Security;
using TopLearn.Core.Servises.Interfaces;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.Web.Pages.Admin.Users
{
    [PermissionChecker(3)]
    public class CreateUserModel : PageModel
    {
        private IUserService _userService;
        private IPermissionService _permissionService;

        public CreateUserModel(IUserService userService, IPermissionService permissionService)
        {
            _userService = userService;
            _permissionService = permissionService;
        }

        [BindProperty]
        public CreateUserViewModel CreateUserViewModel { get; set; }
        public void OnGet()
        {
            ViewData["Roles"] = _permissionService.GetRoles();
        }

        public IActionResult OnPost(List<int> SelectedRoles)
        {
            if (CreateUserViewModel.UserName == null || CreateUserViewModel.Email == null)
            {
                CreateUserViewModel = CreateUserViewModel;
                ViewData["Roles"] = _permissionService.GetRoles();
                return Page();
            }

            if (_userService.IsExistEmail(CreateUserViewModel.Email)||
                _userService.IsExistUserName(CreateUserViewModel.UserName))
            {
                CreateUserViewModel = CreateUserViewModel;
                ViewData["Roles"] = _permissionService.GetRoles();
                return Page();
            }

            int userId = _userService.AddUserFromAdmin(CreateUserViewModel);

            // Add Rolse
            _permissionService.AddRolesToUser(SelectedRoles, userId);

            return Redirect("/Admin/Users");
        }
    }
}
