using System.Collections.Generic;
using System.Threading.Tasks;
using ACJudge.ViewModels.ProblemViewsModel;

namespace ACJudge.Data.Repositories.Interfaces
{
    public interface ISubmissionRepository<T>
    {
        IList<T> List();
        T Add(T entity);
        Task<T> AddAsync(T entity);
        void Update(T entity);
        void Remove(int id);
        void Commit();
        T Find(int id);
        IList<T> FindUserSubmissions(int id);
        /*
         * Problemtype >> Contest =1 -- InterView =2
         * UserName 
         * ProblemName Ex:12A
         * ProblemSource Codeforces etc
         * ProblemResult Accepted - Wrong etc
         * ProblemLang c++ , java etc
         * ContestId can be null or assign to contest id
         */
        IList<T> GetSpecificSubmission(StatusFilter filter);
    }
}
