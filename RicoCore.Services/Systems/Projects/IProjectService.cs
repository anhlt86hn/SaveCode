using RicoCore.Data.Entities.Content;
using RicoCore.Data.Entities.System;
using RicoCore.Services.Content.Posts.Dtos;
using RicoCore.Services.Content.Tags.Dtos;
using RicoCore.Services.Systems.Projects.Dtos;
using RicoCore.Utilities.Dtos;
using System;
using System.Collections.Generic;

namespace RicoCore.Services.Systems.Projects
{ 
    public interface IProjectService : IWebServiceBase<Project, int, ProjectViewModel>
    {       
        bool ValidateAddProjectName(ProjectViewModel postVm);
        bool ValidateUpdateProjectName(ProjectViewModel postVm);

        bool ValidateAddProjectOrder(ProjectViewModel postVm);
        bool ValidateUpdateProjectOrder(ProjectViewModel postVm);
        ProjectViewModel SetValueToNewProject();
        int SetNewProjectOrder();
              
        PagedResult<ProjectViewModel> GetAllPaging(string keyword, int page, int pageSize, string sort);
        ProjectViewModel GetByUrl(string url);     
        List<ProjectViewModel> GetLastest(int top);           

        void MultiDelete(IList<string> selectedIds);
     
    }
}