using ACJudge.Data;
using ACJudge.Data.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ACJudge
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webHost = CreateHostBuilder(args).Build();
            RunMigrations(webHost);
            using var scope = webHost.Services.CreateScope();
            var sevices = scope.ServiceProvider;
            DbInitializer.Seed(sevices);
            webHost.Run();
        }
        private static void RunMigrations(IHost webHost)
        {
            try
            {
                using (var scope = webHost.Services.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<EntitiesContext>();
                    db.Database.Migrate();
                }
            }
            catch
            {
                return;
            }


        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
