using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _2fpro.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity.UI.Services;
using _2fpro.Service.Repository;
using _2fpro.Service.Interface;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http.Features;
using _2fpro.Models;
using _2fpro.Extension;
using Microsoft.DotNet.PlatformAbstractions;
using System.Reflection;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using _2fpro.Models.Realtime;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.SqlServer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Rewrite;
using _2fpro.Models.Middleware;

namespace _2fpro
{
    public class Startup
    {
        public Startup(IConfiguration configuration, ILogger<Startup> logger, IHostingEnvironment env)
        {

            Configuration = configuration;
            _logger = logger;
            _env = env;

        }

        public IConfiguration Configuration;
        private readonly ILogger _logger;
        private readonly IHostingEnvironment _env;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(builder => builder.AddConsole());

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;


            });

            services.Configure<SiteConfig>(Configuration.GetSection("SiteConfig"));
            services.Configure<FormOptions>(options => options.MultipartBodyLengthLimit = 10000000); // or other given limi
                                                                                                     // COOKIE - SESSION

            services.AddRouting();
            services.AddResponseCaching();
            services.AddDistributedMemoryCache();


            if (_env.IsDevelopment())
            {
                services.AddDistributedSqlServerCache(options =>
                {

                    options.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
                    options.SchemaName = "dbo";
                    options.TableName = "SqlCacheTable";
                    options.DefaultSlidingExpiration = TimeSpan.FromDays(180);


                });
            }
            else
            {
                services.AddDistributedSqlServerCache(options =>
                {
                    options.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
                    options.SchemaName = "dbo";
                    options.TableName = "SqlCacheTable";
                    options.DefaultSlidingExpiration = TimeSpan.FromDays(180);
                });
            }
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.


                options.Cookie.HttpOnly = true;
                options.IdleTimeout = TimeSpan.FromDays(180);
            });
            // DB CONTEXT
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            //services.AddDefaultIdentity<ApplicationUser>();
            // ASP.NET MVC
            //services.AddAntiforgery(o => { o.HeaderName = "XN-XSRF-TOKEN"; o.Cookie.Name = "XN-CSRF-TOKEN"; });


            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options =>
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver())
                .AddRazorPagesOptions(options =>
                 {
                     options.AllowAreas = true;
                     options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
                     options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
                     options.Conventions.AddPageRoute("/robotstxt", "/Robots.Txt");

                 })
                 .AddSessionStateTempDataProvider();
            services.AddSignalR();
            // SERVICES DI
            services.AddHttpContextAccessor();
            services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IEmailService, EmailService>();

            services.AddScoped<IYaMarketRepo, YaMarketRepo>();
            services.AddScoped<IDiagnosticService, DiagnosticService>();
            services.AddScoped<IFileManager, FileManager>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<IConfigRepository, ConfigRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IPhotoGallery, PhotoGalleryRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderStatusRepository, OrderStatusRepository>();
            services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            services.AddScoped<IUserRepository, UsersRepository>();
            services.AddScoped<IConfigRepository, ConfigRepository>();


            services.AddScoped<IEmailSender, AuthEmailSender>();
            services.AddScoped<ISliderRepository, SliderRepository>();
            services.AddScoped<IStaticSectionRepository, StaticSectionRepository>();

            // Login Credits
            services.Configure<IdentityOptions>(options =>
            {
                // Default Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;


            });

            // COOKIE
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";

                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AdminLogin";
                options.ExpireTimeSpan = TimeSpan.FromDays(180);

                options.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = ctx =>
                    {
                        var requestPath = ctx.Request.Path;
                        if (requestPath.StartsWithSegments("/Admin"))
                        {
                            ctx.Response.Redirect("/Identity/Account/AdminLogin");
                        }
                        else
                        {
                            ctx.Response.Redirect("/Identity/Account/Login?ReturnUrl=" + requestPath + ctx.Request.QueryString);
                        }
                        return Task.CompletedTask;
                    }
                };

            });


            //Localization


            // Mobile Browser Detection



            services.AddKendo();

            //services.AddLogging();
            //services.AddLogging(loggingBuilder =>
            //{
            //    var loggingSection = Configuration.GetSection("Logging");
            //    loggingBuilder.AddFile(loggingSection);
            //});


            // using Microsoft.AspNetCore.Identity.UI.Services;

            //services.AddSingleton<IEmailSender, EmailSender>();
            //services.Configure<AuthMessageSenderOptions>(Configuration);

            //services.Configure<ii>

            services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders =
                ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        });
        }

        // This methods gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IApplicationLifetime lifetime, IHostingEnvironment env, IServiceProvider serviceProvider, ApplicationDbContext ctx, ILoggerFactory loggerFactory, IDistributedCache cache, IDiagnosticService diag)
        {



            app.UseForwardedHeaders();
            app.Use(async (context, next) =>
            {


                if (context.Request.IsHttps || context.Request.Headers["X-Forwarded-Proto"] == Uri.UriSchemeHttps)
                {
                    await next();
                }
                else
                {
                    string queryString = context.Request.QueryString.HasValue ? context.Request.QueryString.Value : string.Empty;
                    var https = "https://" + context.Request.Host + context.Request.Path + queryString;
                    context.Response.Redirect(https, true);
                }
            });


            lifetime.ApplicationStarted.Register(() =>
            {

            });
            lifetime.ApplicationStopped.Register(() =>
            {

            });
            //Errors Handle
            if (env.IsDevelopment())
            {

                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                //app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
                app.UseExceptionHandler("/error");

            }

            var config = GetConfigValues();
            app.UseStatusCodePagesWithReExecute("/error", "?statusCode={0}");


            //var options = new RewriteOptions();
            //options.Rules.Add(new RedirectToWwwRule());
            //options.AddRedirectToHttps();
            //app.UseRewriter(options);

            app.UseHsts();
            app.UseHttpsRedirection();

            app.UseResponseCaching();
            var cachePeriod = env.IsDevelopment() ? "600" : "104800";

            app.UseDefaultFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = x =>
                {
                    // Requires the following import:
                    // using Microsoft.AspNetCore.Http;
                    x.Context.Response.Headers.Append("Cache-Control", $"public, max-age={cachePeriod}");
                }
            });

            //app.Run(async context =>
            //{
            //    if (context.Request.Path == "/yandex_7dda11c199b878e2.html")
            //    {
            //        await context.Response.WriteAsync("Verification: 7dda11c199b878e2");
            //    }
            //});
            //app.Use(async (context, next) =>
            //{
            //    // For GetTypedHeaders, add: using Microsoft.AspNetCore.Http;
            //    context.Response.GetTypedHeaders().CacheControl =
            //        new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
            //        {
            //            Public = true,
            //            MaxAge = TimeSpan.FromSeconds(int.Parse(cachePeriod))
            //        };
            //    context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
            //        new string[] { "Accept-Encoding" };

            //    await next();
            //});

            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseSession();

            app.UseSignalR(routes =>
            {
                routes.MapHub<Myhub>("/myHub");
            });
            app.UseSiteConfiguration(config); // SITE SETTINGS 
            AddRoutesAndMvc(app); // addmvc + routes 


            app.UseRequestLocalization();
            app.UseKendo(env);

            // CreateRoles(serviceProvider).Wait();

            app.UseSitemap(config.SitePath); // sitemap+robots

            //app.UseRobotsTxt(builder =>
            //builder
            //    .AddSection(section =>
            //        section
            //            .AddComment("robots index")
            //            .AddUserAgent("*")
            //            .Disallow("/Account")
            //            .Disallow("/Signalr")
            //            .Disallow("/LiveChat")
            //            .Disallow("/Consultant")
            //            .Disallow("/Error")
            //            .Disallow("/Order")
            //            .Disallow("/Admin")
            //            .Disallow("/Config")
            //            .Disallow("/Widget")

            //        )
            //    .AddSitemap(config.SitePath+"/sitemap.xml")
            //);

        }
        private SiteConfig GetConfigValues()
        {
            var config = new SiteConfig();
            Configuration.GetSection("SiteConfig").Bind(config);

            return config;
        }

        private void AddRoutesAndMvc(IApplicationBuilder app)
        {
            string[] excludeUrl = { "Post", "tobuy", "Home", "PhotoGallery" };
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                   name: "Post",
                   template: "{controller=Post}/{action=Index}/{id?}");
                //routes.MapAreaRoute("admin_route", "Admin",
                //    "AdminPanel/{controller}/{action}/{id?}");
                routes.MapRoute(
                      name: "adm",
                      template: "/admin",
                      defaults: new { area = "Admin", controller = "Admin", action = "Index" });
                //routes.MapRoute(
                //       name: "home-index",
                //       template: "/",
                //       defaults: new { controller = "Home", action = "Index" });


                routes.MapRoute(
                   name: "ProdDetails",
                   template: "{controller=Product}/{action=Details}/{id?}/{title}");//"{controller=Product}/{action=Details}/{id?}");

                //routes.MapRoute(
                //     name: "adminlogin",
                //     template: "adminpanel_gate",
                //   defaults: new { controller = "AdminPanel", action = "Gate" });
                routes.MapRoute("prodindex", "Prodlist", new { area = "Admin", controller = "Product", action = "ProdList" });
                routes.MapRoute("Prodlist", "Prodlist/{catid?}/{catname}/{page?}", new { area = "Admin", controller = "Product", action = "ProdList" });
                routes.MapRoute("PageIndex", "pages/{url}", new { controller = "Dynamic", action = "Indexx" });

                routes.MapRoute(
                   name: "default",
                   template: "{controller=Home}/{action=Index}/{id?}");


            });
        }

        private void UpdateDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
                {
                    context.Database.Migrate();
                    context.SaveChanges();
                }


            }

        }
        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            //initializing custom roles   
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            string[] roleNames = { "Admin", "Customer", "User", "Manager", "Anonym" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    //create the roles and seed them to the database: Question 1  
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            ApplicationUser user = await UserManager.FindByEmailAsync("admin@mail.com");

            if (user == null)
            {
                user = new ApplicationUser()
                {
                    UserName = "admin@mail.com",
                    Email = "admin@mail.com"

                };
                await UserManager.CreateAsync(user, "admin@123222");
                var token = await UserManager.GenerateEmailConfirmationTokenAsync(user);
                await UserManager.ConfirmEmailAsync(user, token);
            }
            await UserManager.AddToRoleAsync(user, "Admin");

            ApplicationUser user1 = await UserManager.FindByEmailAsync("tester@gmail.com");

            if (user1 == null)
            {
                user1 = new ApplicationUser()
                {
                    UserName = "tester@gmail.com",
                    Email = "tester@gmail.com",
                };
                await UserManager.CreateAsync(user1, "user@123222");
            }
            await UserManager.AddToRoleAsync(user1, "User");


        }

    }
}
