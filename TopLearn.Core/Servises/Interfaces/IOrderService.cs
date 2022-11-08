using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.Core.DTOs.Order;
using TopLearn.DataLayer.Entities.Order;

namespace TopLearn.Core.Servises.Interfaces
{
    public interface IOrderService
    {
        int AddOrder(string UserName, int courseId);
        void UpdatePriceOrder(int orderId);
        Order GetOrderForUserPanel(string userName, int orderId);
        Order GetOrderById(int orderId);
        bool FinalyOrder(string userName, int orderId);
        List<Order> GetUserOrders(string userName);
        void UpdateOrder(Order order);
        bool IsUserInCourse(string userName, int courseId);


        #region Discount

        DiscountUseType UseDiscount(int orderId, string code);
        void AddDiscount(Descount descount);
        List<Descount> GetAllDiscounts();
        Descount GetDiscountById(int discountId);
        void UpdateDiscount(Descount descount);
        bool IsExistCode(string code);

        #endregion
    }
}
