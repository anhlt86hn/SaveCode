using System;
using System.Collections.Generic;
using RicoCore.Services.Content.Feedbacks.Dtos;
using RicoCore.Data.Entities;
using RicoCore.Utilities.Dtos;
using RicoCore.Data.Entities.Content;

namespace RicoCore.Services.Content.Feedbacks
{
    public interface IFeedbackService : IWebServiceBase<Feedback ,Guid, FeedbackViewModel>
    {
        //void Add(FeedbackViewModel feedbackVm);
        //void Update(FeedbackViewModel feedbackVm);    
        //void Delete(int id);
        //FeedbackViewModel GetById(int id);
        //List<FeedbackViewModel> GetAll();  
        PagedResult<FeedbackViewModel> GetAllPaging(string keyword, int page, int pageSize);
    }
}