using Microsoft.Extensions.DependencyInjection;
using RicoCore.Services;
using RicoCore.Services.Content.Contacts;
using RicoCore.Services.Content.Feedbacks;
using RicoCore.Services.Content.Pages;
using RicoCore.Services.Content.PostCategories;
using RicoCore.Services.Content.Posts;
using RicoCore.Services.Content.Slides;
using RicoCore.Services.Content.Tags;
using RicoCore.Services.ECommerce.Bills;
using RicoCore.Services.ECommerce.ProductCategories;
using RicoCore.Services.ECommerce.Products;
using RicoCore.Services.Systems.Accounts;
using RicoCore.Services.Systems.Announcements;
using RicoCore.Services.Systems.Commons;
using RicoCore.Services.Systems.Functions;
using RicoCore.Services.Systems.Logos;
using RicoCore.Services.Systems.Menus;
using RicoCore.Services.Systems.Passwords;
using RicoCore.Services.Systems.Projects;
using RicoCore.Services.Systems.Roles;
using RicoCore.Services.Systems.Settings;
using RicoCore.Services.Systems.Users;

//using RicoCore.Services.ECommerce.Bills;
//using RicoCore.Services.System.Commons;
//using RicoCore.Services.Dapper.Reports;

namespace RicoCore
{
    public static class ServiceRegister
    {
        public static void Register(IServiceCollection services)
        {
            services.AddTransient<IFunctionService, FunctionService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IProductCategoryService, ProductCategoryService>();            
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<IPostCategoryService, PostCategoryService>();
            services.AddTransient<ITagService, TagService>();
            services.AddTransient<IBillService, BillService>();
            services.AddTransient<ISystemConfigService, SystemConfigService>();
            services.AddTransient<ICommonService, CommonService>();
            services.AddTransient<IContactService, ContactService>();
            services.AddTransient<IFeedbackService, FeedbackService>();
            services.AddTransient<IAnnouncementService, AnnouncementService>();            
            services.AddTransient<IPageService, PageService>();
            services.AddTransient<IColorService, ColorService>();
            services.AddTransient<IProductColorService, ProductColorService>();
            services.AddTransient<IMenuService, MenuService>(); 
            services.AddTransient<ISlideService, SlideService>();
            services.AddTransient<ILogoService, LogoService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IPasswordService, PasswordService>();
            services.AddTransient<IProjectService, ProjectService>();
            //services.AddTransient<IReportService, ReportService>();
        }
    }
}