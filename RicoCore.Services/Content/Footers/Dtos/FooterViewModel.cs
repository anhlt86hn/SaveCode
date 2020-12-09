using RicoCore.Infrastructure.Enums;

namespace RicoCore.Services.Content.Footers.Dtos
{
    public class FooterViewModel
    {
        public string Id { set; get; }
        public string Content { set; get; }
        public Status Status { set; get; }
    }
}