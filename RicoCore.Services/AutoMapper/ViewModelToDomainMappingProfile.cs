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
using RicoCore.Services.Systems.Logos.Dtos;
using RicoCore.Services.Systems.Menus.Dtos;
using RicoCore.Services.Systems.Passwords.Dtos;
using RicoCore.Services.Systems.Permissions.Dtos;
using RicoCore.Services.Systems.Projects.Dtos;
using RicoCore.Services.Systems.Roles.Dtos;
using RicoCore.Services.Systems.Settings.Dtos;
using RicoCore.Services.Systems.Users.Dtos;

namespace RicoCore.Services.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            //CreateMap<AdvertistmentViewModel, Advertistment>()
            //  .ConstructUsing(c => new Advertistment(c.Name, c.Description, c.Image, c.Url, c.PositionId,c.Status, c.SortOrder));
            //CreateMap<AdvertistmentPositionViewModel, AdvertistmentPosition>()
            //  .ConstructUsing(c => new AdvertistmentPosition(c.PageId, c.Name));
            //CreateMap<AdvertistmentPageViewModel, AdvertistmentPage>()
            // .ConstructUsing(c => new AdvertistmentPage(c.UniqueCode, c.Name));

            CreateMap<AppRoleViewModel, AppRole>()
                .ConstructUsing(c => new AppRole(c.Name, c.Description));
            CreateMap<AppUserViewModel, AppUser>()
            .ConstructUsing(c => new AppUser(c.FullName, c.UserName, c.Email, c.PhoneNumber, c.Avatar, c.Status));
            CreateMap<FunctionViewModel, Function>()
             .ConstructUsing(c => new Function(c.UniqueCode, c.Name, c.ParentId, c.SortOrder, c.IsActive, c.Status, c.Url, c.CssClass));
            CreateMap<PermissionViewModel, Permission>()
            .ConstructUsing(c => new Permission(c.RoleId, c.FunctionId, c.CanCreate, c.CanRead, c.CanUpdate, c.CanDelete));
            CreateMap<SystemConfigViewModel, Setting>()
                .ConstructUsing(c => new Setting(c.Name, c.Url, c.TextValue, c.IntegerValue, c.BooleanValue, c.DateValue, c.DecimalValue, c.Status, c.UniqueCode));
            CreateMap<AnnouncementViewModel, Announcement>()
                .ConstructUsing(c => new Announcement(c.Title, c.Content, c.Status, c.OwnerId));
            CreateMap<AnnouncementUserViewModel, AnnouncementUser>()
                .ConstructUsing(c => new AnnouncementUser(c.AnnouncementId, c.UserId, c.HasRead));
            CreateMap<AccountViewModel, Account>()
                .ConstructUsing(c => new Account(c.Domain, c.UserName, c.PasswordId, c.Password, c.HiddenPassword, c.Phone, c.SecurityEmail, c.Url, c.Note, c.Order, c.Level));
            CreateMap<PasswordViewModel, Password>()
                .ConstructUsing(c => new Password(c.Content, c.Level, c.Order));
            CreateMap<ContactDetailViewModel, ContactDetail>()
                .ConstructUsing(c => new ContactDetail(c.Name, c.Phone, c.Email, c.Website, c.Address, c.Other, c.Lng, c.Lat, c.Status));
            CreateMap<FeedbackViewModel, Feedback>()
                .ConstructUsing(c => new Feedback(c.Id, c.Name, c.Email, c.Message, c.Status));
            CreateMap<FooterViewModel, Footer>()
                .ConstructUsing(c => new Footer(c.Content, c.Status));
            CreateMap<SlideViewModel, Slide>()
               .ConstructUsing(c => new Slide(c.Name, c.Image, c.Url, c.SortOrder, c.Status, c.Content, c.GroupAlias, c.MainCaption, c.SubCaption, c.SmallCaption, c.ActionCaption));
            CreateMap<PageViewModel, Page>()
              .ConstructUsing(c => new Page(c.Id, c.Name, c.Url, c.UniqueCode, c.Content, c.Status, c.MetaTitle, c.MetaDescription, c.MetaKeywords));

            CreateMap<PostViewModel, Post>()
             .ConstructUsing(c => new Post(c.CategoryId, c.Code, c.Url, c.Name, c.Description, c.Content, c.Image, c.Status, c.HomeFlag, c.HotFlag, c.Tags, c.OrderStatus, c.HotOrderStatus,
              c.HomeOrderStatus, c.HotOrder, c.HomeOrder, c.Viewed, c.SortOrder, c.MetaTitle, c.MetaKeywords, c.MetaDescription));

            //CreateMap<PostImageViewModel, PostImage>()
            //    .ConstructUsing(c => new PostImage(c.PostId, c.Caption, c.Path, c.SortOrder));

            CreateMap<PostCategoryViewModel, PostCategory>()
                 .ConstructUsing(c => new PostCategory(c.ParentId, c.Code, c.Url, c.Name, c.Description, c.Image, c.HotFlag, c.HotOrder, c.HomeFlag, c.HomeOrder,
                 c.Status, c.SortOrder, c.MetaTitle, c.MetaKeywords, c.MetaDescription));

            CreateMap<TagViewModel, Tag>()
                .ConstructUsing(c => new Tag(c.Name, c.Type, c.SortOrder, c.MetaTitle, c.MetaKeywords, c.MetaDescription));

            CreateMap<ProductCategoryViewModel, ProductCategory>()
                .ConstructUsing(c => new ProductCategory(c.ParentId, c.Code, c.Url, c.Name, c.Description, c.Image, c.HotFlag, c.HotOrder, c.HomeFlag, c.HomeOrder, c.Status,
                c.SortOrder, c.MetaTitle, c.MetaKeywords, c.MetaDescription));

            CreateMap<ProductViewModel, Product>()
                .ConstructUsing(c => new Product(c.CategoryId, c.Code, c.Url, c.Name, c.Description, c.Content, c.Image, c.Price, c.PromotionPrice, c.OriginalPrice,
                c.HotFlag, c.HotOrder, c.HomeFlag, c.HomeOrder, c.Points, c.Viewed, c.Tags, c.Status, c.SortOrder, c.MetaTitle, c.MetaKeywords, c.MetaDescription));

            CreateMap<ProductWishlistViewModel, ProductWishlist>()
                .ConstructUsing(c => new ProductWishlist(c.Id, c.UserId, c.ProductId));

            CreateMap<BillViewModel, Bill>().ConstructUsing(c => new Bill(c.CustomerName, c.CustomerAddress, c.CustomerMobile, c.CustomerFacebook, c.ShipName, c.ShipAddress, c.ShipMobile, c.CustomerMessage, c.Code, c.ShippingFee, c.BillStatus, c.PaymentMethod, c.Status, c.CustomerId));

            CreateMap<BillDetailViewModel, BillDetail>().ConstructUsing(c => new BillDetail(c.BillId, c.Code, c.ProductItemId, c.ProductItemName, c.Quantity, c.Price, c.Color));
            CreateMap<MenuViewModel, Menu>().ConstructUsing(c => new Menu(c.ParentId, c.Url, c.Name, c.SortOrder, c.Status));
            CreateMap<ColorViewModel, Color>().ConstructUsing(c => new Color(c.Name, c.EnglishName, c.Code, c.Url, c.SortOrder, c.Status));

            CreateMap<LogoViewModel, Logo>().ConstructUsing(c => new Logo(c.Image, c.ImageAlt, c.Size, c.Favicon, c.SortOrder, c.Status));
            CreateMap<ProjectViewModel, Project>().ConstructUsing(c => new Project(c.Url, c.Name, c.SortOrder, c.Status, c.Source, c.IsActive ));
            CreateMap<SubProjectViewModel, SubProject>().ConstructUsing(c => new SubProject(c.ParentId, c.Url, c.Name, c.SortOrder, c.Source));
            CreateMap<FolderViewModel, Folder>().ConstructUsing(c => new Folder(c.ParentId, c.SubProjectId, c.Url, c.Name, c.Source));
            CreateMap<FileViewModel, File>().ConstructUsing(c => new File(c.FolderId, c.SubProjectId, c.Url, c.Name, c.Content, c.Source));
        }
    }
}