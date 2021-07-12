using GraduationProject.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.Data.Repositories.Interfaces
{
    public interface IBlogRepository<T>
    {
        IList<T> List();
        T Add(T Entity);
        void Update(T Entity);
        void Remove(int Id);
        void Commit();
        T Find(int Id);
        void UpdateVote(int blogId, int userId, int typeVote);
        void UpdateFavourite(int blogId, int userId);
        IList<T> Search(string Title, UserBlog PrepeardBy;
    }
}
