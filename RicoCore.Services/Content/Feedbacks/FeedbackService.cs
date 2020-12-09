using AutoMapper.QueryableExtensions;
using System;
using System.Linq;
using RicoCore.Services.Content.Feedbacks.Dtos;
using RicoCore.Data.Entities;
using RicoCore.Infrastructure.Interfaces;
using RicoCore.Utilities.Dtos;
using RicoCore.Data.Entities.Content;

namespace RicoCore.Services.Content.Feedbacks
{
    public class FeedbackService : WebServiceBase<Feedback, Guid, FeedbackViewModel>, IFeedbackService
    {
        private readonly IRepository<Feedback, Guid> _feedbackRepository;

        public FeedbackService(IRepository<Feedback, Guid> feedbackRepository,
            IUnitOfWork unitOfWork) : base(feedbackRepository, unitOfWork)
        {
            _feedbackRepository = feedbackRepository;
        }        

        public PagedResult<FeedbackViewModel> GetAllPaging(string keyword, int page, int pageSize)
        {
            var query = _feedbackRepository.GetAll();
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Name.Contains(keyword));

            int totalRow = query.Count();
            var data = query.OrderByDescending(x => x.DateCreated)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var paginationSet = new PagedResult<FeedbackViewModel>()
            {
                Results = data.ProjectTo<FeedbackViewModel>().ToList(),
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paginationSet;
        }
    }
}