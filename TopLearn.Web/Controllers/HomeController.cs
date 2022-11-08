using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyEshop.Web.Models;
using System.Diagnostics;
using TopLearn.Core.Servises.Interfaces;

namespace MyEshop.Web.Controllers
{
    public class HomeController : Controller
    {
        private IUserService _userService;
        private ICourseService _courseService;

        public HomeController(IUserService userService, ICourseService courseService)
        {
            _userService = userService;
            _courseService = courseService;
        }

        public IActionResult Index()
        {
            ViewBag.PopularCourse = _courseService.GetPopularCourse();
           return View(_courseService.GetCourse().Item1);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        [Route("OnlinePayment/{id}")]
        public IActionResult OnlinePayment(int id)
        {
            if (HttpContext.Request.Query["Status"]!=""&&
                HttpContext.Request.Query["Status"].ToString().ToLower()=="ok"&&
                HttpContext.Request.Query["Authority"] != "")
            {
                string authority = HttpContext.Request.Query["Authority"];

                var wallet = _userService.GetWalletByWalletId(id);


                var payment = new ZarinpalSandbox.Payment(wallet.Amount);
                var res = payment.Verification(authority).Result;
                if (res.Status == 100)
                {
                    ViewBag.Code = res.RefId;
                    ViewBag.IsSuccess = true;
                    wallet.IsPay = true;
                    _userService.UpdateWallet(wallet);
                }
            }

            return View();
        }

        public IActionResult GetSubGroups(int id)
        {
           List<SelectListItem> list = new List<SelectListItem>()
            {
                new SelectListItem(){Text="انتخاب کنید",Value="0"}
            };


            list.AddRange(_courseService.GetSubGroupForManageCourse(id));

            return Json(new SelectList(list, "Value", "Text"));
        }

        
        [HttpPost]
        [Route("file-upload")]
        public IActionResult UploadImage(IFormFile upload, string CKEditorFuncNum, string CKEditor, string langCode)
        {
            if (upload.Length <= 0) return null;

            var fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName).ToLower();



            var path = Path.Combine(
                Directory.GetCurrentDirectory(), "wwwroot/MyImages",
                fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                upload.CopyTo(stream);

            }



            var url = $"{"/MyImages/"}{fileName}";


            return Json(new { uploaded = true, url });
        }

    }
}