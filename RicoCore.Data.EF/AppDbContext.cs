using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using RicoCore.Data.EF.Configurations;
using RicoCore.Data.EF.Extensions;
using RicoCore.Data.Entities;
using RicoCore.Data.Entities.ECommerce;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.SharedKernel;
using RicoCore.Data.Entities.System;
using RicoCore.Data.Entities.Content;
using RicoCore.Data.Entities.Advs;
using File = RicoCore.Data.Entities.System.File;

namespace RicoCore.Data.EF
{
    /// <summary>
    /// AppDbContext inherit from IdentityDbContext:
    /// using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    /// (NuGet Packages)
    /// Microsoft.AspNetCore.Identity (2.0.0)
    /// Microsoft.AspNetCore.Identity.EntityFrameworkCore (2.0.0)
    /// </summary>
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        /// DbContextOptions:
        /// using Microsoft.EntityFrameworkCore;
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        
        /// <summary>
        /// public DbSet<TEntity> where TEntity : class
        /// using Microsoft.EntityFrameworkCore;
        /// </summary>
        #region DbSet

        #region System
        public DbSet<AppRole> AppRoles { set; get; }
        public DbSet<AppUser> AppUsers { set; get; }
        public DbSet<Account> Accounts { set; get; }
        public DbSet<Password> Passwords { set; get; }
        public DbSet<Function> Functions { set; get; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Setting> SystemConfigs { get; set; }
        //public DbSet<Menu> Menus { set; get; }
        public DbSet<Error> Errors { set; get; }
        public DbSet<Language> Languages { set; get; }
        public DbSet<Logo> Logos { set; get; }
        public DbSet<Menu> Menus { set; get; }
        public DbSet<Announcement> Announcements { set; get; }
        public DbSet<AnnouncementUser> AnnouncementUsers { set; get; }
        public DbSet<AuditLog> AuditLogs { set; get; }
        public DbSet<File> Files { set; get; }
        public DbSet<Folder> Folders { set; get; }
        public DbSet<SubProject> SubProjects { set; get; }
        public DbSet<Project> Projects { set; get; }

        #endregion System

        #region Content
        public DbSet<PostCategory> PostCategories { set; get; }
        public DbSet<Post> Posts { set; get; }
        public DbSet<PostImage> PostImages { set; get; }
        public DbSet<PostTag> PostTags { set; get; }
        public DbSet<ContactDetail> ContactDetails { set; get; }
        public DbSet<Tag> Tags { set; get; }
        public DbSet<Page> Pages { set; get; }
        public DbSet<Slide> Slides { set; get; }
        public DbSet<Feedback> Feedbacks { set; get; }
        public DbSet<Footer> Footers { set; get; }
        #endregion Content

        #region Advertise
        public DbSet<AdvertistmentPage> AdvertistmentPages { get; set; }
        public DbSet<Advertistment> Advertistments { get; set; }
        public DbSet<AdvertistmentPosition> AdvertistmentPositions { get; set; }
        #endregion

        #region ECommerce
        public DbSet<RicoCore.Data.Entities.ECommerce.Attribute> Attributes { set; get; }
        public DbSet<AttributeDescription> AttributeDescriptions { set; get; }
        public DbSet<AttributeGroup> AttributeGroups { set; get; }
        public DbSet<AttributeGroupDescription> AttributeGroupDescriptions { set; get; }

        public DbSet<Bill> Bills { set; get; }
        public DbSet<BillDetail> BillDetails { set; get; }
        public DbSet<Color> Colors { set; get; }
        public DbSet<Size> Sizes { set; get; }
        public DbSet<Product> Products { set; get; }
        public DbSet<ProductCategory> ProductCategories { set; get; }
        public DbSet<ProductAttribute> ProductAttributes { set; get; }
        public DbSet<ProductImage> ProductImages { set; get; }
        public DbSet<ProductQuantity> ProductQuantities { set; get; }
        public DbSet<ProductItem> ProductItems { set; get; }
        public DbSet<ProductColor> ProductColors { set; get; }
        public DbSet<ProductTag> ProductTags { set; get; }
        public DbSet<WholePrice> WholePrices { set; get; }
        public DbSet<ProductWishlist> ProductWishlists { set; get; }
        #endregion

        #endregion DbSet

        /// <summary>
        /// ModelBuilder
        /// using Microsoft.EntityFrameworkCore;
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /// IdentityUserClaim, IdentityRoleClaim, IdentityUserLogin, IdentityUserRole, IdentityUserToken
            /// using Microsoft.AspNetCore.Identity;     
            #region Identity Config
            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims").HasKey(x => x.Id);
            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims").HasKey(x => x.Id);
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins").HasKey(x => x.UserId);
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles").HasKey(x => new { x.RoleId, x.UserId });
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens").HasKey(x => new { x.UserId, x.LoginProvider, x.Name });
            #endregion Identity Config

            modelBuilder.AddConfiguration(new AdvertistmentPositionConfiguration());
            modelBuilder.AddConfiguration(new AdvertistmentPageConfiguration());
            modelBuilder.AddConfiguration(new ContactDetailConfiguration());
            modelBuilder.AddConfiguration(new ErrorConfiguration());
            modelBuilder.AddConfiguration(new FooterConfiguration());
            modelBuilder.AddConfiguration(new TagConfiguration());
            modelBuilder.AddConfiguration(new PostTagConfiguration());
            modelBuilder.AddConfiguration(new ProductTagConfiguration());
            //modelBuilder.AddConfiguration(new PostCategoryConfiguration());
            //modelBuilder.AddConfiguration(new LanguageConfiguration());
            //modelBuilder.AddConfiguration(new PageConfiguration());                        
            //modelBuilder.AddConfiguration(new SystemConfigConfiguration());

            //base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            try
            {
                /// Using Linq:
                /// using System.Linq;

                /// enum EntityState
                /// using Microsoft.EntityFrameworkCore;
                var modified = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified || e.State == EntityState.Added);

                /// EntityEntry
                /// using Microsoft.EntityFrameworkCore.ChangeTracking;
                foreach (EntityEntry item in modified)
                {
                    //var changedOrAddedItem = item.Entity as IDateTracking;
                    //if (changedOrAddedItem != null)
                    if (item.Entity is IDateTracking changedOrAddedItem)
                    {                        
                        if (item.State == EntityState.Added)
                        {
                            changedOrAddedItem.DateCreated = DateTime.Now;                            
                        }
                        changedOrAddedItem.DateModified = DateTime.Now;
                    }
                }
                return base.SaveChanges();
            }
            /// DbUpdateException
            /// using Microsoft.EntityFrameworkCore;
            catch (DbUpdateException entityException)
            {
                throw new ModelValidationException(entityException.Message);
            }
            //return base.SaveChanges();
        }

        /// <summary>
        /// IDesignTimeDbContextFactory
        /// using Microsoft.EntityFrameworkCore.Design;
        /// </summary>
        public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
        {
            public AppDbContext CreateDbContext(string[] args)
            {
                /// Iconfiguration
                /// ConfigurationBuilder
                /// using Microsoft.Extensions.Configuration;
                IConfigurationRoot configuration = new ConfigurationBuilder()

                /// SetBasePath
                /// using Microsoft.Extensions.Configuration.FileExtensions
                /// using Microsoft.Extensions.Configuration.Json
                /// Directory
                /// using Sytem.IO
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

                /// DbContextOptionsBuilder
                /// using Microsoft.EntityFrameworkCore;
                var builder = new DbContextOptionsBuilder<AppDbContext>();
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                builder.UseSqlServer(connectionString);
                return new AppDbContext(builder.Options);
            }
        }
    }
}