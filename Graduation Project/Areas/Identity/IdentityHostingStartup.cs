﻿using System;
using Graduation_Project.Areas.Identity.Data;
using Graduation_Project.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(Graduation_Project.Areas.Identity.IdentityHostingStartup))]
namespace Graduation_Project.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<UserAccountsContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("UserAccountsContextConnection")));

                services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<UserAccountsContext>();
            });
        }
    }
}