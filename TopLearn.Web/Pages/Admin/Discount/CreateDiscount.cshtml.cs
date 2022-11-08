using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.Convertors;
using TopLearn.Core.Servises.Interfaces;
using TopLearn.DataLayer.Entities.Order;

namespace TopLearn.Web.Pages.Admin.Discount
{
    public class CreateDiscountModel : PageModel
    {
        private IOrderService _orderService;
        public CreateDiscountModel(IOrderService orderService)
        {
            _orderService = orderService;
        }


        [BindProperty]
        public Descount Descount { get; set; }
        public void OnGet()
        {
        }

        public IActionResult OnPost(string stDate="",string edDate = "")
        {
            if (string.IsNullOrEmpty(Descount.DescountCode) || Descount.DescountPercent == null||
                _orderService.IsExistCode(Descount.DescountCode))
            {
                return Page();
            }

            if (stDate != "")
            {
                Descount.StartDate = stDate.ToMiladi();
                 
            }

            if (edDate != "")
            {
                Descount.EdnDate = edDate.ToMiladi();
            }


            _orderService.AddDiscount(Descount);

            return RedirectToPage("Index");
        }


        public IActionResult OnGetCheckCode(string code)
        {
            return Content(_orderService.IsExistCode(code).ToString());
        }
    }
}
