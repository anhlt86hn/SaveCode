using System;
using RicoCore.Data.Entities;
using RicoCore.Infrastructure.Interfaces;

namespace RicoCore.Services.Systems.AuditLogs
{
    public class AuditLogService : IAuditLogService
    {
        private IRepository<AuditLog, Guid> _errorRepository;
        private IUnitOfWork _unitOfWork;

        public AuditLogService(IRepository<AuditLog, Guid> errorRepository, IUnitOfWork unitOfWork)
        {
            this._errorRepository = errorRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Create(AuditLog error)
        {
            _errorRepository.Insert(error);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}