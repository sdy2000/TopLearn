using Microsoft.AspNetCore.Mvc;
using TopLearn.Core.Convertors;
using TopLearn.Core.Servises.Interfaces;
using TopLearn.DataLayer.Entities.User;
using TopLearn.Core.Generators;
using TopLearn.Core.Security;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using TopLearn.Core.Senders;
using TopLearn.Core.DTOs;

namespace TopLearn.Web.Controllers
{
    public class AccountController : Controller
    {
        private IUserService _userService;
        private IViewRenderService _viewRender;

        public AccountController(IUserService userService, IViewRenderService viewRender)
        {
            _userService = userService;
            _viewRender = viewRender;
        }


        #region Register

        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register(RegisterViewModel register)
        {
            if (!ModelState.IsValid)
                return View(register);

            if (_userService.IsExistUserName(register.UserName))
            {
                ModelState.AddModelError("UserName", "نام کاربری معتبر نمی باشد!");
                return View(register);
            }
            if (_userService.IsExistEmail(FixedText.FixEmail(register.Email)))
            {
                ModelState.AddModelError("Email", "ایمیل کاربری معتبر نمی باشد!");
                return View(register);
            }

            User user = new User()
            {
                UserName = register.UserName,
                Email = FixedText.FixEmail(register.Email),
                Password = PasswordHelper.EncodePasswordMd5(register.Password),
                ActiveCode = NameGenerator.GenerateUniqCode(),
                RegisterDate = DateTime.Now,
                UserAvatar = "Defult.jpg"
            };

            _userService.AddUser(user);

            #region Send Activation Email

            string body = _viewRender.RenderToStringAsync("_ActiveEmail", user);
            SendEmail.Send(user.Email, "فعالسازی", body);

            #endregion

            return View("SuccessRegister", user);
        }

        #endregion


        #region Login

        [Route("Login")]
        public IActionResult LogIn(bool EditProfile=false)
        {
            ViewBag.EditProfile = EditProfile;
            return View();
        }


        [Route("Login")]
        [HttpPost]
        public IActionResult LogIn(LoginViewModel login)
        {
            if (!ModelState.IsValid)
                return View(login);

            var user = _userService.LoginUser(login);
            if (user != null)
            {
                if (user.IsActive)
                {
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                        new Claim(ClaimTypes.Name, user.UserName)
                    };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    var properties = new AuthenticationProperties
                    {
                        IsPersistent = login.RememberMe
                    };
                    HttpContext.SignInAsync(principal, properties);


                    ViewBag.IsSuccess = true;
                    return View();
                }
                else
                {
                    ModelState.AddModelError("Email", "حساب کاربری شما فعال نمی باشد !");
                    return View(login);
                }
            }

            ModelState.AddModelError("Email", "کاربری با این مشخصات یافت نشد !");
            return View(login);

        }

        #endregion


        #region Logout

        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Login");
        }

        #endregion


        #region Active Account

        public IActionResult ActiveAccount(string id)
        {
            ViewBag.Isactive = _userService.ActiveAccount(id);
            return View();
        }

        #endregion


        #region ForgotPassword

        [Route("ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [Route("ForgotPassword")]
        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordViewModel forgot)
        {
            if (!ModelState.IsValid)
                return View(forgot);

            string fixedEmail = FixedText.FixEmail(forgot.Email);
            User user = _userService.GetUserByEmail(fixedEmail);

            if (user == null)
            {
                ModelState.AddModelError("Email", "کاربری یافت نشد");
                return View(forgot);
            }


            #region Send Activation Email

            string body = _viewRender.RenderToStringAsync("_ForgotPassword", user);
            SendEmail.Send(user.Email, "بازیابی حساب کاربری", body);

            #endregion



            ViewBag.IsSuccess = true;
            return View();
        }


        #endregion


        #region Reset Password

        public IActionResult ResetPassword(string id)
        {
            return View(new ResetPasswordViewModel()
            {
                ActiveCode = id
            });
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel reset)
        {
            if (!ModelState.IsValid)
                return View(reset);

            User user = _userService.GetUserByActiveCode(reset.ActiveCode);

            if (user == null)
                return NotFound();

            string hashNewPassword = PasswordHelper.EncodePasswordMd5(reset.Password);
            user.Password = hashNewPassword;
            _userService.UpdateUser(user);


            return Redirect("/Login");
        }

        #endregion


    }
}
