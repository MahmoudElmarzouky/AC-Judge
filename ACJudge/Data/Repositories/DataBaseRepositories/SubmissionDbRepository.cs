using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACJudge.Data.Models;
using ACJudge.Data.Repositories.Interfaces;
using ACJudge.ViewModels.ProblemViewsModel;
using Microsoft.EntityFrameworkCore;

namespace ACJudge.Data.Repositories.DataBaseRepositories
{
    public class SubmissionDbRepository : ISubmissionRepository<Submission>
    {
        private readonly EntitiesContext _dbContext;
        public SubmissionDbRepository(EntitiesContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Submission Add(Submission newSubmission)
        {
            _dbContext.Submissions.Add(newSubmission);
            Commit();
            return newSubmission;
        }
        public async Task<Submission> AddAsync(Submission newSubmission)
        {
            await _dbContext.Submissions.AddAsync(newSubmission);
            Commit();
            return newSubmission;
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public Submission Find(int id)
        {
            var submission = List().FirstOrDefault(innerSubmission => innerSubmission.SubmissionId == id);
            return submission;
        }

        public IList<Submission> List()
        {
            return _dbContext.Submissions
                .Include(p => p.Problem)
                .Include(p => p.User)
                .ToList();
        }

        public void Remove(int id)
        {
            var submission = Find(id);
            if (submission == null) return;
            _dbContext.Submissions.Remove(submission);
            Commit();
        }
        public void Update(Submission newSubmission)
        {
            _dbContext.Submissions.Update(newSubmission);
            Commit();
        }

        public IList<Submission> GetSpecificSubmission(StatusFilter filter)
        {
            return _dbContext.Submissions
               .Include(p => p.Problem)
               .Include(p => p.User)
               .Include(p=>p.Contest)
               .Where(filter.Valid).ToList();
        }

        public IList<Submission> FindUserSubmissions(int id)
        {
            return List().Where(p => p.UserId == id).ToList();
        }
    }
}
