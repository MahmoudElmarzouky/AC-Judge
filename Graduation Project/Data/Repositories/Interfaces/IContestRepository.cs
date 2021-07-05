using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.Data.Repositories.Interfaces
{
    public interface IContestRepository<T>
    {
        IList<T> List();
        IList<T> PublicContests(); 
        T Add(T Entity);
        void Update(T Entity);
        void Remove(int Id);
        void Commit();
        void AddProblemToContest(int problemId, int contestId);
        T CreateNewContest(int userId, T newContest);
        T Find(int Id);
        void RegisterInContest(int userId, int contestId); 
    }
}
