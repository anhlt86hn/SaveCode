using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RicoCore.Data.Entities;
using RicoCore.Infrastructure.Enums;
using RicoCore.Utilities.Constants;
using RicoCore.Data.Entities.System;
using RicoCore.Data.Entities.Content;
using RicoCore.Infrastructure.Interfaces;
using RicoCore.Infrastructure.SharedKernel;
using Microsoft.EntityFrameworkCore;
using RicoCore.Data.Entities.ECommerce;

namespace RicoCore.Data.EF
{
    public class DbInitializer
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IRepository<Post, Guid> _postRepository;
        private readonly DbSet<PostCategory> PostCategories;

        public DbInitializer(AppDbContext context,
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            IRepository<Post, Guid> postRepository)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _postRepository = postRepository;
            PostCategories = context.Set<PostCategory>();
        }

        private string GenerateCode()
        {            
            return CommonHelper.GenerateRandomCode();            
        }

        public async Task Seed()
        {
            try
            {
                var ricoRoleId = Guid.NewGuid();
                var adminRoleId = Guid.NewGuid();
                var managerRoleId = Guid.NewGuid();
                var employeeRoleId = Guid.NewGuid();
                var editerRoleId = Guid.NewGuid();
                #region Role
                if (!_roleManager.Roles.Any())
                {                   
                    await _roleManager.CreateAsync(new AppRole() { Id = ricoRoleId, Name = "Rico", NormalizedName = "Rico", Description = "Rico" });
                    await _roleManager.CreateAsync(new AppRole() { Id = adminRoleId, Name = "Admin", NormalizedName = "Admin", Description = "Admin" });
                    await _roleManager.CreateAsync(new AppRole() { Id = managerRoleId, Name = "Quản lý", NormalizedName = "Quản lý", Description = "Quản lý" });
                    await _roleManager.CreateAsync(new AppRole() { Id = employeeRoleId, Name = "Nhân viên", NormalizedName = "Nhân viên", Description = "Nhân viên" });
                    await _roleManager.CreateAsync(new AppRole() { Id = editerRoleId, Name = "Biên tập viên", NormalizedName = "Biên tập viên", Description = "Biên tập viên" });
                }
                #endregion

                #region User
                if (!_userManager.Users.Any())
                {
                    await _userManager.CreateAsync(new AppUser() { Id = Guid.NewGuid(), UserName = "rico", FullName = "rico", Email = "luonganh@gmail.com", Status = Status.Actived, DateCreated = DateTime.Now }, "Alo12345");
                    var user1 = await _userManager.FindByNameAsync("rico"); await _userManager.AddToRoleAsync(user1, "Admin");                   
                }
                #endregion

                #region Function            
                if (!_context.Functions.Any())
                {
                    try
                    {
                        //_context.Functions.Add(new Function() { UniqueCode = "SECURITY", Name = "BẢO MẬT", ParentId = null, SortOrder = 1, IsActive = true, Status = Status.Actived, Url = "/", CssClass = "fa-desktop" });
                        //_context.SaveChanges();
                        _context.Functions.Add(new Function() { UniqueCode = "MANAGEMENT", Name = "QUẢN TRỊ", ParentId = null, SortOrder = 2, IsActive = true, Status = Status.Actived, Url = "/", CssClass = "fa-desktop" });
                        _context.SaveChanges();
                        _context.Functions.Add(new Function() { UniqueCode = "CHECKLIST", Name = "Checklist", ParentId = null, SortOrder = 3, IsActive = true, Status = Status.Actived, Url = "/", CssClass = "fa-desktop" });
                        _context.SaveChanges();
                        _context.Functions.Add(new Function() { UniqueCode = "GENERAL", Name = "Tổng hợp", ParentId = null, SortOrder = 4, IsActive = true, Status = Status.Actived, Url = "/", CssClass = "fa-desktop" });
                        _context.SaveChanges();
                        _context.Functions.Add(new Function() { UniqueCode = "BACKEND", Name = "Back-End", ParentId = null, SortOrder = 5, IsActive = true, Status = Status.Actived, Url = "/", CssClass = "fa-desktop" });
                        _context.SaveChanges();
                        _context.Functions.Add(new Function() { UniqueCode = "FRONTEND", Name = "Front-End", ParentId = null, SortOrder = 6, IsActive = true, Status = Status.Actived, Url = "/", CssClass = "fa-desktop" });
                        _context.SaveChanges();
                        _context.Functions.Add(new Function() { UniqueCode = "DATABASE", Name = "Database", ParentId = null, SortOrder = 7, IsActive = true, Status = Status.Actived, Url = "/", CssClass = "fa-desktop" });
                        _context.SaveChanges();                       
                        _context.Functions.Add(new Function() { UniqueCode = "ENGLISH", Name = "English", ParentId = null, SortOrder = 8, IsActive = true, Status = Status.Actived, Url = "/", CssClass = "fa-desktop" });
                        _context.SaveChanges();

                        //_context.Functions.Add(new Function() { UniqueCode = "ACCOUNT", Name = "TÀI KHOẢN", ParentId = 1, SortOrder = 1, IsActive = true, Status = Status.Actived, Url = "/admin/account/index", CssClass = "fa-table" });
                        //_context.SaveChanges();
                        //_context.Functions.Add(new Function() { UniqueCode = "PASSWORD", Name = "Mật khẩu", ParentId = 1, SortOrder = 2, IsActive = true, Status = Status.Actived, Url = "/admin/password/index", CssClass = "fa-chevron-down" });
                        //_context.SaveChanges();

                        _context.Functions.Add(new Function() { UniqueCode = "POST_LIST", Name = "BÀI VIẾT", ParentId = 1, SortOrder = 1, IsActive = true, Status = Status.Actived, Url = "/admin/post/index", CssClass = "fa-table" });
                        _context.SaveChanges();
                        _context.Functions.Add(new Function() { UniqueCode = "POST_CATEGORY", Name = "Danh mục bài viết", ParentId = 1, SortOrder = 2, IsActive = true, Status = Status.Actived, Url = "/admin/postcategory/index", CssClass = "fa-chevron-down" });
                        _context.SaveChanges();
                        _context.Functions.Add(new Function() { UniqueCode = "POST_TAG", Name = "Tag bài viết", ParentId = 1, SortOrder = 3, IsActive = true, Status = Status.Actived, Url = "/admin/posttag/index", CssClass = "fa-tag" });
                        _context.SaveChanges();
                        _context.Functions.Add(new Function() { UniqueCode = "PAGE", Name = "Trang", ParentId = 1, SortOrder = 4, IsActive = true, Status = Status.Actived, Url = "/admin/page/index", CssClass = "fa-table" });
                        _context.SaveChanges();
                        _context.Functions.Add(new Function() { UniqueCode = "FUNCTION", Name = "Chức năng", ParentId = 1, SortOrder = 5, IsActive = true, Status = Status.Actived, Url = "/admin/function/index", CssClass = "fa-home" });
                        _context.SaveChanges();                                                                                        
                        _context.Functions.Add(new Function() { UniqueCode = "CUSTOM_CONFIG", Name = "Tùy chỉnh", ParentId = 1, SortOrder = 100, IsActive = true, Status = Status.Actived, Url = "/admin/customconfig/index", CssClass = "fa-home" });
                        _context.SaveChanges();

                        _context.Functions.Add(new Function() { UniqueCode = "CHECKLIST_C#", Name = "C#", ParentId = 2, SortOrder = 1, IsActive = true, Status = Status.Actived, Url = "/admin/checklist/csharp", CssClass = "fa-table" });
                        _context.SaveChanges();
                        _context.Functions.Add(new Function() { UniqueCode = "CHECKLIST_SQL", Name = "SQL", ParentId = 2, SortOrder = 2, IsActive = true, Status = Status.Actived, Url = "/admin/checklist/sql", CssClass = "fa-table" });
                        _context.SaveChanges();
                        _context.Functions.Add(new Function() { UniqueCode = "CHECKLIST_FRONTEND", Name = "Front-End", ParentId = 2, SortOrder = 3, IsActive = true, Status = Status.Actived, Url = "/admin/checklist/frontend", CssClass = "fa-table" });
                        _context.SaveChanges();
                        _context.Functions.Add(new Function() { UniqueCode = "CHECKLIST_ASPNETMVC", Name = "ASP.NET MVC", ParentId = 2, SortOrder = 4, IsActive = true, Status = Status.Actived, Url = "/admin/checklist/aspnetmvc", CssClass = "fa-tag" });
                        _context.SaveChanges();
                        _context.Functions.Add(new Function() { UniqueCode = "CHECKLIST_ADONET_ENTITYFRAMEWORK", Name = "ADO.NET - Entity Framework", ParentId = 2, SortOrder = 5, IsActive = true, Status = Status.Actived, Url = "/admin/checklist/adonet_ef", CssClass = "fa-table" });
                        _context.SaveChanges();
                        _context.Functions.Add(new Function() { UniqueCode = "CHECKLIST_ASPNETCORE", Name = "ASP.NET Core", ParentId = 2, SortOrder = 6, IsActive = true, Status = Status.Actived, Url = "/admin/checklist/aspnetcore", CssClass = "fa-table" });
                        _context.SaveChanges();

                        _context.Functions.Add(new Function() { UniqueCode = "TOTALLY", Name = "TỔNG HỢP", ParentId = 3, SortOrder = 1, IsActive = true, Status = Status.Actived, Url = "/admin/topic/tonghop", CssClass = "fa-table" });
                        _context.SaveChanges();
                        _context.Functions.Add(new Function() { UniqueCode = "TAGS", Name = "TAGS", ParentId = 3, SortOrder = 2, IsActive = true, Status = Status.Actived, Url = "/admin/tag/index", CssClass = "fa-table" });
                        _context.SaveChanges();
                        _context.Functions.Add(new Function() { UniqueCode = "OTHERS", Name = "Khác", ParentId = 3, SortOrder = 3, IsActive = true, Status = Status.Actived, Url = "/admin/topic/khac", CssClass = "fa-table" });
                        _context.SaveChanges();
                        _context.Functions.Add(new Function() { UniqueCode = "ALGORITMS", Name = "Cấu trúc dữ liệu giải thuật", ParentId = 3, SortOrder = 4, IsActive = true, Status = Status.Actived, Url = "/admin/topic/ctdlgt", CssClass = "fa-tag" });
                        _context.SaveChanges();
                        _context.Functions.Add(new Function() { UniqueCode = "TESTING", Name = "Testing", ParentId = 3, SortOrder = 5, IsActive = true, Status = Status.Actived, Url = "/admin/topic/testing", CssClass = "fa-table" });
                        _context.SaveChanges();

                        _context.Functions.Add(new Function() { UniqueCode = "ASP.NET", Name = "ASP.NET", ParentId = 4, SortOrder = 1, IsActive = true, Status = Status.Actived, Url = "/admin/topic/aspnet", CssClass = "fa-table" });
                        _context.SaveChanges();
                        _context.Functions.Add(new Function() { UniqueCode = "C#", Name = "C#", ParentId = 4, SortOrder = 2, IsActive = true, Status = Status.Actived, Url = "/admin/topic/csharp", CssClass = "fa-tag" });
                        _context.SaveChanges();

                        _context.Functions.Add(new Function() { UniqueCode = "JQUERY", Name = "Jquery", ParentId = 5, SortOrder = 1, IsActive = true, Status = Status.Actived, Url = "/admin/topic/jquery", CssClass = "fa-table" });
                        _context.SaveChanges();
                        _context.Functions.Add(new Function() { UniqueCode = "JAVASCRIPT", Name = "Javascript", ParentId = 5, SortOrder = 2, IsActive = true, Status = Status.Actived, Url = "/admin/topic/javascript", CssClass = "fa-tag" });
                        _context.SaveChanges();
                        _context.Functions.Add(new Function() { UniqueCode = "ANGULAR", Name = "Angular", ParentId = 5, SortOrder = 3, IsActive = true, Status = Status.Actived, Url = "/admin/topic/angular", CssClass = "fa-table" });
                        _context.SaveChanges();
                        _context.Functions.Add(new Function() { UniqueCode = "REACTJS", Name = "ReactJs", ParentId = 5, SortOrder = 4, IsActive = true, Status = Status.Actived, Url = "/admin/topic/reactjs", CssClass = "fa-tag" });
                        _context.SaveChanges();
                        _context.Functions.Add(new Function() { UniqueCode = "VUEJS", Name = "VueJs", ParentId = 5, SortOrder = 5, IsActive = true, Status = Status.Actived, Url = "/admin/topic/vuejs", CssClass = "fa-tag" });
                        _context.SaveChanges();
                        _context.Functions.Add(new Function() { UniqueCode = "HTML_CSS", Name = "HTML/CSS", ParentId = 5, SortOrder = 6, IsActive = true, Status = Status.Actived, Url = "/admin/topic/htmlcss", CssClass = "fa-tag" });
                        _context.SaveChanges();

                        _context.Functions.Add(new Function() { UniqueCode = "THIETKECSDL", Name = "Thiết kế Cơ Sở Dữ Liệu", ParentId = 6, SortOrder = 1, IsActive = true, Status = Status.Actived, Url = "/admin/topic/thietkecsdl", CssClass = "fa-table" });
                        _context.SaveChanges();
                        _context.Functions.Add(new Function() { UniqueCode = "SQL", Name = "SQL", ParentId = 6, SortOrder = 2, IsActive = true, Status = Status.Actived, Url = "/admin/topic/sql", CssClass = "fa-tag" });
                        _context.SaveChanges();
                        _context.Functions.Add(new Function() { UniqueCode = "MSSQLSERVER", Name = "MS SQL Server", ParentId = 6, SortOrder = 3, IsActive = true, Status = Status.Actived, Url = "/admin/topic/mssqlserver", CssClass = "fa-tag" });
                        _context.SaveChanges();
                        _context.Functions.Add(new Function() { UniqueCode = "MYSQL", Name = "MYSQL", ParentId = 6, SortOrder = 4, IsActive = true, Status = Status.Actived, Url = "/admin/topic/mysql", CssClass = "fa-table" });
                        _context.SaveChanges();
                        _context.Functions.Add(new Function() { UniqueCode = "MONGODB", Name = "MongoDb", ParentId = 6, SortOrder = 5, IsActive = true, Status = Status.Actived, Url = "/admin/topic/mongodb", CssClass = "fa-tag" });
                        _context.SaveChanges();
                        _context.Functions.Add(new Function() { UniqueCode = "ORACLE", Name = "Oracle", ParentId = 6, SortOrder = 6, IsActive = true, Status = Status.Actived, Url = "/admin/topic/oracle", CssClass = "fa-table" });
                        _context.SaveChanges();

                        _context.Functions.Add(new Function() { UniqueCode = "LISTENING", Name = "Listening", ParentId = 7, SortOrder = 1, IsActive = true, Status = Status.Actived, Url = "/admin/topic/listening", CssClass = "fa-table" });
                        _context.SaveChanges();
                        _context.Functions.Add(new Function() { UniqueCode = "SPEAKING", Name = "Speaking", ParentId = 7, SortOrder = 2, IsActive = true, Status = Status.Actived, Url = "/admin/topic/speaking", CssClass = "fa-tag" });
                        _context.SaveChanges();
                        _context.Functions.Add(new Function() { UniqueCode = "READING", Name = "Reading", ParentId = 7, SortOrder = 3, IsActive = true, Status = Status.Actived, Url = "/admin/topic/reading", CssClass = "fa-table" });
                        _context.SaveChanges();
                        _context.Functions.Add(new Function() { UniqueCode = "WRITING", Name = "Writing", ParentId = 7, SortOrder = 4, IsActive = true, Status = Status.Actived, Url = "/admin/topic/writing", CssClass = "fa-table" });
                        _context.SaveChanges();

                    }
                    catch (Exception ex)
                    {

                        throw ex; 
                    }
                    
                }
                #endregion

                #region Permission

                if (!_context.Permissions.Any())
                {
                    _context.Permissions.AddRange(new List<Permission>()
                    {
                    #region rico
                    new Permission() { RoleId = adminRoleId, FunctionId = 1, CanCreate = true, CanUpdate = true, CanRead = true, CanDelete = true },
                    new Permission() { RoleId = adminRoleId, FunctionId = 2, CanCreate = true, CanUpdate = true, CanRead = true, CanDelete = true },
                    new Permission() { RoleId = adminRoleId, FunctionId = 3, CanCreate = true, CanUpdate = true, CanRead = true, CanDelete = true },
                    new Permission() { RoleId = adminRoleId, FunctionId = 4, CanCreate = true, CanUpdate = true, CanRead = true, CanDelete = true },
                    new Permission() { RoleId = adminRoleId, FunctionId = 5, CanCreate = true, CanUpdate = true, CanRead = true, CanDelete = true },
                    new Permission() { RoleId = adminRoleId, FunctionId = 6, CanCreate = true, CanUpdate = true, CanRead = true, CanDelete = true },
                    new Permission() { RoleId = adminRoleId, FunctionId = 7, CanCreate = true, CanUpdate = true, CanRead = true, CanDelete = true },

                    new Permission() { RoleId = adminRoleId, FunctionId = 8, CanCreate = true, CanUpdate = true, CanRead = true, CanDelete = true },
                    new Permission() { RoleId = adminRoleId, FunctionId = 9, CanCreate = true, CanUpdate = true, CanRead = true, CanDelete = true },
                    new Permission() { RoleId = adminRoleId, FunctionId = 10, CanCreate = true, CanUpdate = true, CanRead = true, CanDelete = true },
                    new Permission() { RoleId = adminRoleId, FunctionId = 11, CanCreate = true, CanUpdate = true, CanRead = true, CanDelete = true },
                    new Permission() { RoleId = adminRoleId, FunctionId = 12, CanCreate = true, CanUpdate = true, CanRead = true, CanDelete = true },
                    new Permission() { RoleId = adminRoleId, FunctionId = 13, CanCreate = true, CanUpdate = true, CanRead = true, CanDelete = true },

                    new Permission() { RoleId = adminRoleId, FunctionId = 14, CanCreate = true, CanUpdate = true, CanRead = true, CanDelete = true },
                    new Permission() { RoleId = adminRoleId, FunctionId = 15, CanCreate = true, CanUpdate = true, CanRead = true, CanDelete = true },
                    new Permission() { RoleId = adminRoleId, FunctionId = 16, CanCreate = true, CanUpdate = true, CanRead = true, CanDelete = true },
                    new Permission() { RoleId = adminRoleId, FunctionId = 17, CanCreate = true, CanUpdate = true, CanRead = true, CanDelete = true },
                    new Permission() { RoleId = adminRoleId, FunctionId = 18, CanCreate = true, CanUpdate = true, CanRead = true, CanDelete = true },
                    new Permission() { RoleId = adminRoleId, FunctionId = 19, CanCreate = true, CanUpdate = true, CanRead = true, CanDelete = true },

                    new Permission() { RoleId = adminRoleId, FunctionId = 20, CanCreate = true, CanUpdate = true, CanRead = true, CanDelete = true },
                    new Permission() { RoleId = adminRoleId, FunctionId = 21, CanCreate = true, CanUpdate = true, CanRead = true, CanDelete = true },
                    new Permission() { RoleId = adminRoleId, FunctionId = 22, CanCreate = true, CanUpdate = true, CanRead = true, CanDelete = true },
                    new Permission() { RoleId = adminRoleId, FunctionId = 23, CanCreate = true, CanUpdate = true, CanRead = true, CanDelete = true },
                    new Permission() { RoleId = adminRoleId, FunctionId = 24, CanCreate = true, CanUpdate = true, CanRead = true, CanDelete = true },

                    new Permission() { RoleId = adminRoleId, FunctionId = 25, CanCreate = true, CanUpdate = true, CanRead = true, CanDelete = true },
                    new Permission() { RoleId = adminRoleId, FunctionId = 26, CanCreate = true, CanUpdate = true, CanRead = true, CanDelete = true },

                    new Permission() { RoleId = adminRoleId, FunctionId = 27, CanCreate = true, CanUpdate = true, CanRead = true, CanDelete = true },
                    new Permission() { RoleId = adminRoleId, FunctionId = 28, CanCreate = true, CanUpdate = true, CanRead = true, CanDelete = true },
                    new Permission() { RoleId = adminRoleId, FunctionId = 29, CanCreate = true, CanUpdate = true, CanRead = true, CanDelete = true },
                    new Permission() { RoleId = adminRoleId, FunctionId = 30, CanCreate = true, CanUpdate = true, CanRead = true, CanDelete = true },
                    new Permission() { RoleId = adminRoleId, FunctionId = 31, CanCreate = true, CanUpdate = true, CanRead = true, CanDelete = true },
                    new Permission() { RoleId = adminRoleId, FunctionId = 32, CanCreate = true, CanUpdate = true, CanRead = true, CanDelete = true },

                    new Permission() { RoleId = adminRoleId, FunctionId = 33, CanCreate = true, CanUpdate = true, CanRead = true, CanDelete = true },
                    new Permission() { RoleId = adminRoleId, FunctionId = 34, CanCreate = true, CanUpdate = true, CanRead = true, CanDelete = true },
                    new Permission() { RoleId = adminRoleId, FunctionId = 35, CanCreate = true, CanUpdate = true, CanRead = true, CanDelete = true },
                    new Permission() { RoleId = adminRoleId, FunctionId = 36, CanCreate = true, CanUpdate = true, CanRead = true, CanDelete = true },
                    new Permission() { RoleId = adminRoleId, FunctionId = 37, CanCreate = true, CanUpdate = true, CanRead = true, CanDelete = true },
                    new Permission() { RoleId = adminRoleId, FunctionId = 38, CanCreate = true, CanUpdate = true, CanRead = true, CanDelete = true },

                    new Permission() { RoleId = adminRoleId, FunctionId = 39, CanCreate = true, CanUpdate = true, CanRead = true, CanDelete = true },
                    new Permission() { RoleId = adminRoleId, FunctionId = 40, CanCreate = true, CanUpdate = true, CanRead = true, CanDelete = true },
                    new Permission() { RoleId = adminRoleId, FunctionId = 41, CanCreate = true, CanUpdate = true, CanRead = true, CanDelete = true },
                    new Permission() { RoleId = adminRoleId, FunctionId = 42, CanCreate = true, CanUpdate = true, CanRead = true, CanDelete = true },                    
                    #endregion   
                    
                    #region rico bao mat
                    //new Permission() { RoleId = adminRoleId, FunctionId = 1, CanCreate = true, CanUpdate = true, CanRead = true, CanDelete = true },
                    //new Permission() { RoleId = adminRoleId, FunctionId = 2, CanCreate = true, CanUpdate = true, CanRead = true, CanDelete = true },
                    //new Permission() { RoleId = adminRoleId, FunctionId = 3, CanCreate = true, CanUpdate = true, CanRead = true, CanDelete = true },                    
                    #endregion              
                    });
                    _context.SaveChanges();
                }
                    #endregion         

                #region Footer
            if (_context.Footers.Count(x => x.Id == CommonConstants.DefaultFooterId) == 0)
                {
                    string content = "Footer";
                    _context.Footers.Add(new Footer()
                    {
                        Id = CommonConstants.DefaultFooterId,
                        Content = content
                    });
                    _context.SaveChanges();
                }
                #endregion

                #region Contact
                //if (_context.ContactDetails.Count(x => x.Id == CommonConstants.DefaultFooterId) == 0)
                 if (!_context.ContactDetails.Any())
                    {
                        _context.ContactDetails.Add(new ContactDetail()
                        {
                            //Id = CommonConstants.DefaultFooterId,
                            Name = "Lương Tuấn Anh",
                            Status = Status.Actived,
                            Address = "",
                            Email = "",
                            Phone = "0902 281 086",
                            Website = "luonganh.com",
                            Lng = 105.8017848,
                            Lat = 20.9888323,
                        });
                    _context.SaveChanges();
                }
                #endregion

                #region Advertistment
                //if (!_context.AdvertistmentPages.Any())
                //{
                //    List<AdvertistmentPage> pages = new List<AdvertistmentPage>()
                //    {
                //        new AdvertistmentPage() { Id = Guid.NewGuid(), UniqueCode="home", Name="Trang chủ"},
                //        new AdvertistmentPage() { Id =Guid.NewGuid(), UniqueCode ="product-cate", Name="Danh mục sản phẩm" },
                //        new AdvertistmentPage() { Id = Guid.NewGuid(), UniqueCode ="product-detail", Name="Chi tiết sản phẩm"},
                //    };
                //    _context.AdvertistmentPages.AddRange(pages);
                //}
                #endregion

                #region SystemConfig
                if (!_context.SystemConfigs.Any(x => x.UniqueCode == "HomeMetaTitle"))
                {
                    _context.SystemConfigs.Add(new Setting()
                    {
                        Id = Guid.NewGuid(),
                        UniqueCode = "HomeMetaTitle",
                        Name = "HomeMetaTitle",
                        TextValue = "Lương Tuấn Anh",
                        Status = Status.Actived
                    });
                    _context.SaveChanges();
                }
                if (!_context.SystemConfigs.Any(x => x.UniqueCode == "HomeMetaKeywords"))
                {
                    _context.SystemConfigs.Add(new Setting()
                    {
                        Id = Guid.NewGuid(),
                        UniqueCode = "HomeMetaKeywords",
                        Name = "HomeMetaKeywords",
                        TextValue = "",
                        Status = Status.Actived
                    });
                    _context.SaveChanges();
                }
                if (!_context.SystemConfigs.Any(x => x.UniqueCode == "HomeMetaDescription"))
                {
                    _context.SystemConfigs.Add(new Setting()
                    {
                        Id = Guid.NewGuid(),
                        UniqueCode = "HomeMetaDescription",
                        Name = "HomeMetaDescription",
                        TextValue = "Đây là trang web cá nhân của Lương Tuấn Anh",
                        Status = Status.Actived
                    });
                    _context.SaveChanges();
                }
                await _context.SaveChangesAsync();
                #endregion
               
                #region PostCategory                

                //    if (!_context.PostCategories.Any())
                //{                                        
                //    _context.PostCategories.Add(new PostCategory() { ParentId = null, Code = GenerateCode(), Url = "song-khoe", Name = "Sống khỏe", Description = "Sống khỏe", HotFlag = 0, HotOrder = 0, HomeFlag = 0, HomeOrder = 0, Status = Status.Actived, SortOrder = 1, MetaTitle = "Sống khỏe - Sức khỏe là vàng | Songkhoe.co", MetaDescription = "Sống khỏe", MetaKeywords = "Sống khỏe", DateCreated = DateTime.Now, DateModified = DateTime.Now }); _context.SaveChanges();
                //    _context.PostCategories.Add(new PostCategory() { ParentId = null, Code = GenerateCode(), Url = "#", Name = "Thể dục thể thao", Description = "Thể dục thể thao", HotFlag = 0, HotOrder = 0, HomeFlag = 0, HomeOrder = 0, Status = Status.Actived, SortOrder = 2, MetaTitle = "Thể dục thể thao", MetaDescription = "Thể dục thể thao", MetaKeywords = "Thể dục thể thao", DateCreated = DateTime.Now, DateModified = DateTime.Now }); _context.SaveChanges();
                //    _context.PostCategories.Add(new PostCategory() { ParentId = null, Code = GenerateCode(), Url = "#", Name = "English", Description = "English", HotFlag = 0, HotOrder = 0, HomeFlag = 0, HomeOrder = 0, Status = Status.Actived, SortOrder = 3, MetaTitle = "English", MetaDescription = "English", MetaKeywords = "English", DateCreated = DateTime.Now, DateModified = DateTime.Now }); _context.SaveChanges();
                //    _context.PostCategories.Add(new PostCategory() { ParentId = null, Code = GenerateCode(), Url = "#", Name = "Lập trình", Description = "Lập trình", HotFlag = 0, HotOrder = 0, HomeFlag = 0, HomeOrder = 0, Status = Status.Actived, SortOrder = 4, MetaTitle = "Lập trình", MetaDescription = "Lập trình", MetaKeywords = "Lập trình", DateCreated = DateTime.Now, DateModified = DateTime.Now }); _context.SaveChanges();
                //    _context.PostCategories.Add(new PostCategory() { ParentId = null, Code = GenerateCode(), Url = "#", Name = "Học tập", Description = "Học tập", HotFlag = 0, HotOrder = 0, HomeFlag = 0, HomeOrder = 0, Status = Status.Actived, SortOrder = 5, MetaTitle = "Học tập", MetaDescription = "Học tập", MetaKeywords = "học tập", DateCreated = DateTime.Now, DateModified = DateTime.Now }); _context.SaveChanges();
                //    _context.PostCategories.Add(new PostCategory() { ParentId = null, Code = GenerateCode(), Url = "#", Name = "Cuộc sống", Description = "Cuộc sống", HotFlag = 0, HotOrder = 0, HomeFlag = 0, HomeOrder = 0, Status = Status.Actived, SortOrder = 6, MetaTitle = "Cuộc sống", MetaDescription = "Cuộc sống", MetaKeywords = "cuộc sống", DateCreated = DateTime.Now, DateModified = DateTime.Now }); _context.SaveChanges();
                //    _context.PostCategories.Add(new PostCategory() { ParentId = null, Code = GenerateCode(), Url = "#", Name = "Chữa bệnh", Description = "Chữa bệnh", HotFlag = 0, HotOrder = 0, HomeFlag = 0, HomeOrder = 0, Status = Status.Actived, SortOrder = 7, MetaTitle = "Chữa bệnh", MetaDescription = "Chữa bệnh", MetaKeywords = "chữa bệnh", DateCreated = DateTime.Now, DateModified = DateTime.Now }); _context.SaveChanges();
                //    _context.PostCategories.Add(new PostCategory() { ParentId = null, Code = GenerateCode(), Url = "#", Name = "Dữ liệu", Description = "Dữ liệu", HotFlag = 0, HotOrder = 0, HomeFlag = 0, HomeOrder = 0, Status = Status.Actived, SortOrder = 8, MetaTitle = "Dữ liệu", MetaDescription = "Dữ liệu", MetaKeywords = "dữ liệu", DateCreated = DateTime.Now, DateModified = DateTime.Now }); _context.SaveChanges();
                                                           
                //    _context.PostCategories.Add(new PostCategory() { ParentId = 2, Code = GenerateCode(), Url = "the-duc", Name = "Thể dục", Description = "Thể dục", HotFlag = 0, HotOrder = 0, HomeFlag = 0, HomeOrder = 0, Status = Status.Actived, SortOrder = 1, MetaTitle = "Thể dục - Thể dục thể thao | Songkhoe.co", MetaDescription = "Thể dục", MetaKeywords = "thể dục", DateCreated = DateTime.Now, DateModified = DateTime.Now }); _context.SaveChanges();
                //    _context.PostCategories.Add(new PostCategory() { ParentId = 2, Code = GenerateCode(), Url = "gym-xuong-khop", Name = "Gym xương khớp", Description = "Gym cho người bị bệnh về xương khớp", HotFlag = 0, HotOrder = 0, HomeFlag = 0, HomeOrder = 0, Status = Status.Actived, SortOrder = 2, MetaTitle = "Gym cho người bị bệnh về xương khớp - Thể dục thể thao | Songkhoe.co", MetaDescription = "Gym cho người bị bệnh về xương khớp, cột sống như: thoái hóa xương khớp, thoát vị đĩa đệm...", MetaKeywords = "gym, bệnh xương khớp, thoái hóa xương khớp, thoát vị đĩa đệm", DateCreated = DateTime.Now, DateModified = DateTime.Now }); _context.SaveChanges();
                //    _context.PostCategories.Add(new PostCategory() { ParentId = 2, Code = GenerateCode(), Url = "boi", Name = "Bơi", Description = "Bơi", HotFlag = 0, HotOrder = 0, HomeFlag = 0, HomeOrder = 0, Status = Status.Actived, SortOrder = 3, MetaTitle = "Bơi - Thể dục thể thao | Songkhoe.co", MetaDescription = "Bơi", MetaKeywords = "bơi", DateCreated = DateTime.Now, DateModified = DateTime.Now }); _context.SaveChanges();
                //    _context.PostCategories.Add(new PostCategory() { ParentId = 2, Code = GenerateCode(), Url = "khi-cong", Name = "Khí công", Description = "Khí công", HotFlag = 0, HotOrder = 0, HomeFlag = 0, HomeOrder = 0, Status = Status.Actived, SortOrder = 4, MetaTitle = "Khí công - Thể dục thể thao | Songkhoe.co", MetaDescription = "Khí công", MetaKeywords = "khí công", DateCreated = DateTime.Now, DateModified = DateTime.Now }); _context.SaveChanges();
                //    _context.PostCategories.Add(new PostCategory() { ParentId = 2, Code = GenerateCode(), Url = "yoga", Name = "Yoga", Description = "Yoga", HotFlag = 0, HotOrder = 0, HomeFlag = 0, HomeOrder = 0, Status = Status.Actived, SortOrder = 5, MetaTitle = "Yoga - Thể dục thể thao | Songkhoe.co", MetaDescription = "Yoga", MetaKeywords = "yoga", DateCreated = DateTime.Now, DateModified = DateTime.Now }); _context.SaveChanges();
              
                //    _context.PostCategories.Add(new PostCategory() { ParentId = 3, Code = GenerateCode(), Url = "listening", Name = "Listening", Description = "Listening", HotFlag = 0, HotOrder = 0, HomeFlag = 0, HomeOrder = 0, Status = Status.Actived, SortOrder = 1, MetaTitle = "Listening - English | Songkhoe.co", MetaDescription = "Listening", MetaKeywords = "listening", DateCreated = DateTime.Now, DateModified = DateTime.Now }); _context.SaveChanges();
                //    _context.PostCategories.Add(new PostCategory() { ParentId = 3, Code = GenerateCode(), Url = "speaking", Name = "Speaking", Description = "Speaking", HotFlag = 0, HotOrder = 0, HomeFlag = 0, HomeOrder = 0, Status = Status.Actived, SortOrder = 2, MetaTitle = "Speaking - English | Songkhoe.co", MetaDescription = "Speaking", MetaKeywords = "speaking", DateCreated = DateTime.Now, DateModified = DateTime.Now }); _context.SaveChanges();
                //    _context.PostCategories.Add(new PostCategory() { ParentId = 3, Code = GenerateCode(), Url = "reading", Name = "Reading", Description = "Reading", HotFlag = 0, HotOrder = 0, HomeFlag = 0, HomeOrder = 0, Status = Status.Actived, SortOrder = 3, MetaTitle = "Reading - English | Songkhoe.co", MetaDescription = "Reading", MetaKeywords = "reading", DateCreated = DateTime.Now, DateModified = DateTime.Now }); _context.SaveChanges();
                //    _context.PostCategories.Add(new PostCategory() { ParentId = 3, Code = GenerateCode(), Url = "writing", Name = "Writing", Description = "Writing", HotFlag = 0, HotOrder = 0, HomeFlag = 0, HomeOrder = 0, Status = Status.Actived, SortOrder = 4, MetaTitle = "Writing - English | Songkhoe.co", MetaDescription = "Writing", MetaKeywords = "writing", DateCreated = DateTime.Now, DateModified = DateTime.Now }); _context.SaveChanges();

                //    _context.PostCategories.Add(new PostCategory() { ParentId = 4, Code = GenerateCode(), Url = "tong-hop", Name = "Tổng hợp", Description = "Tổng hợp - Lập trình", HotFlag = 0, HotOrder = 0, HomeFlag = 0, HomeOrder = 0, Status = Status.Actived, SortOrder = 1, MetaTitle = "Tổng hợp - Lập trình | Songkhoe.co", MetaDescription = "Tổng hợp - Lập trình", MetaKeywords = "tổng hợp, lập trình", DateCreated = DateTime.Now, DateModified = DateTime.Now }); _context.SaveChanges();
                //    _context.PostCategories.Add(new PostCategory() { ParentId = 4, Code = GenerateCode(), Url = "cau-truc-du-lieu-va-giai-thuat", Name = "Cấu trúc dữ liệu và giải thuật", Description = "Cấu trúc dữ liệu và giải thuật", HotFlag = 0, HotOrder = 0, HomeFlag = 0, HomeOrder = 0, Status = Status.Actived, SortOrder = 2, MetaTitle = "Cấu trúc dữ liệu và giải thuật - Lập trình | Songkhoe.co", MetaDescription = "Cấu trúc dữ liệu và giải thuật", MetaKeywords = "Cấu trúc dữ liệu và giải thuật", DateCreated = DateTime.Now, DateModified = DateTime.Now }); _context.SaveChanges();
                //    _context.PostCategories.Add(new PostCategory() { ParentId = 4, Code = GenerateCode(), Url = "c-sharp", Name = "C#", Description = "C#", HotFlag = 0, HotOrder = 0, HomeFlag = 0, HomeOrder = 0, Status = Status.Actived, SortOrder = 3, MetaTitle = "C# - Lập trình | Songkhoe.co", MetaDescription = "C#", MetaKeywords = "C#", DateCreated = DateTime.Now, DateModified = DateTime.Now }); _context.SaveChanges();
                //    _context.PostCategories.Add(new PostCategory() { ParentId = 4, Code = GenerateCode(), Url = "asp-net-mvc", Name = "ASP.NET MVC", Description = "ASP.NET MVC", HotFlag = 0, HotOrder = 0, HomeFlag = 0, HomeOrder = 0, Status = Status.Actived, SortOrder = 4, MetaTitle = "ASP.NET MVC - Lập trình | Songkhoe.co", MetaDescription = "ASP.NET MVC", MetaKeywords = "ASP.NET MVC", DateCreated = DateTime.Now, DateModified = DateTime.Now }); _context.SaveChanges();
                //    _context.PostCategories.Add(new PostCategory() { ParentId = 4, Code = GenerateCode(), Url = "asp-net-core", Name = "ASP.NET CORE", Description = "ASP.NET CORE", HotFlag = 0, HotOrder = 0, HomeFlag = 0, HomeOrder = 0, Status = Status.Actived, SortOrder = 5, MetaTitle = "ASP.NET CORE - Lập trình | Songkhoe.co", MetaDescription = "ASP.NET CORE", MetaKeywords = "ASP.NET CORE", DateCreated = DateTime.Now, DateModified = DateTime.Now }); _context.SaveChanges();
                //    _context.PostCategories.Add(new PostCategory() { ParentId = 4, Code = GenerateCode(), Url = "html-css", Name = "HTML-CSS", Description = "HTML-CSS", HotFlag = 0, HotOrder = 0, HomeFlag = 0, HomeOrder = 0, Status = Status.Actived, SortOrder = 6, MetaTitle = "HTML-CSS - Lập trình | Songkhoe.co", MetaDescription = "HTML-CSS", MetaKeywords = "HTML-CSS", DateCreated = DateTime.Now, DateModified = DateTime.Now }); _context.SaveChanges();
                //    _context.PostCategories.Add(new PostCategory() { ParentId = 4, Code = GenerateCode(), Url = "javascript", Name = "Javascript", Description = "Javascript", HotFlag = 0, HotOrder = 0, HomeFlag = 0, HomeOrder = 0, Status = Status.Actived, SortOrder = 7, MetaTitle = "Javascript - Lập trình | Songkhoe.co", MetaDescription = "Javascript", MetaKeywords = "Javascript", DateCreated = DateTime.Now, DateModified = DateTime.Now }); _context.SaveChanges();
                //    _context.PostCategories.Add(new PostCategory() { ParentId = 4, Code = GenerateCode(), Url = "jquery", Name = "Jquery", Description = "Jquery", HotFlag = 0, HotOrder = 0, HomeFlag = 0, HomeOrder = 0, Status = Status.Actived, SortOrder = 8, MetaTitle = "Jquery - Lập trình | Songkhoe.co", MetaDescription = "Jquery", MetaKeywords = "Jquery", DateCreated = DateTime.Now, DateModified = DateTime.Now }); _context.SaveChanges();
                //    _context.PostCategories.Add(new PostCategory() { ParentId = 4, Code = GenerateCode(), Url = "reactjs", Name = "Reactjs", Description = "Reactjs", HotFlag = 0, HotOrder = 0, HomeFlag = 0, HomeOrder = 0, Status = Status.Actived, SortOrder = 9, MetaTitle = "Reactjs - Lập trình | Songkhoe.co", MetaDescription = "Reactjs", MetaKeywords = "Reactjs", DateCreated = DateTime.Now, DateModified = DateTime.Now }); _context.SaveChanges();
                //    _context.PostCategories.Add(new PostCategory() { ParentId = 4, Code = GenerateCode(), Url = "ms-sql-server", Name = "MS SQL Server", Description = "MS SQL Server", HotFlag = 0, HotOrder = 0, HomeFlag = 0, HomeOrder = 0, Status = Status.Actived, SortOrder = 10, MetaTitle = "MS SQL Server - Lập trình | Songkhoe.co", MetaDescription = "MS SQL Server", MetaKeywords = "MS SQL Server", DateCreated = DateTime.Now, DateModified = DateTime.Now }); _context.SaveChanges();
                //    _context.PostCategories.Add(new PostCategory() { ParentId = 4, Code = GenerateCode(), Url = "mongodb", Name = "MongoDb", Description = "MongoDb", HotFlag = 0, HotOrder = 0, HomeFlag = 0, HomeOrder = 0, Status = Status.Actived, SortOrder = 11, MetaTitle = "MongoDb - Lập trình | Songkhoe.co", MetaDescription = "MongoDb", MetaKeywords = "MongoDb", DateCreated = DateTime.Now, DateModified = DateTime.Now }); _context.SaveChanges();
                //    _context.PostCategories.Add(new PostCategory() { ParentId = 4, Code = GenerateCode(), Url = "testing", Name = "Testing", Description = "Testing", HotFlag = 0, HotOrder = 0, HomeFlag = 0, HomeOrder = 0, Status = Status.Actived, SortOrder = 12, MetaTitle = "Testing - Lập trình | Songkhoe.co", MetaDescription = "Testing", MetaKeywords = "Testing", DateCreated = DateTime.Now, DateModified = DateTime.Now }); _context.SaveChanges();

                //    _context.PostCategories.Add(new PostCategory() { ParentId = 5, Code = GenerateCode(), Url = "tu-duong", Name = "Tu dưỡng", Description = "Tu dưỡng", HotFlag = 0, HotOrder = 0, HomeFlag = 0, HomeOrder = 0, Status = Status.Actived, SortOrder = 1, MetaTitle = "Tu dưỡng - Học tập | Songkhoe.co", MetaDescription = "Tu dưỡng cả đời người trong mọi lĩnh vực của cuộc sống, tất cả đều là những người thầy...", MetaKeywords = "tu dưỡng", DateCreated = DateTime.Now, DateModified = DateTime.Now }); _context.SaveChanges();

                //    _context.PostCategories.Add(new PostCategory() { ParentId = 6, Code = GenerateCode(), Url = "gia-dinh", Name = "Gia đình", Description = "Gia đình, phụ nữ, trẻ em, người già", HotFlag = 0, HotOrder = 0, HomeFlag = 0, HomeOrder = 0, Status = Status.Actived, SortOrder = 1, MetaTitle = "Gia đình, phụ nữ, trẻ em, người già - Cuộc sống | Songkhoe.co", MetaDescription = "Gia đình, phụ nữ, trẻ em, người già", MetaKeywords = "gia đình, phụ nữ, trẻ em, người già", DateCreated = DateTime.Now, DateModified = DateTime.Now }); _context.SaveChanges();
                //    _context.PostCategories.Add(new PostCategory() { ParentId = 6, Code = GenerateCode(), Url = "xa-hoi", Name = "Xã hội", Description = "Xã hội", HotFlag = 0, HotOrder = 0, HomeFlag = 0, HomeOrder = 0, Status = Status.Actived, SortOrder = 2, MetaTitle = "Xã hội - Cuộc sống | Songkhoe.co", MetaDescription = "Xã hội", MetaKeywords = "xã hội", DateCreated = DateTime.Now, DateModified = DateTime.Now }); _context.SaveChanges();
                //    _context.PostCategories.Add(new PostCategory() { ParentId = 6, Code = GenerateCode(), Url = "tinh-yeu", Name = "Tình yêu", Description = "Tình yêu", HotFlag = 0, HotOrder = 0, HomeFlag = 0, HomeOrder = 0, Status = Status.Actived, SortOrder = 3, MetaTitle = "Tình yêu - Cuộc sống | Songkhoe.co", MetaDescription = "Mọi thứ về tình yêu trong cuộc sống", MetaKeywords = "tình yêu", DateCreated = DateTime.Now, DateModified = DateTime.Now }); _context.SaveChanges();
                //    _context.PostCategories.Add(new PostCategory() { ParentId = 6, Code = GenerateCode(), Url = "lich-su-van-hoa", Name = "Lịch sử văn hóa", Description = "Lịch sử văn hóa, chính trị, quân sự của Việt Nam, thế giới", HotFlag = 0, HotOrder = 0, HomeFlag = 0, HomeOrder = 0, Status = Status.Actived, SortOrder = 4, MetaTitle = "Lịch sử văn hóa - Cuộc sống | Songkhoe.co", MetaDescription = "Kiến thức, thông tin về lịch sử văn hóa, chính trị, quân sự của Việt Nam, thế giới", MetaKeywords = "lịch sử, văn hóa, chính trị, quân sự, Việt Nam, thế giới", DateCreated = DateTime.Now, DateModified = DateTime.Now }); _context.SaveChanges();
                //    _context.PostCategories.Add(new PostCategory() { ParentId = 6, Code = GenerateCode(), Url = "the-gioi", Name = "Thế giới", Description = "Thế giới", HotFlag = 0, HotOrder = 0, HomeFlag = 0, HomeOrder = 0, Status = Status.Actived, SortOrder = 5, MetaTitle = "Thế giới - Cuộc sống | Songkhoe.co", MetaDescription = "Thế giới", MetaKeywords = "thế giới", DateCreated = DateTime.Now, DateModified = DateTime.Now }); _context.SaveChanges();

                //    _context.PostCategories.Add(new PostCategory() { ParentId = 7, Code = GenerateCode(), Url = "xuong-khop", Name = "Xương khớp", Description = "Xương khớp", HotFlag = 0, HotOrder = 0, HomeFlag = 0, HomeOrder = 0, Status = Status.Actived, SortOrder = 1, MetaTitle = "Xương khớp - Chữa bệnh | Songkhoe.co", MetaDescription = "Các bệnh xương khớp", MetaKeywords = "thoái hóa xương khớp, thoát vị đĩa đệm", DateCreated = DateTime.Now, DateModified = DateTime.Now }); _context.SaveChanges();

                //    _context.PostCategories.Add(new PostCategory() { ParentId = 8, Code = GenerateCode(), Url = "ghi-chu", Name = "Ghi chú", Description = "Ghi chú", HotFlag = 0, HotOrder = 0, HomeFlag = 0, HomeOrder = 0, Status = Status.Actived, SortOrder = 1, MetaTitle = "Ghi chú | Songkhoe.co", MetaDescription = "Ghi chú", MetaKeywords = "ghi chú", DateCreated = DateTime.Now, DateModified = DateTime.Now }); _context.SaveChanges();
                //    _context.PostCategories.Add(new PostCategory() { ParentId = 8, Code = GenerateCode(), Url = "luu-tru", Name = "Lưu trữ", Description = "Ghi chú", HotFlag = 0, HotOrder = 0, HomeFlag = 0, HomeOrder = 0, Status = Status.Actived, SortOrder = 1, MetaTitle = "Lưu trữ | Songkhoe.co", MetaDescription = "Lưu trữ", MetaKeywords = "lưu trữ", DateCreated = DateTime.Now, DateModified = DateTime.Now }); _context.SaveChanges();
                //}

                //var postCategories = new List<PostCategory>()
                //{
                //    new PostCategory(){ParentId=null, Code = GenerateCode(), Url="suc-khoe", Name="Sức khỏe", Description="Sức khỏe", HotFlag=0, HotOrder=0, HomeFlag=0, HomeOrder=0, Status = Status.Actived, SortOrder=1, MetaTitle="Sức khỏe", MetaDescription="Kiến thức về sức khỏe", MetaKeywords="kiến thức, sức khỏe", DateCreated = DateTime.Now, DateModified = DateTime.Now},
                //    new PostCategory(){ParentId=null, Code = GenerateCode(), Url="an-uong", Name="Ăn uống", Description="Ăn uống", HotFlag=0, HotOrder=0, HomeFlag=0, HomeOrder=0, Status = Status.Actived, SortOrder=2, MetaTitle="Ăn uống", MetaDescription="Kiến thức, thông tin về ăn uống, dinh dưỡng", MetaKeywords="ăn uống, dinh dưỡng", DateCreated = DateTime.Now, DateModified = DateTime.Now},                    
                //};
                //_context.PostCategories.AddRange(postCategories);
                //_context.SaveChanges();

                #endregion

                #region Post
                //if (!_context.Posts.Any())
                //{                   
                //    List<Post> posts = new List<Post>()
                //    {
                //new Post(){Id=Guid.NewGuid(), CategoryId=subCategory1Of1Id, Code = null, Name="Post 1", Url="post-1", Tags="post 1, tag post 1", MetaTitle="Meta title post 1", MetaDescription="Meta Description post 1", MetaKeywords="post 1, post a", Status=Status.Actived, SortOrder=1, Content="Content Post 1", Description="Description Post 1", HomeFlag=0, HomeOrder=0, HotFlag=0, HotOrder=0, Viewed=0, DateCreated=DateTime.Now, DateModified=DateTime.Now},
                //new Post(){Id=Guid.NewGuid(), CategoryId=subCategory1Of1Id, Code = null, Name="Post 2", Url="post-2", Tags="post 2, tag post 2", MetaTitle="Meta title post 2", MetaDescription="Meta Description post 2", MetaKeywords="post 2, post b", Status=Status.Actived, SortOrder=2, Content="Content Post 2", Description="Description Post 2", HomeFlag=0, HomeOrder=0, HotFlag=0, HotOrder=0, Viewed=0, DateCreated=DateTime.Now, DateModified=DateTime.Now},
                //new Post(){Id=Guid.NewGuid(), CategoryId=subCategory2Of1Id, Code = null, Name="Post 3", Url="post-3", Tags="post 3, tag post 3", MetaTitle="Meta title post 3", MetaDescription="Meta Description post 3", MetaKeywords="post 3, post c", Status=Status.Actived, SortOrder=1, Content="Content Post 3", Description="Description Post 3", HomeFlag=0, HomeOrder=0, HotFlag=0, HotOrder=0, Viewed=0, DateCreated=DateTime.Now, DateModified=DateTime.Now},
                //new Post(){Id=Guid.NewGuid(), CategoryId=subCategory2Of1Id, Code = null, Name="Post 4", Url="post-4", Tags="post 4, tag post 4", MetaTitle="Meta title post 4", MetaDescription="Meta Description post 4", MetaKeywords="post 4, post d", Status=Status.Actived, SortOrder=2, Content="Content Post 4", Description="Description Post 4", HomeFlag=0, HomeOrder=0, HotFlag=0, HotOrder=0, Viewed=0, DateCreated=DateTime.Now, DateModified=DateTime.Now},
                //new Post(){Id=Guid.NewGuid(), CategoryId=subCategory1Of1Id, Code = null, Name="Post 1", Url="post-1", Tags="post 1, tag post 1", MetaTitle="Meta title post 1", MetaDescription="Meta Description post 1", MetaKeywords="post 1, post a", Status=Status.Actived, SortOrder=3, Content="Content Post 1", Description="Description Post 1", HomeFlag=0, HomeOrder=0, HotFlag=0, HotOrder=0, Viewed=0, DateCreated=DateTime.Now, DateModified=DateTime.Now},
                //new Post(){Id=Guid.NewGuid(), CategoryId=subCategory1Of1Id, Code = null, Name="Post 2", Url="post-2", Tags="post 2, tag post 2", MetaTitle="Meta title post 2", MetaDescription="Meta Description post 2", MetaKeywords="post 2, post b", Status=Status.Actived, SortOrder=4, Content="Content Post 2", Description="Description Post 2", HomeFlag=0, HomeOrder=0, HotFlag=0, HotOrder=0, Viewed=0, DateCreated=DateTime.Now, DateModified=DateTime.Now},
                //new Post(){Id=Guid.NewGuid(), CategoryId=subCategory2Of1Id, Code = null, Name="Post 3", Url="post-3", Tags="post 3, tag post 3", MetaTitle="Meta title post 3", MetaDescription="Meta Description post 3", MetaKeywords="post 3, post c", Status=Status.Actived, SortOrder=3, Content="Content Post 3", Description="Description Post 3", HomeFlag=0, HomeOrder=0, HotFlag=0, HotOrder=0, Viewed=0, DateCreated=DateTime.Now, DateModified=DateTime.Now},
                //new Post(){Id=Guid.NewGuid(), CategoryId=subCategory2Of1Id, Code = null, Name="Post 4", Url="post-4", Tags="post 4, tag post 4", MetaTitle="Meta title post 4", MetaDescription="Meta Description post 4", MetaKeywords="post 4, post d", Status=Status.Actived, SortOrder=4, Content="Content Post 4", Description="Description Post 4", HomeFlag=0, HomeOrder=0, HotFlag=0, HotOrder=0, Viewed=0, DateCreated=DateTime.Now, DateModified=DateTime.Now},
                //new Post(){Id=Guid.NewGuid(), CategoryId=subCategory1Of1Id, Code = null, Name="Post 1", Url="post-1", Tags="post 1, tag post 1", MetaTitle="Meta title post 1", MetaDescription="Meta Description post 1", MetaKeywords="post 1, post a", Status=Status.Actived, SortOrder=5, Content="Content Post 1", Description="Description Post 1", HomeFlag=0, HomeOrder=0, HotFlag=0, HotOrder=0, Viewed=0, DateCreated=DateTime.Now, DateModified=DateTime.Now},
                //new Post(){Id=Guid.NewGuid(), CategoryId=subCategory1Of1Id, Code = null, Name="Post 2", Url="post-2", Tags="post 2, tag post 2", MetaTitle="Meta title post 2", MetaDescription="Meta Description post 2", MetaKeywords="post 2, post b", Status=Status.Actived, SortOrder=6, Content="Content Post 2", Description="Description Post 2", HomeFlag=0, HomeOrder=0, HotFlag=0, HotOrder=0, Viewed=0, DateCreated=DateTime.Now, DateModified=DateTime.Now},
                //new Post(){Id=Guid.NewGuid(), CategoryId=subCategory2Of1Id, Code = null, Name="Post 3", Url="post-3", Tags="post 3, tag post 3", MetaTitle="Meta title post 3", MetaDescription="Meta Description post 3", MetaKeywords="post 3, post c", Status=Status.Actived, SortOrder=5, Content="Content Post 3", Description="Description Post 3", HomeFlag=0, HomeOrder=0, HotFlag=0, HotOrder=0, Viewed=0, DateCreated=DateTime.Now, DateModified=DateTime.Now},
                //new Post(){Id=Guid.NewGuid(), CategoryId=subCategory2Of1Id, Code = null, Name="Post 4", Url="post-4", Tags="post 4, tag post 4", MetaTitle="Meta title post 4", MetaDescription="Meta Description post 4", MetaKeywords="post 4, post d", Status=Status.Actived, SortOrder=6, Content="Content Post 4", Description="Description Post 4", HomeFlag=0, HomeOrder=0, HotFlag=0, HotOrder=0, Viewed=0, DateCreated=DateTime.Now, DateModified=DateTime.Now},
                //new Post(){Id=Guid.NewGuid(), CategoryId=subCategory1Of1Id, Code = null, Name="Post 1", Url="post-1", Tags="post 1, tag post 1", MetaTitle="Meta title post 1", MetaDescription="Meta Description post 1", MetaKeywords="post 1, post a", Status=Status.Actived, SortOrder=7, Content="Content Post 1", Description="Description Post 1", HomeFlag=0, HomeOrder=0, HotFlag=0, HotOrder=0, Viewed=0, DateCreated=DateTime.Now, DateModified=DateTime.Now},
                //new Post(){Id=Guid.NewGuid(), CategoryId=subCategory1Of1Id, Code = null, Name="Post 2", Url="post-2", Tags="post 2, tag post 2", MetaTitle="Meta title post 2", MetaDescription="Meta Description post 2", MetaKeywords="post 2, post b", Status=Status.Actived, SortOrder=8, Content="Content Post 2", Description="Description Post 2", HomeFlag=0, HomeOrder=0, HotFlag=0, HotOrder=0, Viewed=0, DateCreated=DateTime.Now, DateModified=DateTime.Now},
                //new Post(){Id=Guid.NewGuid(), CategoryId=subCategory2Of1Id, Code = null, Name="Post 3", Url="post-3", Tags="post 3, tag post 3", MetaTitle="Meta title post 3", MetaDescription="Meta Description post 3", MetaKeywords="post 3, post c", Status=Status.Actived, SortOrder=7, Content="Content Post 3", Description="Description Post 3", HomeFlag=0, HomeOrder=0, HotFlag=0, HotOrder=0, Viewed=0, DateCreated=DateTime.Now, DateModified=DateTime.Now},
                //new Post(){Id=Guid.NewGuid(), CategoryId=subCategory2Of1Id, Code = null, Name="Post 4", Url="post-4", Tags="post 4, tag post 4", MetaTitle="Meta title post 4", MetaDescription="Meta Description post 4", MetaKeywords="post 4, post d", Status=Status.Actived, SortOrder=8, Content="Content Post 4", Description="Description Post 4", HomeFlag=0, HomeOrder=0, HotFlag=0, HotOrder=0, Viewed=0, DateCreated=DateTime.Now, DateModified=DateTime.Now},
                //new Post(){Id=Guid.NewGuid(), CategoryId=subCategory1Of1Id, Code = null, Name="Post 1", Url="post-1", Tags="post 1, tag post 1", MetaTitle="Meta title post 1", MetaDescription="Meta Description post 1", MetaKeywords="post 1, post a", Status=Status.Actived, SortOrder=9, Content="Content Post 1", Description="Description Post 1", HomeFlag=0, HomeOrder=0, HotFlag=0, HotOrder=0, Viewed=0, DateCreated=DateTime.Now, DateModified=DateTime.Now},
                //new Post(){Id=Guid.NewGuid(), CategoryId=subCategory1Of1Id, Code = null, Name="Post 2", Url="post-2", Tags="post 2, tag post 2", MetaTitle="Meta title post 2", MetaDescription="Meta Description post 2", MetaKeywords="post 2, post b", Status=Status.Actived, SortOrder=10, Content="Content Post 2", Description="Description Post 2", HomeFlag=0, HomeOrder=0, HotFlag=0, HotOrder=0, Viewed=0, DateCreated=DateTime.Now, DateModified=DateTime.Now},
                //new Post(){Id=Guid.NewGuid(), CategoryId=subCategory2Of1Id, Code = null, Name="Post 3", Url="post-3", Tags="post 3, tag post 3", MetaTitle="Meta title post 3", MetaDescription="Meta Description post 3", MetaKeywords="post 3, post c", Status=Status.Actived, SortOrder=9, Content="Content Post 3", Description="Description Post 3", HomeFlag=0, HomeOrder=0, HotFlag=0, HotOrder=0, Viewed=0, DateCreated=DateTime.Now, DateModified=DateTime.Now},
                //new Post(){Id=Guid.NewGuid(), CategoryId=subCategory2Of1Id, Code = null, Name="Post 4", Url="post-4", Tags="post 4, tag post 4", MetaTitle="Meta title post 4", MetaDescription="Meta Description post 4", MetaKeywords="post 4, post d", Status=Status.Actived, SortOrder=10, Content="Content Post 4", Description="Description Post 4", HomeFlag=0, HomeOrder=0, HotFlag=0, HotOrder=0, Viewed=0, DateCreated=DateTime.Now, DateModified=DateTime.Now},
                //new Post(){Id=Guid.NewGuid(), CategoryId=subCategory1Of1Id, Code = null, Name="Post 1", Url="post-1", Tags="post 1, tag post 1", MetaTitle="Meta title post 1", MetaDescription="Meta Description post 1", MetaKeywords="post 1, post a", Status=Status.Actived, SortOrder=11, Content="Content Post 1", Description="Description Post 1", HomeFlag=0, HomeOrder=0, HotFlag=0, HotOrder=0, Viewed=0, DateCreated=DateTime.Now, DateModified=DateTime.Now},
                //new Post(){Id=Guid.NewGuid(), CategoryId=subCategory1Of1Id, Code = null, Name="Post 2", Url="post-2", Tags="post 2, tag post 2", MetaTitle="Meta title post 2", MetaDescription="Meta Description post 2", MetaKeywords="post 2, post b", Status=Status.Actived, SortOrder=12, Content="Content Post 2", Description="Description Post 2", HomeFlag=0, HomeOrder=0, HotFlag=0, HotOrder=0, Viewed=0, DateCreated=DateTime.Now, DateModified=DateTime.Now},
                //new Post(){Id=Guid.NewGuid(), CategoryId=subCategory2Of1Id, Code = null, Name="Post 3", Url="post-3", Tags="post 3, tag post 3", MetaTitle="Meta title post 3", MetaDescription="Meta Description post 3", MetaKeywords="post 3, post c", Status=Status.Actived, SortOrder=11, Content="Content Post 3", Description="Description Post 3", HomeFlag=0, HomeOrder=0, HotFlag=0, HotOrder=0, Viewed=0, DateCreated=DateTime.Now, DateModified=DateTime.Now},
                //new Post(){Id=Guid.NewGuid(), CategoryId=subCategory2Of1Id, Code = null, Name="Post 4", Url="post-4", Tags="post 4, tag post 4", MetaTitle="Meta title post 4", MetaDescription="Meta Description post 4", MetaKeywords="post 4, post d", Status=Status.Actived, SortOrder=12, Content="Content Post 4", Description="Description Post 4", HomeFlag=0, HomeOrder=0, HotFlag=0, HotOrder=0, Viewed=0, DateCreated=DateTime.Now, DateModified=DateTime.Now},
                //new Post(){Id=Guid.NewGuid(), CategoryId=subCategory1Of1Id, Code = null, Name="Post 1", Url="post-1", Tags="post 1, tag post 1", MetaTitle="Meta title post 1", MetaDescription="Meta Description post 1", MetaKeywords="post 1, post a", Status=Status.Actived, SortOrder=13, Content="Content Post 1", Description="Description Post 1", HomeFlag=0, HomeOrder=0, HotFlag=0, HotOrder=0, Viewed=0, DateCreated=DateTime.Now, DateModified=DateTime.Now},
                //new Post(){Id=Guid.NewGuid(), CategoryId=subCategory1Of1Id, Code = null, Name="Post 2", Url="post-2", Tags="post 2, tag post 2", MetaTitle="Meta title post 2", MetaDescription="Meta Description post 2", MetaKeywords="post 2, post b", Status=Status.Actived, SortOrder=14, Content="Content Post 2", Description="Description Post 2", HomeFlag=0, HomeOrder=0, HotFlag=0, HotOrder=0, Viewed=0, DateCreated=DateTime.Now, DateModified=DateTime.Now},
                //new Post(){Id=Guid.NewGuid(), CategoryId=subCategory2Of1Id, Code = null, Name="Post 3", Url="post-3", Tags="post 3, tag post 3", MetaTitle="Meta title post 3", MetaDescription="Meta Description post 3", MetaKeywords="post 3, post c", Status=Status.Actived, SortOrder=13, Content="Content Post 3", Description="Description Post 3", HomeFlag=0, HomeOrder=0, HotFlag=0, HotOrder=0, Viewed=0, DateCreated=DateTime.Now, DateModified=DateTime.Now},
                //new Post(){Id=Guid.NewGuid(), CategoryId=subCategory2Of1Id, Code = null, Name="Post 4", Url="post-4", Tags="post 4, tag post 4", MetaTitle="Meta title post 4", MetaDescription="Meta Description post 4", MetaKeywords="post 4, post d", Status=Status.Actived, SortOrder=14, Content="Content Post 4", Description="Description Post 4", HomeFlag=0, HomeOrder=0, HotFlag=0, HotOrder=0, Viewed=0, DateCreated=DateTime.Now, DateModified=DateTime.Now},
                //new Post(){Id=Guid.NewGuid(), CategoryId=subCategory1Of1Id, Code = null, Name="Post 1", Url="post-1", Tags="post 1, tag post 1", MetaTitle="Meta title post 1", MetaDescription="Meta Description post 1", MetaKeywords="post 1, post a", Status=Status.Actived, SortOrder=15, Content="Content Post 1", Description="Description Post 1", HomeFlag=0, HomeOrder=0, HotFlag=0, HotOrder=0, Viewed=0, DateCreated=DateTime.Now, DateModified=DateTime.Now},
                //new Post(){Id=Guid.NewGuid(), CategoryId=subCategory1Of1Id, Code = null, Name="Post 2", Url="post-2", Tags="post 2, tag post 2", MetaTitle="Meta title post 2", MetaDescription="Meta Description post 2", MetaKeywords="post 2, post b", Status=Status.Actived, SortOrder=16, Content="Content Post 2", Description="Description Post 2", HomeFlag=0, HomeOrder=0, HotFlag=0, HotOrder=0, Viewed=0, DateCreated=DateTime.Now, DateModified=DateTime.Now},
                //new Post(){Id=Guid.NewGuid(), CategoryId=subCategory2Of1Id, Code = null, Name="Post 3", Url="post-3", Tags="post 3, tag post 3", MetaTitle="Meta title post 3", MetaDescription="Meta Description post 3", MetaKeywords="post 3, post c", Status=Status.Actived, SortOrder=15, Content="Content Post 3", Description="Description Post 3", HomeFlag=0, HomeOrder=0, HotFlag=0, HotOrder=0, Viewed=0, DateCreated=DateTime.Now, DateModified=DateTime.Now},
                //new Post(){Id=Guid.NewGuid(), CategoryId=subCategory2Of1Id, Code = null, Name="Post 3", Url="post-3", Tags="post 3, tag post 3", MetaTitle="Meta title post 3", MetaDescription="Meta Description post 3", MetaKeywords="post 3, post c", Status=Status.Actived, SortOrder=16, Content="Content Post 3", Description="Description Post 3", HomeFlag=0, HomeOrder=0, HotFlag=0, HotOrder=0, Viewed=0, DateCreated=DateTime.Now, DateModified=DateTime.Now}
                //    };
                //    _context.Posts.AddRange(posts);
                //    _context.SaveChanges();
                //}
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }           
        }
    }
}
