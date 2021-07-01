using GraduationProject.Data.Models;
using GraduationProject.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.Data.Repositories.DataBaseRepositories
{
    public class GroupDbRepository : IRepository<Group>
    {
        readonly private EntitiesContext dbcontext;
        public GroupDbRepository(EntitiesContext dbcontext)
        {
            this.dbcontext = dbcontext;
            foreach(var item in this.dbcontext.Groups.ToList())
                LoadCurrentGroup(item);
        }
        public Group Add(Group newGroup)
        {
            dbcontext.Add(newGroup);
            Commit();
            return newGroup;
        }

        public void Commit()
        {
            dbcontext.SaveChanges();
        }

        public Group Find(int Id)
        {
            var group = dbcontext.Groups.FirstOrDefault(group => group.GroupId == Id);
           
            return group;
        }

        public IList<Group> List()
        {
            var list = dbcontext.Groups;
            
            return dbcontext.Groups.ToList();
        }

        public void Remove(int Id)
        {
            var group = Find(Id);
            if (group != null)
            {
                dbcontext.Groups.Remove(group);
                Commit();
            }
        }

        

        public void Update(Group newGroup)
        {
            dbcontext.Groups.Update(newGroup);
            Commit();
        }
        private void LoadCurrentGroup(Group group)
        {
           dbcontext.Entry(group).Collection(c => c.UserGroup).Load();
           dbcontext.Entry(group).Collection(c => c.Contests).Load();
           dbcontext.Entry(group).Collection(c => c.blogs).Load();
            foreach (var real in group.UserGroup)
                dbcontext.Entry(real).Reference(c => c.User).Load();
        }
    }
}
