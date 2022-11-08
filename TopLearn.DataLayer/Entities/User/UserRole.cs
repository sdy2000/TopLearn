using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopLearn.DataLayer.Entities.User
{
    public class UserRole
    {
        public UserRole()
        {

        }
        

        [Key]
        public int UR_Id { get; set; }
        public int UsreId { get; set; }
        public int RoleId { get; set; }





        #region Relations

        [ForeignKey("UsreId")]
        public virtual User User { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }

        #endregion

    }
}
