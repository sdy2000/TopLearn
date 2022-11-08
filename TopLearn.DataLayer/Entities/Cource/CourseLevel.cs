using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopLearn.DataLayer.Entities.Cource
{
    public class CourseLevel
    {
        [Key]
        public int LevelId { get; set; }


        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد!")]
        public string LevelTitle { get; set; }



        public List<Course> Courses { get; set; }
    }
}
