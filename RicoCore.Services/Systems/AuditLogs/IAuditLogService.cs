using System;
using System.Collections.Generic;
using System.Text;
using RicoCore.Data.Entities;

namespace RicoCore.Services.Systems.AuditLogs
{
    public interface IAuditLogService
    {
        void Create(AuditLog error);

        void Save();
    }
}
