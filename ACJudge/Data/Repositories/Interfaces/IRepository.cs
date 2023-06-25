using System.Collections.Generic;

namespace ACJudge.Data.Repositories.Interfaces
{
    public interface IRepository<T>
    {
        IList<T> List();
        T Add(T item);
        void Update(T item);
        void Remove(int id);
        void Commit();
        T Find(int id);
    }
}
