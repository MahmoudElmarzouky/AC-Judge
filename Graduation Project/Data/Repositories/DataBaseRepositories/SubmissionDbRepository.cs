using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraduationProject.Data.Models;
using GraduationProject.Data.Repositories.Interfaces;

namespace GraduationProject.Data.Repositories.DataBaseRepositories
{
    public class SubmissionDbRepository : IRepository<Submission>
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
            var Submission = dbcontext.Submissions.FirstOrDefault(Submission => Submission.SubmissionId == Id);
            return Submission; 
        }

        public IList<Submission> List()
        {
            return dbcontext.Submissions.ToList(); 
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
    }
}
