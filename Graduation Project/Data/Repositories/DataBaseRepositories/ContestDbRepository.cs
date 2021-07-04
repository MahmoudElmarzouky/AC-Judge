using GraduationProject.Data.Models;
using GraduationProject.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.Data.Repositories.DataBaseRepositories
{
    public class ContestDbRepository : IRepository<Contest>
    {
        readonly private EntitiesContext dbcontext;
        public ContestDbRepository(EntitiesContext dbcontext)
        {
            this.dbcontext = dbcontext;
            foreach(var item in dbcontext.Contests.ToList())
                LoadCurrentContest(item);
        }
    
        public Contest Add(Contest newContest)
        {
            dbcontext.Add(newContest);
            Commit();
            return newContest;
        }

        public void Commit()
        {
            dbcontext.SaveChanges();
        }

        public Contest Find(int Id)
        {
            var contest = dbcontext.Contests.FirstOrDefault(contest => contest.contestId == Id);
            return contest;
        }

        public IList<Contest> List()
        {
            return dbcontext.Contests.ToList();
        }

        public void Remove(int Id)
        {
            var contest = Find(Id);
            if (contest != null)
            {
                dbcontext.Contests.Remove(contest);
                Commit();
            }
        }


        public void Update(Contest newContest)
        {
            var contest = Find(newContest.contestId);
            contest.contestDuration = newContest.contestDuration;
            contest.contestTitle = newContest.contestTitle;
            contest.contestVisabilty = newContest.contestVisabilty;
            Commit();
          
        }
        private void LoadCurrentContest(Contest contest)
        {
            dbcontext.Entry(contest).Collection(c => c.ContestProblems).Load();
            dbcontext.Entry(contest).Collection(c => c.UserContest).Load();
            dbcontext.Entry(contest).Collection(c => c.Submissions).Load();
            dbcontext.Entry(contest).Reference(c => c.group).Load(); 
        }
    }
}
