using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.DataLayer.Entities.Permissions
{
    public class RolePermission
    {
        [Key]
        public int PR_Id { get; set; }
        public int RoleId { get; set; }
        public int PermissionId { get; set; }






        #region Relations

        [ForeignKey("RoleId")]
        public Role Role { get; set; }

        [ForeignKey("PermissionId")]
        public Permission Permission { get; set; }

        #endregion
    }
}
