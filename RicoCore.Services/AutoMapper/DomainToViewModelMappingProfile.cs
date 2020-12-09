using AutoMapper;
using RicoCore.Data.Entities;
using RicoCore.Data.Entities.Content;
using RicoCore.Data.Entities.ECommerce;
using RicoCore.Data.Entities.System;
using RicoCore.Services.Content.Contacts.Dtos;
using RicoCore.Services.Content.Feedbacks.Dtos;
using RicoCore.Services.Content.Footers.Dtos;
using RicoCore.Services.Content.Pages.Dtos;
using RicoCore.Services.Content.PostCategories.Dtos;
using RicoCore.Services.Content.Posts.Dtos;
using RicoCore.Services.Content.Slides.Dtos;
using RicoCore.Services.Content.Tags.Dtos;
using RicoCore.Services.ECommerce.Bills.Dtos;
using RicoCore.Services.ECommerce.ProductCategories.Dtos;
using RicoCore.Services.ECommerce.Products.Dtos;
using RicoCore.Services.Systems.Accounts.Dtos;
using RicoCore.Services.Systems.Announcements.Dtos;
using RicoCore.Services.Systems.Functions.Dtos;
using RicoCore.Services.Systems.Menus.Dtos;
using RicoCore.Services.Systems.Passwords.Dtos;
using RicoCore.Services.Systems.Permissions.Dtos;
using RicoCore.Services.Systems.Projects.Dtos;
using RicoCore.Services.Systems.Roles.Dtos;
using RicoCore.Services.Systems.Settings.Dtos;
using RicoCore.Services.Systems.Users.Dtos;

namespace RicoCore.Services.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            //CreateMap<Advertistment, AdvertistmentViewModel>().MaxDepth(2);
            //CreateMap<AdvertistmentPage, AdvertistmentPageViewModel>().MaxDepth(2);
            //CreateMap<AdvertistmentPosition, AdvertistmentPositionViewModel>().MaxDepth(2);

            CreateMap<AppRole, AppRoleViewModel>().MaxDepth(2);
            CreateMap<AppUser, AppUserViewModel>().MaxDepth(2);
            CreateMap<Function, FunctionViewModel>().MaxDepth(2);
            CreateMap<Permission, PermissionViewModel>().MaxDepth(2);
            CreateMap<Setting, SystemConfigViewModel>().MaxDepth(2);
            CreateMap<Announcement, AnnouncementViewModel>().MaxDepth(2);
            CreateMap<AnnouncementUser, AnnouncementUserViewModel>().MaxDepth(2);
            CreateMap<Menu, MenuViewModel>().MaxDepth(2);
            CreateMap<Account, AccountViewModel>().MaxDepth(2);
            CreateMap<Password, PasswordViewModel>().MaxDepth(2);
            CreateMap<Project, ProjectViewModel>().MaxDepth(2);
            CreateMap<SubProject, SubProjectViewModel>().MaxDepth(2);
            CreateMap<Folder, FolderViewModel>().MaxDepth(2);
            CreateMap<File, FileViewModel>().MaxDepth(2);

            CreateMap<ContactDetail, ContactDetailViewModel>().MaxDepth(2);
            CreateMap<Feedback, FeedbackViewModel>().MaxDepth(2);
            CreateMap<Footer, FooterViewModel>().MaxDepth(2);
            CreateMap<Slide, SlideViewModel>().MaxDepth(2);
            CreateMap<Page, PageViewModel>().MaxDepth(2);
            CreateMap<Post, PostViewModel>().MaxDepth(2);
            CreateMap<PostImage, PostImageViewModel>().MaxDepth(2);
            CreateMap<PostTag, PostTagViewModel>().MaxDepth(2);
            CreateMap<PostCategory, PostCategoryViewModel>().MaxDepth(2);
            CreateMap<Tag, TagViewModel>().MaxDepth(2);

            CreateMap<ProductCategory, ProductCategoryViewModel>().MaxDepth(2);
            CreateMap<Product, ProductViewModel>().MaxDepth(2);
            CreateMap<ProductTag, ProductTagViewModel>().MaxDepth(2);
            CreateMap<ProductQuantity, ProductQuantityViewModel>().MaxDepth(2);
            CreateMap<ProductImage, ProductImageViewModel>().MaxDepth(2);
            CreateMap<ProductWishlist, ProductWishlistViewModel>().MaxDepth(2);
            CreateMap<WholePrice, WholePriceViewModel>().MaxDepth(2);
            CreateMap<Color, ColorViewModel>().MaxDepth(2);
            CreateMap<ProductColor, ProductColorViewModel>().MaxDepth(2);
            CreateMap<Size, SizeViewModel>().MaxDepth(2);
            CreateMap<Bill, BillViewModel>().MaxDepth(2);
            CreateMap<BillDetail, BillDetailViewModel>().MaxDepth(2);
        }
    }
}