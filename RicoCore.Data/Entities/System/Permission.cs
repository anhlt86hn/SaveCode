using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.System
{
    [Table("Permissions")]
    public class Permission : DomainEntity<int>
    {
        public Permission()
        {
        }

        public Permission(Guid roleId, int functionId, bool canCreate, bool canRead, bool canUpdate, bool canDelete)
        {
            RoleId = roleId;
            FunctionId = functionId;
            CanCreate = canCreate;
            CanRead = canRead;
            CanUpdate = canUpdate;
            CanDelete = canDelete;
        }

        [StringLength(450)]
        [Required]
        public Guid RoleId { get; set; }

        [StringLength(128)]
        [Required]
        public int FunctionId { get; set; }

        public bool CanCreate { set; get; }
        public bool CanRead { set; get; }

        public bool CanUpdate { set; get; }
        public bool CanDelete { set; get; }


        //[ForeignKey("RoleId")]
        //public virtual AppRole AppRole { get; set; }

        //[ForeignKey("FunctionId")]
        //public virtual Function Function { get; set; }
    }
}