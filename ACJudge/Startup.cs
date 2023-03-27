using ACJudge.Data;
using ACJudge.Data.Models;
using ACJudge.Data.Repositories.DataBaseRepositories;
using ACJudge.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ACJudge
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddDbContext<EntitiesContext>(
               options =>
               {
                   options.UseSqlServer(Configuration.GetConnectionString("UserAccountsContextConnection"));
               });
            services.AddScoped<IGroupRepository<Group>, GroupDbRepository> ();

            services.AddScoped<ISubmissionRepository<Submission>, SubmissionDbRepository> ();
            services.AddScoped<IUserRepository<User>, UserDbRepository>(); 
            services.AddScoped<IBlogRepository<Blog>, BlogDbRepository>();
            services.AddScoped<IRepository<Comment>, CommentDbRepository>();
            services.AddScoped<IProblemRepository<Problem>, ProblemDbRepository > ();
            services.AddScoped<IContestRepository<Contest>, ContestDbRepository>(); 
          
            services.AddMvc(options => options.EnableEndpointRouting = false);
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMvc(route => {
                route.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
            
        }
    }
}
