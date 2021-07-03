using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.Data.Repositories.Interfaces
{
    public interface IUserRepository<T>
    {
        IList<T> List();
        T Add(T Entity);
        void Update(T Entity);
        void Remove(int Id);
        void Commit();
        T Find(int Id);
        T Find(string Id);
        T FindByUserName(string name);
    }
}
