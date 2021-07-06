using GraduationProject.Data;
using GraduationProject.Data.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webHost = CreateHostBuilder(args).Build();
            RunMigrations(webHost);
            webHost.Run();
        }
        private static void RunMigrations(IHost webHost)
        {
            using var scope = webHost.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<EntitiesContext>();
            DbInitializer.Seed(scope.ServiceProvider);
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
