using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.DataLayer.Entities.Order;

namespace TopLearn.DataLayer.Entities.User
{
    public class UserDiscountCode
    {
        [Key]
        public int UD_Id { get; set; }


        [Required]
        public int UserId { get; set; }


        [Required]
        public int DiscountId { get; set; }



        #region Relation

        [ForeignKey("UserId")]
        public User User { get; set; }
        [ForeignKey("DiscountId")]
        public Descount Descount { get; set; }

        #endregion
    }
}
