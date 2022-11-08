using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopLearn.DataLayer.Entities.Cource
{
    public class CourseStatus
    {
        [Key]
        public int StatusId { get; set; }


        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد!")]
        public string StatusTitle { get; set; }





        public List<Course> Courses { get; set; }
    }
}
