using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Graduation_Project.Data.Models;

namespace Graduation_Project.Data
{
    public class Context : DbContext
    {
        public Context(): base("Server=(localdb)\\mssqllocaldb;Database=GraduationProject;Trusted_Connection=True;MultipleActiveResultSets=true")
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Contest> Contests { get; set; }
        public DbSet<Problem> Problems { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<UserContest> UserContest { get; set; }
        public DbSet<UserGroup> UserGroup { get; set; }
        public DbSet<ProblemTag> ProblemTag { get; set; }
    }
}
