using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopLearn.Core.Convertors;
using TopLearn.Core.DTOs;
using TopLearn.Core.Senders;
using TopLearn.Core.Servises.Interfaces;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.Web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class HomeController : Controller
    {
        private IUserService _userService;
        private IViewRenderService _viewRender;

        public HomeController(IUserService userService, IViewRenderService viewRender)
        {
            _userService = userService;
            _viewRender = viewRender;
        }


        public IActionResult Index()
        {
            return View(_userService.GetUserInformation(User.Identity.Name));
        }

        [Route("UserPanel/EditPdrofile")]
        public IActionResult EditProfile()
        {
            return View(_userService.GetDataForEditProfileUser(User.Identity.Name));
        }


        [Route("UserPanel/EditPdrofile")]
        [HttpPost]
        public IActionResult EditProfile(EditProfileViewModel profile)
        {
            if (!ModelState.IsValid)
                return View(profile);


            _userService.EditProfile(User.Identity.Name, profile);

            // LogOut
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);



            return Redirect("/Login?EditProfile=true");
        }


        [Route("UserPanel/ChangePassword")]
        public IActionResult ChangePassword()
        {
            return View();
        }


        [Route("UserPanel/ChangePassword")]
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordViewModel changePassword)
        {
            string currentUserName = User.Identity.Name;
            if (!ModelState.IsValid)
                return View(changePassword);

            if (!_userService.CompareOldPassword(changePassword.OldPassword, currentUserName))
            {
                ModelState.AddModelError("OldPassword", "کلمه عبور فعلی صحیح نمی باشد !");
                return View(changePassword);
            }

            _userService.ChangeUserPassword(changePassword.Password, currentUserName);

            ViewBag.IsSuccess = true;
            return View(changePassword);
        }
    }
}
