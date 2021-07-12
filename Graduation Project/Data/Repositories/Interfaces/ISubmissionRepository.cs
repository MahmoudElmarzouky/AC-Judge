using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.Data.Repositories.Interfaces
{
    public interface ISubmissionRepository<T>
    {
        IList<T> List();
        T Add(T Entity);
        void Update(T Entity);
        void Remove(int Id);
        void Commit();
        T Find(int Id);
        IList<T> FindSubmissionUser(int Id);
        /*
         * Problemtype >> Contest =1 -- InterView =2
         * UserName 
         * ProblemName Ex:12A
         * ProblemSource Codeforces etc
         * ProblemResult Accepted - Wrong etc
         * ProblemLang c++ , java etc
         * ContestId can be null or assign to contest id
         */
        IList<T> GetSubmissionSpecific(int Problemtype, string UserName, string ProblemName, string ProblemSource, string ProblemResult, string ProblemLang, int? ContestId);
    }
}
