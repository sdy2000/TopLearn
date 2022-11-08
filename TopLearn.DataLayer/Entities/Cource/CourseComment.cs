using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopLearn.DataLayer.Entities.Cource
{
    public class CourseComment
    {
        [Key]
        public int CommetnId { get; set; }


        public int CourseId { get; set; }


        public int UserId { get; set; }


        [Display(Name = "نظر")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [MaxLength(900, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد!")]
        public string Comment { get; set; }


        public DateTime CreateDate { get; set; }


        public bool IsDelete { get; set; }


        public bool IsAdminRead { get; set; }





        #region Relation

        [ForeignKey("CourseId")]
        public Course Course { get; set; }

        [ForeignKey("UserId")]
        public User.User User { get; set; }

        #endregion

    }
}
