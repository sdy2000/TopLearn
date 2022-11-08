using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.DTOs;
using TopLearn.Core.Security;
using TopLearn.Core.Servises.Interfaces;

namespace TopLearn.Web.Pages.Admin.Users
{
    [PermissionChecker(4)]
    public class EditUserModel : PageModel
    {
        private IUserService _userService;
        private IPermissionService _permissionService;

        public EditUserModel(IUserService userService, IPermissionService permissionService)
        {
            _userService = userService;
            _permissionService = permissionService;
        }

        [BindProperty]
        public EditUserViewModel EditUserViewModel { get; set; }
        public void OnGet(int id)
        {
            EditUserViewModel = _userService.GetUserForShowInEditMode(id);
            ViewData["Roles"] = _permissionService.GetRoles();
        }



        public IActionResult OnPost(List<int> SelectedRoles, int userId)
        {

            if (EditUserViewModel.UserName == null || EditUserViewModel.Email == null)
            {
                EditUserViewModel = _userService.GetUserForShowInEditMode(EditUserViewModel.UserId);
                ViewData["Roles"] = _permissionService.GetRoles();
                return Page();
            }

            if (EditUserViewModel.UserName != _userService.GetUserNameById(EditUserViewModel.UserId))
            {
                if (_userService.IsExistUserName(EditUserViewModel.UserName))
                {
                    EditUserViewModel = _userService.GetUserForShowInEditMode(EditUserViewModel.UserId);
                    ViewData["Roles"] = _permissionService.GetRoles();
                    return Page();
                }
            }
            if(EditUserViewModel.Email != _userService.GetEmailById(EditUserViewModel.UserId))
            {
                if (_userService.IsExistEmail(EditUserViewModel.Email))
                {
                    EditUserViewModel = _userService.GetUserForShowInEditMode(EditUserViewModel.UserId);
                    ViewData["Roles"] = _permissionService.GetRoles();
                    return Page();
                }
            }

            _userService.EditUserFromAdmin(EditUserViewModel);
            _permissionService.EditRolesUser(SelectedRoles, EditUserViewModel.UserId);

            return RedirectToPage("Index");
        }
    }
}
