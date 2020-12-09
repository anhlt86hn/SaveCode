using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using RicoCore.Data.EF;
using RicoCore.Data.Entities.Content;
using RicoCore.Data.Entities.System;
using RicoCore.Infrastructure.Enums;
using RicoCore.Infrastructure.Interfaces;
using RicoCore.Infrastructure.SharedKernel;
using RicoCore.Services.Content.Posts.Dtos;
using RicoCore.Services.Content.Tags.Dtos;
using RicoCore.Services.Systems.Projects.Dtos;
using RicoCore.Utilities.Constants;
using RicoCore.Utilities.Dtos;
using RicoCore.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RicoCore.Services.Systems.Projects
{
    public class ProjectService : WebServiceBase<Project, int, ProjectViewModel>, IProjectService
    {
        private readonly IRepository<Project, int> _projectRepository;
       
        private readonly IUnitOfWork _unitOfWork;

        public ProjectService(IRepository<Project, int> projectRepository,                      
            IUnitOfWork unitOfWork) : base(projectRepository, unitOfWork)
        {
            _projectRepository = projectRepository;           
            _unitOfWork = unitOfWork;          
        }      
      
        public bool ValidateAddProjectName(ProjectViewModel projectVm)
        {
            return _projectRepository.GetAll().Any(x => x.Name.ToLower() == projectVm.Name.ToLower());
        }
        public bool ValidateUpdateProjectName(ProjectViewModel projectVm)
        {
            var compare = _projectRepository.GetAllIncluding(x => x.Name.ToLower() == projectVm.Name.ToLower());
            var result = compare.Count() > 1 ? true : false;
            return result;
        }

        public bool ValidateAddProjectOrder(ProjectViewModel projectVm)
        {
            return _projectRepository.GetAll().Any(x => x.SortOrder == projectVm.SortOrder && x.SortOrder != 0);
        }
        public bool ValidateUpdateProjectOrder(ProjectViewModel projectVm)
        {
            var compare = _projectRepository.GetAllIncluding(x => x.SortOrder == projectVm.SortOrder && x.SortOrder != 0);
            var result = compare.Count() > 1 ? true : false;
            return result;
        }
        public ProjectViewModel SetValueToNewProject()
        {
            var project = new ProjectViewModel();                              
            project.SortOrder = SetNewProjectOrder();            
            return project;
        }
        public int SetNewProjectOrder()
        {
            var order = 0;
            var list = _projectRepository.GetAllIncluding(x=>x.SortOrder != 0).OrderBy(x => x.SortOrder).Select(x => x.SortOrder).ToList();
            order = CommonMethods.GetOrder(list);
            return order;
        }

       
        public PagedResult<ProjectViewModel> GetAllPaging(string keyword, int page, int pageSize, string sortBy)
        {          
            var query = _projectRepository.GetAll();
            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(x => x.Name.ToLower().Contains(keyword.ToLower()));
            int totalRow = query.Count();
            switch (sortBy)
            {
                case "theo-ten":
                    query = query.OrderBy(x => x.Name);
                    break;

                case "moi-nhat":
                    query = query.OrderBy(x => x.DateModified);
                    break;

                default:
                    query = query.OrderByDescending(x => x.DateCreated);
                    break;
            }           
            var data = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
            var list = new List<ProjectViewModel>();
            foreach (var item in data)
            {
                var projectVm = new ProjectViewModel()
                {
                    Id = item.Id,                    
                    Url = item.Url,
                    Name = item.Name,                 
                    DateCreated = item.DateCreated,
                    DateModified = item.DateModified,
                    DateDeleted = item.DateDeleted,                                               
                    SortOrder = item.SortOrder,                 
                    Status = item.Status,                   
                };
                list.Add(projectVm);
            }
            var paginationSet = new PagedResult<ProjectViewModel>()
            {
                Results = list,
                //Results = data.ProjectTo<ProjectViewModel>().ToList(),
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize,
            };
            return paginationSet;
        }

       
        public ProjectViewModel GetByUrl(string url)
        {
            var project = _projectRepository.FirstOrDefault(x => x.Url == url);
            if (project == null) throw new Exception("Null");
            var vm = Mapper.Map<Project, ProjectViewModel>(project);
            return vm;
        }
     
        
        public override void Add(ProjectViewModel projectVm)
        {
            projectVm.Status = projectVm.IsActive == true ? Status.Actived : Status.InActived;          
            var project = Mapper.Map<ProjectViewModel, Project>(projectVm);
            if (!string.IsNullOrWhiteSpace(projectVm.Name))
            {
                project.Name = projectVm.Name.Trim();
            }
            var getUrlFromName = TextHelper.ToUnsignString(project.Name.ToLower());
            if (string.IsNullOrWhiteSpace(projectVm.Url) || projectVm.Url.ToLower().Trim() == getUrlFromName)
                project.Url = getUrlFromName;
            if(!string.IsNullOrWhiteSpace(projectVm.Url) && projectVm.Url.ToLower().Trim() != getUrlFromName)
                project.Url = projectVm.Url.ToLower().Trim();
       
            var query = _projectRepository.FirstOrDefault(x => x.Url == project.Url);                              
                 
            _projectRepository.Insert(project);
        }      
        
        public override void Update(ProjectViewModel projectVm)
        {
            try
            {
                var project = _projectRepository.GetById(projectVm.Id);
                var createdDate = project.DateCreated;

                //var projectTemp = Projects.FirstOrDefault(x => x.Id == projectVm.Id);
                //var createdDate = projectTemp.DateCreated;

                projectVm.Status = projectVm.IsActive == true ? Status.Actived : Status.InActived;
                

                Mapper.Map(projectVm, project);
                project.DateCreated = createdDate;
                var projectId = project.Id;
                
                if (!string.IsNullOrWhiteSpace(projectVm.Name))
                {
                    project.Name = projectVm.Name.Trim();
                }
                var getUrlFromName = TextHelper.ToUnsignString(project.Name.ToLower());
               
                if(string.IsNullOrWhiteSpace(projectVm.Url) || projectVm.Url.ToLower().Trim() == getUrlFromName )
                project.Url = TextHelper.ToUnsignString(project.Name.ToLower().Trim());

                if (!string.IsNullOrWhiteSpace(projectVm.Url) && projectVm.Url.ToLower().Trim() != getUrlFromName)
                    project.Url = projectVm.Url.ToLower().Trim();

                    var query = _projectRepository.GetAllIncluding(x => x.Url == project.Url).ToList();               
                              
                // ko the tracked
                _projectRepository.Update(project);
            }
            catch (Exception ex) 
            {
                throw ex;
            }
            
        }

        public List<ProjectViewModel> GetLastest(int top)
        {
            return _projectRepository.GetAll().Where(x => x.Status == Status.Actived).OrderByDescending(x => x.DateCreated)
                .Take(top).ProjectTo<ProjectViewModel>().ToList();
        }
              

        public void MultiDelete(IList<string> selectedIds)
        {
            foreach (var item in selectedIds)
            {
                var project = _projectRepository.FirstOrDefault(x => x.Id == int.Parse(item));
                var projectId = project.Id;
                _projectRepository.Delete(projectId);

                //var project = Projects.Where(r => r.Id == Guid.Parse(item)).FirstOrDefault();
                //Projects.Remove(project);
                             
            }
            _unitOfWork.Commit();
        }

        
    }
}