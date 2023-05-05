using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using _2fpro.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace _2fpro
{
    public class Program
    {

        public static Dictionary<string, string> arrayDict = new Dictionary<string, string>
        {
            {"array:entries:0", "value0"},
            {"array:entries:1", "value1"},
            {"array:entries:2", "value2"},
            {"array:entries:4", "value4"},
            {"array:entries:5", "value5"}
        };
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args)
                 .Build();



            using (var serviceScope = host.Services
               .GetRequiredService<IServiceScopeFactory>()
               .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
                {
                    context.Database.Migrate();
                    context.SaveChanges();
                }

            }
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
             .ConfigureAppConfiguration((hostingContext, config) =>
             {
                 config.SetBasePath(Directory.GetCurrentDirectory());
                 config.AddCommandLine(args);
                 config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                 config.AddEnvironmentVariables();
                 //config.AddCommandLine(args, _switchMappings);

             })
            .UseStartup<Startup>();
        //.UseKestrel(options =>
        //{

        //    options.Limits.MaxRequestBodySize = 4000000; // or a given limit
        //});
        //.UseUrls("https://*:56621", "http://*:56620");


    }
}
