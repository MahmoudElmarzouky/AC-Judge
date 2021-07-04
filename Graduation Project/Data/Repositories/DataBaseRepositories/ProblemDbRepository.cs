using System;
using System.Collections.Generic;
using System.Linq;
using GraduationProject.Data.Models;
using GraduationProject.Data.Repositories.IProblemRepository;
using Microsoft.EntityFrameworkCore;

namespace GraduationProject.Data.Repositories.DataBaseRepositories
{
    public class ProblemDbRepository : IProblemRepository<Problem>
    {
        readonly private EntitiesContext dbcontext;
        public ProblemDbRepository(EntitiesContext dbcontext)
        {
            this.dbcontext = dbcontext; 
        }
        public Problem Add(Problem newProblem)
        {
            dbcontext.Add(newProblem);
            Commit(); 
            return newProblem; 
        }

        public void Commit()
        {
            dbcontext.SaveChanges(); 
        }

        public Problem Find(int Id)
        {
            var problem = dbcontext.Problems
                .FirstOrDefault(problem => problem.ProblemId == Id);
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

        public IList<Problem> Search(int x, IList<string> list)
        {
            var items = new List<Problem>();
            if (x == 1)
            {
                int type = int.Parse(list[0]);
                items = dbcontext.Problems.Where(item => item.problemType == type).ToList();
            }
            else if (x == 2)
            {
                int type = int.Parse(list[0]);
                string problemID = list[1];
                string problemName = list[2];
                string ProblemSource = list[3];
                items = dbcontext.Problems.Where(item =>
                    item.problemType == type
                    && item.problemSourceId.Contains(problemID)
                    && item.problemTitle.Contains(problemName)
                    && item.ProblemSource.Contains(ProblemSource)
                ).ToList();
            }
            else if (x == 3)
            {
                int type = int.Parse(list[0]);
                string problemID = list[1];
                string problemName = list[2];
                items = dbcontext.Problems.Where(item =>
                    item.problemType == type
                    && item.problemSourceId.Contains(problemID)
                    && item.problemTitle.Contains(problemName)
                ).ToList();
            }
            else if(x==4)
            {
                int type = int.Parse(list[0]);
                string problemID = list[1];
                string ProblemSource = list[2];
                items = dbcontext.Problems.Where(item =>
                    item.problemType == type
                    && item.problemSourceId.Contains(problemID)
                    && item.ProblemSource.Contains(ProblemSource)
                ).ToList();
            }
            else if (x == 5)
            {
                int type = int.Parse(list[0]);
                string problemName = list[1];
                string ProblemSource = list[2];
                items = dbcontext.Problems.Where(item =>
                    item.problemType == type
                    && item.problemTitle.Contains(problemName)
                    && item.ProblemSource.Contains(ProblemSource)
                ).ToList();
            }
            else if (x == 6)
            {
                int type = int.Parse(list[0]);
                string problemID = list[1];
                items = dbcontext.Problems.Where(item =>
                    item.problemType == type
                    && item.problemSourceId.Contains(problemID)
                ).ToList();
            }
            else if (x == 7)
            {
                int type = int.Parse(list[0]);
                string problemName = list[1];
                items = dbcontext.Problems.Where(item =>
                    item.problemType == type
                    && item.problemTitle.Contains(problemName)
                ).ToList();
            }
            else if (x == 8)
            {
                int type = int.Parse(list[0]);
                string ProblemSource = list[1];
                items = dbcontext.Problems.Where(item =>
                    item.problemType == type
                    && item.ProblemSource.Contains(ProblemSource)
                ).ToList();
            }

            return items;
        }

        public void Update(Problem newProblem)
        {
            dbcontext.Problems.Update(newProblem); 
            Commit();
        }
    }
}
