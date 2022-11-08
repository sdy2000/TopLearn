using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.Convertors;
using TopLearn.Core.Servises.Interfaces;
using TopLearn.DataLayer.Entities.Order;

namespace TopLearn.Web.Pages.Admin.Discount
{
    public class EditDiscountModel : PageModel
    {
        private IOrderService _orderService;
        public EditDiscountModel(IOrderService orderService)
        {
            _orderService = orderService;
        }


        [BindProperty]
        public Descount Descount { get; set; }
        public void OnGet(int id)
        {
            Descount = _orderService.GetDiscountById(id);
        }

        public IActionResult OnPost(string stDate = "", string edDate = "")
        {
            if (string.IsNullOrEmpty(Descount.DescountCode) || Descount.DescountPercent == null)
            {
                return Page();
            }

            if (!string.IsNullOrEmpty(stDate))
            {
                Descount.StartDate = stDate.ToMiladi();

            }

            if (!string.IsNullOrEmpty(edDate))
            {
                Descount.EdnDate = edDate.ToMiladi();
            }


            _orderService.UpdateDiscount(Descount);

            return RedirectToPage("Index");
        }
    }
}
