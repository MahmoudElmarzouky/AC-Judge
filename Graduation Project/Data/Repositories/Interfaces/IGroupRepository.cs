using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraduationProject.Data.Models;

namespace GraduationProject.Data.Repositories.Interfaces
{
    public interface IGroupRepository<T>
    {
        IList<T> List();
        T Add(T Entity);
        void Update(T Entity);
        void Remove(int Id);
        void Commit();
        T Find(int Id);
        void CreateNewGroup(int userId, Group newGroup);
        void AddUser(int groupId, int userId);
        void InviteUser(int groupId, int userId); 
        void RemoveUser(int userId, int groupId);
        void FlipFavourite(int groupId, int userId);
        void ChangeUserRole(int groupId, int userId, string v);
        IList<T> MyGroups(int id);
        Boolean IsOwner(int groupId, int userId); 
    }
}
