using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopLearn.DataLayer.Entities.Cource
{
    public class CourseEpisode
    {
        [Key]
        public int EpisodeId { get; set; }


        [Required]
        public int CourseId { get; set; }


        [Display(Name = "عنوان بخش")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [MaxLength(400, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد!")]
        public string EpisodeTitle { get; set; }


        [Display(Name = "زمان")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public TimeSpan EpisodeTime { get; set; }


        [Display(Name = "فایل")]
        public string EpisodFileName { get; set; }


        [Display(Name = "رایگان")]
        public bool IsFree { get; set; }




        [ForeignKey("CourseId")]
        public Course Course { get; set; }

    }
}
