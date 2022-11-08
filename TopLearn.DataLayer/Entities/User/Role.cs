using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.DataLayer.Entities.Permissions;

namespace TopLearn.DataLayer.Entities.User
{
    public class Role
    {
        public Role()
        {

        }


        [Key]
        public int RoleId { get; set; }


        [Display(Name = "نام نقش")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد!")]
        public string RoleTitel { get; set; }


        public bool IsDelete { get; set; }




        #region Relations

        public virtual List<UserRole> UserRoles { get; set; }
        public List<RolePermission> RolePermissions { get; set; }

        #endregion
    }
}
