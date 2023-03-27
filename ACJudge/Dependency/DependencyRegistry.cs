using ACJudge.Data;
using ACJudge.Data.Models;
using ACJudge.Data.Repositories.DataBaseRepositories;
using ACJudge.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ACJudge.Dependency
{
    public abstract class DependencyRegistry
    {
        public static void Register(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            RegisterRepositories(services);
        }

        // [TODO] migrate to DataAccessLayer -> use Dao
        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped<IGroupRepository<Group>, GroupDbRepository>();
            services.AddScoped<ISubmissionRepository<Submission>, SubmissionDbRepository>();
            services.AddScoped<IUserRepository<User>, UserDbRepository>();
            services.AddScoped<IBlogRepository<Blog>, BlogDbRepository>();
            services.AddScoped<IRepository<Comment>, CommentDbRepository>();
            services.AddScoped<IProblemRepository<Problem>, ProblemDbRepository>();
            services.AddScoped<IContestRepository<Contest>, ContestDbRepository>();
        }
    }
}