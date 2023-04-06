using System.Collections.Generic;
using ACJudge.ViewModels.ProblemViewsModel;

namespace ACJudge.Data.Repositories.Interfaces
{
    public interface IProblemRepository<T>
    {
        IList<T> List();
        T Add(T entity);
        void Update(T entity);
        void Remove(int id);
        void Commit();
        T Find(int id);
        T FindByName(string onlineJudge,string problemSourceId); 
        IList<T> Search(ProblemFilter filter);
    }
}
