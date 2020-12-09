using RicoCore.Services.Content.Contacts.Dtos;
using RicoCore.Services.Content.Feedbacks.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RicoCore.Models
{
    public class ContactPageViewModel
    {
        public ContactDetailViewModel ContactDetail { set; get; }

        public FeedbackViewModel Feedback { set; get; }

    }
}
