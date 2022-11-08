using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.DTOs;
using TopLearn.Core.Security;
using TopLearn.Core.Servises.Interfaces;

namespace TopLearn.Web.Pages.Admin.Users
{
    [PermissionChecker(2)]
    public class IndexModel : PageModel
    {
        private IUserService _userService;

        public IndexModel(IUserService userService)
        {
            _userService = userService;
        }


        public UsersForAdminViewModel UsersForAdminViewModel { get; set; }
        public void OnGet(int pageId=1, string filterUserName="",string filterEmail="")
        {
            UsersForAdminViewModel = _userService.GetUsers(pageId,filterEmail,filterUserName);
        }

    }
}
