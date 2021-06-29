using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraduationProject.Data.Models;
using GraduationProject.Data.Repositories.Interfaces;

namespace GraduationProject.Data.Repositories.DataBaseRepositories
{
    public class ProblemDbRepository : IRepository<Problem>
    {
        readonly private EntitiesContext dbcontext;
        public ProblemDbRepository(EntitiesContext dbcontext)
        {
            this.dbcontext = dbcontext; 
        }
        public Problem Add(Problem newProblem)
        {
            dbcontext.Problems.Add(newProblem);
            Commit(); 
            return newProblem; 
        }

        public void Commit()
        {
            dbcontext.SaveChanges(); 
        }

        public Problem Find(int Id)
        {
            var problem = dbcontext.Problems.FirstOrDefault(problem => problem.ProblemId == Id);
            return problem; 
        }

        public IList<Problem> List()
        {
            return dbcontext.Problems.ToList();
        }

        public void Remove(int Id)
        {
            var problem = Find(Id);
            if (problem != null)
            {
                dbcontext.Problems.Remove(problem);
                Commit(); 
            }
        }

        public void Update(Problem newProblem)
        {
            var problem = Find(newProblem.ProblemId);
            problem = newProblem;
            Commit();
        }
    }
}
