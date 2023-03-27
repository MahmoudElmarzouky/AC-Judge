using System;
using ACJudge.Areas.Identity.Data;
using ACJudge.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(ACJudge.Areas.Identity.IdentityHostingStartup))]
namespace ACJudge.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<UserAccountsContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("UserAccountsContextConnection")));
                // must Confirm the Email -> set RequireConfirmedAccount = true 
                services.AddDefaultIdentity<AuthUser>(options => options.SignIn.RequireConfirmedAccount = false)
                    .AddEntityFrameworkStores<UserAccountsContext>();
            });
        }
    }
}