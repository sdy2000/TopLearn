using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopLearn.Core.DTOs.Order;
using TopLearn.Core.Servises.Interfaces;

namespace TopLearn.Web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class MyOrdersController : Controller
    {
           private IOrderService _orderService;
        public MyOrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public IActionResult Index()
        { 
            return View(_orderService.GetUserOrders(User.Identity.Name));
        }

        public IActionResult ShowOrder(int id,bool finaly = false,string type="")
        {
            var order = _orderService.GetOrderForUserPanel(User.Identity.Name, id);

            if (order == null)
                return NotFound();

            ViewBag.finaly = finaly;
            ViewBag.Type = type;
            return View(order);
        }

        public IActionResult FinalyOrder(int id)
        {
            if (_orderService.FinalyOrder(User.Identity.Name, id))
            {
                return Redirect("/UserPanel/MyOrders/ShowOrder/" + id + "?finaly=true");
            }

            return BadRequest();
        }

        public IActionResult UseDescount(int orderId,string code)
        {
            DiscountUseType type = _orderService.UseDiscount(orderId, code);

            return Redirect("/UserPanel/MyOrders/ShowOrder/" + orderId + "?type=" + type.ToString());
        }
    }
}
