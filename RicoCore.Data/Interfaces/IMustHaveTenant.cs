using System;
using System.Collections.Generic;
using System.Text;

namespace RicoCore.Data.Interfaces
{
    interface IMustHaveTenant
    {
      Guid TenantId { get; set; }

    }
}
