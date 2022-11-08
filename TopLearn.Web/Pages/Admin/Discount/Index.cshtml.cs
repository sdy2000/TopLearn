using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.Servises.Interfaces;
using TopLearn.DataLayer.Entities.Order;

namespace TopLearn.Web.Pages.Admin.Discount
{
    public class IndexModel : PageModel
    {
        private IOrderService _orderService;
        public IndexModel(IOrderService orderService)
        {
            _orderService = orderService;
        }


        [BindProperty]
        public List<Descount> Descounts { get; set; }
        public void OnGet()
        {
            Descounts = _orderService.GetAllDiscounts();
        }
    }
}
