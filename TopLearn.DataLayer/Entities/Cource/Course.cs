using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.DataLayer.Entities.Order;

namespace TopLearn.DataLayer.Entities.Cource
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }


        [Display(Name = "گروه اصلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public int GroupId { get; set; }


        public int? SubGroup { get; set; }


        [Display(Name = "مدرس دوره")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public int TeacherId { get; set; }


        [Display(Name = "وضعیت دوره")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public int StatusId { get; set; }


        
        [Display(Name = "سطح دوره")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public int LevelId { get; set; }


        [Display(Name = "نام دوره")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد!")]
        public string CourseTitle { get; set; }


        [Display(Name = "شرح دوره")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public string Description { get; set; }


        [Display(Name = "قیمت دوره")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public int CoursePrice { get; set; }


        [Display(Name = "تگ ها")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [MaxLength(600, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد!")]
        public string Tags { get; set; }


        [Display(Name = "تصویر دوره")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [MaxLength(100, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد!")]
        public string CourseImageName { get; set; }


        [Display(Name = "دمو دوره")]
        [MaxLength(100, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد!")]
        public string? DemoFileName { get; set; }


        [Display(Name = "تاریخ ایجاد دوره")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public DateTime CreateDate { get; set; }


        [Display(Name = "تاریخ آپدیت دوره")]
        public DateTime? UpdateDate { get; set; }






        #region Relations

        [ForeignKey("TeacherId")]
        public User.User User { get; set; }

        [ForeignKey("GroupId")]
        public CourseGroup CourseGroup { get; set; }

        [ForeignKey("SubGroup")]
        public CourseGroup Group { get; set; }

        [ForeignKey("StatusId")]
        public CourseStatus CourseStatus { get; set; }

        [ForeignKey("LevelId")]
        public CourseLevel CourseLevel { get; set; }
        public List<CourseEpisode> CourseEpisodes { get; set; }
        public virtual List<OrderDetail> OrderDetails { get; set; }
        public List<UserCourse> UserCourses { get; set; }
        public List<CourseComment> CourseComments { get; set; }
        public List<CourseVote> CourseVotes { get; set; }


        #endregion
    }
}
