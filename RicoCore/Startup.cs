using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using RicoCore.Authorization;
using RicoCore.Data.EF;
using RicoCore.Data.Entities;
using RicoCore.Data.Entities.System;
using RicoCore.Extensions;
using RicoCore.Helpers;
using RicoCore.Infrastructure.Interfaces;
using RicoCore.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Hosting;

namespace RicoCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // Add DbContext
            services.AddDbContext<AppDbContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                   b => b.MigrationsAssembly("RicoCore.Data.EF")));

            // Add Identity
            services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            // Config Identity
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            // session
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(2);
                options.Cookie.HttpOnly = true;
            });

            // Add recaptcha
            //services.AddRecaptcha(new RecaptchaOptions()
            //{
            //    SiteKey = Configuration["Recaptcha:SiteKey"],
            //    SecretKey = Configuration["Recaptcha:SecretKey"]
            //});

            // Add AutoMapper
            services.AddAutoMapper();

            // external login Facebook/Google
            //services.AddAuthentication()
            //  .AddFacebook(facebookOpts =>
            //  {
            //      facebookOpts.AppId = Configuration["Authentication:Facebook:AppId"];
            //      facebookOpts.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
            //  })
            //  .AddGoogle(googleOpts => {
            //      googleOpts.ClientId = Configuration["Authentication:Google:ClientId"];
            //      googleOpts.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
            //  });

            // config AutoMapper (imapper for dot net core)
            services.AddSingleton(Mapper.Configuration); // using AutoMapper;
            services.AddScoped<IMapper>(sp => new Mapper(sp.GetRequiredService<AutoMapper.IConfigurationProvider>(), sp.GetService));

            services.AddMvc(options =>
            {
                options.CacheProfiles.Add("Default",
                    new CacheProfile()
                    {
                        Duration = 60
                    });
                options.CacheProfiles.Add("Never",
                    new CacheProfile()
                    {
                        Location = ResponseCacheLocation.None,
                        NoStore = true
                    });
            }).AddViewLocalization(
                    LanguageViewLocationExpanderFormat.Suffix,
                    opts => { opts.ResourcesPath = "Resources"; })
                .AddDataAnnotationsLocalization();

            // DefaultContractResolver()           
            // using Newtonsoft.Json.Serialization;
            // giai quyet Issue: GenericResult class tra ve null vi Json tu*. chuyen^? sang cu' phap' Camel (chu~ cai' dau tien tro thanh viet thuo`ng)

            //.AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            //services.AddLocalization(opts => { opts.ResourcesPath = "Resources"; });

            //services.Configure<RequestLocalizationOptions>(
            //  opts =>
            //  {
            //      var supportedCultures = new List<CultureInfo>
            //      {
            //          new CultureInfo("vi-VN"),
            //          new CultureInfo("en-US")                        
            //      };

            //      opts.DefaultRequestCulture = new RequestCulture("vi-VN");
            //      // Formatting numbers, dates, etc.
            //      opts.SupportedCultures = supportedCultures;
            //      // UI strings that we have localized.
            //      opts.SupportedUICultures = supportedCultures;
            //  });

            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Add Razor Pages
            services.AddRazorPages()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
                    options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
                    //options.Conventions.AddAreaFolderRouteModelConvention("Identity", "/Account/", model =>
                    //{
                    //    foreach (var selector in model.Selectors)
                    //    {
                    //        var attributeRouteModel = selector.AttributeRouteModel;
                    //        attributeRouteModel.Order = -1;
                    //        attributeRouteModel.Template = attributeRouteModel.Template.Remove(0, "Identity".Length);
                    //    }
                    //});
                });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });

            // add application services.
            services.AddTransient(typeof(IUnitOfWork), typeof(EFUnitOfWork));
            services.AddScoped(typeof(IRepository<,>), typeof(EFRepository<,>));
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IViewRenderService, ViewRenderService>();

            //Register for DI
            services.AddScoped<SignInManager<AppUser>, SignInManager<AppUser>>();
            services.AddScoped<UserManager<AppUser>, UserManager<AppUser>>();
            services.AddScoped<RoleManager<AppRole>, RoleManager<AppRole>>();
            services.AddTransient(typeof(IWebServiceBase<,,>), typeof(WebServiceBase<,,>));
            services.AddScoped<IUserClaimsPrincipalFactory<AppUser>, CustomClaimsPrincipalFactory>();

            // Initialize Seed Data
            services.AddTransient<DbInitializer>();            

            //Register for service
            ServiceRegister.Register(services);

            // chỉ cho phép domain nào truy cập được
            services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyMethod()
                .AllowAnyHeader()
                //.AllowAnyOrigin()
                //.WithOrigins("http://rem.songkhoe.co", "https://rem.songkhoe.co")                
                .AllowCredentials();
            }));

            // Register SignalR
            //services.AddSignalR();

            //services.AddMvc()
            //   .AddViewLocalization()
            //   .AddDataAnnotationsLocalization()
            //   .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
            services.AddTransient<IAuthorizationHandler, BaseResourceAuthorizationHandler>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        //public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //loggerFactory.AddFile("Logs/logs-{Date}.txt");
            if (env.IsDevelopment())
            {
                //app.UseBrowserLink();
                app.UseDeveloperExceptionPage();

                // need uncomment
                //app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            // resizing images
            //app.UseImageResizer();

            app.UseHttpsRedirection();
            // For wwwroot directory
            app.UseStaticFiles();
            app.UseCookiePolicy();

            // Configure Identity
            app.UseAuthentication();

            app.UseRouting();

            // must appear between app.UseRouting() and app.UseEndpoints()
            app.UseAuthorization();

            app.UseCors("CorsPolicy");
            //app.UseSignalR(routes =>
            //{
            //    routes.MapHub<SignalRHub>("/teduHub");
            //});

            // session
            app.UseSession();

            // localization
            //var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            //app.UseRequestLocalization(options.Value);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Home}/{action=Index}/{id?}");
            //    //template: "{area:exists}/{controller=Login}/{action=Index}/{id?}");
            //    routes.MapRoute(
            //            name: "areaRoute",
            //            template: "{area:exists}/{controller=Login}/{action=Index}/{id?}");
            //    //routes.MapRoute(
            //    //    name: "danh sach bai viet",
            //    //    template: "danh-muc/{controller=Post}/{action=Index}/{id?}");
            //    //routes.MapRoute(
            //    //    name: "Chi tiet bai viet",
            //    //    template: "bai-viet/{controller=Home}/{action=Index}/{id?}");
            //});
        }
    }
}
