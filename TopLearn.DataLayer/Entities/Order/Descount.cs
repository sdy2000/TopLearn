using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.DataLayer.Entities.Order
{
    public class Descount
    {
        [Key]
        public int DiscountId { get; set; }

        [Display(Name = "کد تخفیف")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد!")]
        public string DescountCode { get; set; }


        [Display(Name = "درصد تخفیف")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public int DescountPercent { get; set; }


        public int? UsableCount { get; set; }


        public DateTime? StartDate { get; set; }


        public DateTime? EdnDate { get; set; }






        public List<UserDiscountCode> UserDiscountCodes { get; set; }
    }
}
