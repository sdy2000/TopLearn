using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopLearn.DataLayer.Entities.Cource
{
    public class CourseGroup
    {
        [Key]
        public int GroupId { get; set; }


        [Display(Name = "عنوان گروه")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد!")]
        public string GroupTitle { get; set; }


        [Display(Name = "حذف شده ؟")]
        public bool IsDelete { get; set; }


        [Display(Name ="گروه اصلی")]
        public int? ParentId { get; set; }




        #region Relation

        [ForeignKey("ParentId")]
        public List<CourseGroup> CourseGroups { get; set; }

        [InverseProperty("CourseGroup")]
        public List<Course> Courses { get; set; }

        [InverseProperty("Group")]
        public List<Course> SubGroup { get; set; }

        #endregion
    }
}
