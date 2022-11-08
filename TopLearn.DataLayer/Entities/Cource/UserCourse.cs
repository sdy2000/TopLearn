using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopLearn.DataLayer.Entities.Cource
{
    public class UserCourse
    {
        [Key]
        public int UC_Id { get; set; }


        [Required]
        public int UserId { get; set; }


        [Required]
        public int CourseId { get; set; }





        #region Relation

        [ForeignKey("CourseId")]
        public Course Cource { get; set; }

        [ForeignKey("UserId")]
        public User.User User { get; set; }

        #endregion
    }
}
