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
    }
