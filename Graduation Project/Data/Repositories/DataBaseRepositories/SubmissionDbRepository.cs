using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraduationProject.Data.Models;
using GraduationProject.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GraduationProject.Data.Repositories.DataBaseRepositories
{
    public class SubmissionDbRepository : ISubmissionRepository<Submission>
    {
        readonly private EntitiesContext dbcontext;
        public SubmissionDbRepository(EntitiesContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        public Submission Add(Submission newSubmission)
        {
            dbcontext.Submissions.Add(newSubmission);
            Commit();
            return newSubmission;
        }

        public void Commit()
        {
            dbcontext.SaveChanges();
        }

        public Submission Find(int Id)
        {
            var Submission = dbcontext.Submissions
                .Include(p => p.problem)
                .Include(p => p.user)
                .FirstOrDefault(Submission => Submission.SubmissionId == Id);
            return Submission;
        }

        public IList<Submission> List()
        {
            return dbcontext.Submissions
                .Include(p => p.problem)
                .Include(p => p.user)
                .ToList();
        }

        public void Remove(int Id)
        {
            var Submission = Find(Id);
            if (Submission != null)
            {
                dbcontext.Submissions.Remove(Submission);
                Commit();
            }
        }


        public void Update(Submission newSubmission)
        {
            dbcontext.Submissions.Update(newSubmission);
            Commit();
        }

        public IList<Submission> GetSubmissionSpecific(int Problemtype, string UserName, string ProblemName, string ProblemSource, string ProblemResult, string ProblemLang, int? ContestId)
        {
            return dbcontext.Submissions
               .Include(p => p.problem)
               .Include(p => p.user)
               .Where(s =>
                s.problem.problemType == Problemtype &&
                (UserName==""? s.user.UserName.Contains(UserName) : s.user.UserName == UserName) &&
                s.problem.problemSourceId.Contains(ProblemName) &&
                s.problem.ProblemSource.Contains(ProblemSource) &&
                s.Verdict.Contains(ProblemResult) &&
                s.ProgrammingLanguage.Contains(ProblemLang) &&
                s.contestId == ContestId
               ).ToList();
        }

        public IList<Submission> FindSubmissionUser(int Id)
        {
            return dbcontext.Submissions
                .Include(p => p.problem)
                .Include(p => p.user)
                .Where(p => p.userId == Id)
                .ToList();
        }
    }
}
