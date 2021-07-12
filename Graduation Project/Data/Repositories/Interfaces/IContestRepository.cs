using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraduationProject.ViewModels.ContestViewsModel;

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
        void FlipFavourite(int contestId, int userId);
        Boolean IsOwner(int contestId, int userId); 
        IList<T> Filter(ContestFilter model);
        string Submit(int userId, int contestId, int problemId, string Code, string lang); 
    }
}
