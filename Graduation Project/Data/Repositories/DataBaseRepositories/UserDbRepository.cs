using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraduationProject.Data.Models;
using GraduationProject.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GraduationProject.Data.Repositories
{
    public class UserDbRepository : IUserRepository<User>
    {
        readonly private EntitiesContext dbcontext; 
        public UserDbRepository(EntitiesContext dbcontext)
        {
            this.dbcontext = dbcontext; 
        }
        public User Add(User newUser)
        {
            dbcontext.Users.Add(newUser);
            Commit();
            return newUser; 
        }
        public User Find(int Id)
        {
            var user = dbcontext.Users
                .Include(s => s.submissions)
                .Include(pu => pu.ProblemUsers)
                .FirstOrDefault(user => user.UserId == Id);
            return user; 
        }
        public User Find(string Id)
        {
            var user = dbcontext.Users
                .Include(s => s.submissions)
                .Include(pu => pu.ProblemUsers)
                .FirstOrDefault(user => user.UserIdentityId == Id);
            return user;
        }
        public IList<User> List()
        {
            return dbcontext.Users
                .Include(s => s.submissions)
                .Include(pu => pu.ProblemUsers)
                .ToList(); 
        }

        public void Remove(int Id)
        {
            var user = Find(Id);
            if (user != null)
            {
                dbcontext.Users.Remove(user);
                Commit();
            }
        }

        public void Update(User newUser)
        {
            dbcontext.Users.Update(newUser); 
            Commit();
        }
        public void Commit()
        {
            dbcontext.SaveChanges();
        }

        public User FindByUserName(string name)
        {
            var user = dbcontext.Users.FirstOrDefault(u => u.UserName == name);
            return user; 
        }
    }
}
