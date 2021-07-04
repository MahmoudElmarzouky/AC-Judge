using GraduationProject.Data.Models;
using GraduationProject.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.Data.Repositories.DataBaseRepositories
{
    public class GroupDbRepository : IGroupRepository<Group>
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

        public void CreateNewGroup(int userId, Group newGroup)
        {
            newGroup.creationTime = DateTime.Now; 
            Add(newGroup);
            RemoveUserRole(userId, newGroup.GroupId); 
            newGroup.UserGroup.Add(CreateUserRole(userId, newGroup.GroupId, "Creator", DateTime.Now, false));
            Commit(); 
        }

        private UserGroup CreateUserRole(int userId, int groupId, string role, DateTime MemberSince, Boolean isFavourite)
        {
            return new UserGroup { UserId = userId, GroupId = groupId, UserRole = role, MemberSince = MemberSince, isFavourite = isFavourite };
        }
        private void RemoveUserRole(int userId, int groupId)
        {
            var group = Find(groupId);
            var userGroup = group.UserGroup.FirstOrDefault(u => u.GroupId == groupId && u.UserId == userId);
            if (userGroup != null)
            {
                group.UserGroup.Remove(userGroup);
                Commit(); 
            }
        }

        private void LoadCurrentGroup(Group group)
        {
           dbcontext.Entry(group).Collection(c => c.UserGroup).Load();
           dbcontext.Entry(group).Collection(c => c.Contests).Load();
           dbcontext.Entry(group).Collection(c => c.blogs).Load();
            foreach (var blog in group.blogs)
                dbcontext.Entry(blog).Collection(c => c.userBlog).Load(); 
            foreach (var real in group.UserGroup)
                dbcontext.Entry(real).Reference(c => c.User).Load();
            foreach (var contest in group.Contests)
                    dbcontext.Entry(contest).Collection(c => c.UserContest).Load();
            foreach (var blog in group.blogs)
                dbcontext.Entry(blog).Collection(c => c.userBlog).Load();
        }

        public void AddUser(int groupId, int userId)
        {
            RemoveUserRole(userId, groupId);
            var group = Find(groupId);
            group.UserGroup.Add(CreateUserRole(userId, groupId, "Participant", DateTime.Now, false));
            Commit(); 
        }

        public void RemoveUser(int userId, int groupId)
        {
            RemoveUserRole(userId, groupId); 
            // Assign New Use As a creator if the deleted user is the creator 
        }
        private UserGroup getUserGroupRole(int groupId, int userId)
        {
            return Find(groupId).UserGroup.FirstOrDefault(u => u.GroupId == groupId && u.UserId == userId);
        }
        public void FlipFavourite(int groupId, int userId)
        {
            var currentUserGroup = getUserGroupRole(groupId, userId); 
            if (currentUserGroup == null)
                return;
            currentUserGroup.isFavourite ^= true;
            Commit(); 
        }
        public void ChangeUserRole(int groupId, int userId, string newRole)
        {
            var currentUserGroup = getUserGroupRole(groupId, userId);
            if (currentUserGroup == null)
                return;
            currentUserGroup.UserRole = newRole;
            Commit(); 
        }
    }
}
