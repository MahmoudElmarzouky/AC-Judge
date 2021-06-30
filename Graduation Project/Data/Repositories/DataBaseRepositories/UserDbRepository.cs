using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraduationProject.Data.Models;
using GraduationProject.Data.Repositories.Interfaces;

namespace GraduationProject.Data.Repositories
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
            dbcontext.Users.Update(newUser); 
            Commit();
        }
        public void Commit()
        {
            dbcontext.SaveChanges();
        }

        public IList<User> search(int x, IList<string> list)
        {
            throw new NotImplementedException();
        }
    }
}
