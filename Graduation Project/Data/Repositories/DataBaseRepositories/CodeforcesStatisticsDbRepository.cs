using GraduationProject.Data.Models;
using GraduationProject.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.Data.Repositories.DataBaseRepositories
{
    public class CodeforcesStatisticsDbRepository : IRepository<CodeforcesStatistics>
    {
        readonly private EntitiesContext dbcontext;
        public CodeforcesStatisticsDbRepository(EntitiesContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public CodeforcesStatistics Add(CodeforcesStatistics newCodeforcesStatistics)
        {
            dbcontext.Add(newCodeforcesStatistics);
            Commit();
            return newCodeforcesStatistics;
        }

        public void Commit()
        {
            dbcontext.SaveChanges();
        }

        public CodeforcesStatistics Find(int Id)
        {
            var codeforcesStatistics = dbcontext.CodeforcesStatistics.FirstOrDefault(codeforces=>codeforces.CodeforcesStatisticsId==Id);
            return codeforcesStatistics;
        }

        public IList<CodeforcesStatistics> List()
        {
           return   dbcontext.CodeforcesStatistics.ToList();
        }

        public void Remove(int Id)
        {
            var codeforcesStatistics = Find(Id);
            if (codeforcesStatistics != null)
            {
                dbcontext.CodeforcesStatistics.Remove(codeforcesStatistics);
                Commit();
            }
        }


        public void Update(CodeforcesStatistics newCodeforcesStatistics)
        {
            dbcontext.CodeforcesStatistics.Update(newCodeforcesStatistics);
            Commit();
        }
    }
}
