using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Graduation_Project.Data.Models;
using Graduation_Project.Data.Repositories.Interfaces;

namespace Graduation_Project.Data.Repositories
{
    public class UserDbRepository : IRepository<User>
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
            var user = dbcontext.Users.FirstOrDefault(user => user.UserId == Id);
            return user; 
        }

        public IList<User> List()
        {
            return dbcontext.Users.ToList(); 
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
            var user = Find(newUser.UserId);
            user = newUser; 
            Commit();
        }
        public void Commit()
        {
            dbcontext.SaveChanges();
        }
    }
}
