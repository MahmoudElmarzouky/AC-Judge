using System;
using System.Collections.Generic;

namespace GraduationProject.Data.Repositories.IProblemRepository
{
    public interface IProblemRepository<T>
    {
        IList<T> List();
        T Add(T Entity);
        void Update(T Entity);
        void Remove(int Id);
        void Commit();
        T Find(int Id);
        IList<T> Search(int x, IList<string> list);
    }
}
