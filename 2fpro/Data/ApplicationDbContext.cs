using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using _2fpro.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace _2fpro.Data
{

    public class ApplicationUser : IdentityUser
    {



        [Display(Name = "Телефон")]
        public string Phone { get; set; }

        [Display(Name = "Имя")]
        public string Name { get; set; }
        [Display(Name = "Фамилия")]
        public string Sirname { get; set; }
        [Display(Name = "Фамилия")]
        public string Firstname { get; set; }
        [Display(Name = "Адрес")]
        public string Address { get; set; }

        [Display(Name = "Тип доставки")]
        public string Delivery { get; set; }
        [Display(Name = "Способ оплаты")]
        public string Payment { get; set; }

        public int ResetUser { get; set; }

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<ImportDataProduct> ImportProducts { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Config> Configs { get; set; }
        public DbSet<YaMakret> YaMakrets { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Menu> Menues { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<ProdImage> ProdImages { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<LiveUser> LiveUsers { get; set; }
        public DbSet<StaticSection> StaticSections { get; set; }
        //public DbSet<LiveRoom> LiveRooms{ get; set; }
        //public DbSet<LiveConnection> LiveConnections { get; set; }
        public DbSet<Message> LiveMessages { get; set; }
        public DbSet<Diagnostic> Diagnostics { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            // Customize the ASP.NET Core Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Core Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            //builder.Entity<Config>()
            //.HasKey(p => new { p.ConfigID });
            //builder.Entity<Config>().Property(p => p.ConfigID).ValueGeneratedOnAdd();
            //builder.Entity<Menu>().Property<bool>("IsPublish");

            //modelbuilder.ForSqlServerHasSequence<int>("DBSequence")
            //                  .StartsAt(1000).IncrementsBy(2);

            // sequence for menu
            builder.HasSequence<int>("GetSeq");

            builder.Entity<Menu>()
                .Property(o => o.SortOrder)
                .HasDefaultValueSql("NEXT VALUE FOR GetSeq");
            // sequence for slider


            builder.Entity<Portfolio>()
                .Property(o => o.Sortindex)
                .HasDefaultValueSql("NEXT VALUE FOR GetSeq");
            builder.Entity<Product>()
               .Property(o => o.Sortindex)
               .HasDefaultValueSql("NEXT VALUE FOR GetSeq");
            builder.Entity<ProdImage>()
               .Property(o => o.Sortindex)
               .HasDefaultValueSql("NEXT VALUE FOR GetSeq");
            builder.Entity<Image>()
               .Property(o => o.Sortindex)
               .HasDefaultValueSql("NEXT VALUE FOR GetSeq");
            builder.Entity<Gallery>()
              .Property(o => o.Sortindex)
              .HasDefaultValueSql("NEXT VALUE FOR GetSeq");
            builder.Entity<Category>()
              .Property(o => o.Sortindex)
              .HasDefaultValueSql("NEXT VALUE FOR GetSeq");
            builder.Entity<StaticSection>()
              .Property(o => o.Sequance)
              .HasDefaultValueSql("NEXT VALUE FOR GetSeq");

            builder.Entity<Config>().HasData(new Config() { ConfigID = 1, SiteName = "sitename", SelectedIsOnlineID = false });

            //
            builder.Entity<Menu>().HasData(new Menu() { Id = 1, Url = "Home", Text = "title", ParentId = 0, MenuSection = 0, LastModifiedDate = new DateTime(2019, 08, 20) });

            builder.Entity<YaMakret>().HasData(new YaMakret() { ID = 1 });
            //builder.Entity<StaticSection>().Property(x => x.ID).ValueGeneratedOnAdd();

            builder.Entity<StaticSection>().HasData(
                    new StaticSection { ID = 1, Content = "ss", SectionType = 1, Title = "static1" },
                    new StaticSection { ID = 2, Content = "ss", SectionType = 2, Title = "static2" },
                    new StaticSection { ID = 3, Content = "ss", SectionType = 3, Title = "static3" },
                    new StaticSection { ID = 4, Content = "ss", SectionType = 4, Title = "static4" },
                    new StaticSection { ID = 5, Content = "ss", SectionType = 5, Title = "static5" },
                    new StaticSection { ID = 6, Content = "ss", SectionType = 6, Title = "static6" },
                    new StaticSection { ID = 7, Content = "ss", SectionType = 7, Title = "static7" },
                    new StaticSection { ID = 8, Content = "ss", SectionType = 8, Title = "static8" },
                    new StaticSection { ID = 9, Content = "ss", SectionType = 9, Title = "static9" },
                     new StaticSection { ID = 10, Content = "ss", SectionType = 10, Title = "static10" },
                    new StaticSection { ID = 11, Content = "ss", SectionType = 11, Title = "static11" });
            //     if (!context.Configs.Any())
            //{
            //    context.Configs.AddOrUpdate(x => x.ConfigID, new Config { SelectedIsOnlineID = false });

            //}
            ////dfdf sadf
            //if (!context.Menues.Any())
            //{
            //    context.Menues.AddOrUpdate(x => x.Url,
            //      new Menu { Url = "Home", Text = "title", ParentId = 0, MenuSection = 0, SortOrder = 0, LastModifiedDate = DateTime.Now }
            //      );

            //}
            //if (!context.StaticSections.Any())
            //{
            //    context.StaticSections.AddOrUpdate(x => x.Title,
            //        new StaticSection { Content = "ss", SectionType = 1, Title = "static1" },
            //        new StaticSection { Content = "ss", SectionType = 2, Title = "static2" },
            //        new StaticSection { Content = "ss", SectionType = 3, Title = "static3" },
            //        new StaticSection { Content = "ss", SectionType = 4, Title = "static4" },
            //        new StaticSection { Content = "ss", SectionType = 5, Title = "static5" },
            //        new StaticSection { Content = "ss", SectionType = 6, Title = "static6" }
            //    );

            //}

            //context.SaveChanges();
        }
    }
}
