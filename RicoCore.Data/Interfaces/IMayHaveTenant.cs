using System;
using System.Collections.Generic;
using System.Text;

namespace RicoCore.Data.Interfaces
{
    public interface IMayHaveTenant
    {
        Guid? TenantId { get; set; }
    }
}
