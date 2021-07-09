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

        public IList<Submission> GetSubmissionSpecific(bool IsPublic,int Problemtype)
        {
            return dbcontext.Submissions
               .Include(p => p.problem)
               .Include(p => p.user)
               .Where(s => s.InContest==IsPublic && s.problem.problemType== Problemtype).ToList();
        }

        public IList<Submission> FindSubmissionUser(int Id)
        {
            return dbcontext.Submissions
                .Include(p => p.problem)
                .Include(p => p.user)
                .Where(p=> p.userId==Id)
                .ToList();
        }
    }
}
