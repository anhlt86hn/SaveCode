using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RicoCore.Services.Systems.Functions.Dtos;
using RicoCore.Services.Systems.Roles.Dtos;

namespace RicoCore.Services.Systems.Permissions.Dtos
{
    public class PermissionViewModel
    {

        public int Id { get; set; }


        public Guid RoleId { get; set; }

        public int FunctionId { get; set; }

        public bool CanCreate { set; get; }

        public bool CanRead { set; get; }

        public bool CanUpdate { set; get; }

        public bool CanDelete { set; get; }

    }
}