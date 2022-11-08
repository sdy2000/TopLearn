using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopLearn.Core.DTOs;
using TopLearn.Core.Servises.Interfaces;

namespace TopLearn.Web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class WalletController : Controller
    {
        private IUserService _userService;

        public WalletController(IUserService userService)
        {
            _userService = userService;
        }


        [Route("UserPanel/Wallet")]
        public IActionResult Wallet()
        {

            ViewBag.ListWallet = _userService.GetWalletUser(User.Identity.Name);
            return View();
        }


        [Route("UserPanel/Wallet")]
        [HttpPost]
        public IActionResult Wallet(ChargeWalletViewModel charge)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ListWallet = _userService.GetWalletUser(User.Identity.Name);
                return View(charge);
            }

           int walletId = _userService.ChargeWallet(User.Identity.Name, charge.Amount, "شارژ حساب");



            #region Online Payment

            var payment = new ZarinpalSandbox.Payment(charge.Amount);
            var res = payment.PaymentRequest("شارژ کیف پول", "http://localhost:5114/OnlinePayment/" + walletId, "sajjad.darvish.yektayi@gmail.com", "09370776595");

            if (res.Result.Status == 100)
            {
                return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + res.Result.Authority);
            }

            #endregion



            return null;
        }
    }
}
