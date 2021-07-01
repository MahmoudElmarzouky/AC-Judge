using GraduationProject.Data.Models;
using GraduationProject.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.Data.Repositories
{
    public class atcoderStatisticsDbRepository : IRepository<atcoderStatistics>
    {
        readonly private EntitiesContext dbcontext;
        public atcoderStatisticsDbRepository(EntitiesContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public atcoderStatistics Add(atcoderStatistics newatcoderStatistics)
        {
            dbcontext.Add(newatcoderStatistics);
            Commit();
            return newatcoderStatistics;
        }

        public void Commit()
        {
            dbcontext.SaveChanges();
        }

        public atcoderStatistics Find(int Id)
        {
            var atcoderstatistics = dbcontext.atcoderStatistics.FirstOrDefault(atcoder => atcoder.atcoderStatisticsId == Id);
            return atcoderstatistics;
        }

        public IList<atcoderStatistics> List()
        {
            return dbcontext.atcoderStatistics.ToList();
        }

        public void Remove(int Id)
        {
            var atcoder = Find(Id);
            if (atcoder != null)
            {
                dbcontext.atcoderStatistics.Remove(atcoder);
                Commit();
            }
        }

       

        public void Update(atcoderStatistics newAtcoderStatistics)
        {
            dbcontext.atcoderStatistics.Update(newAtcoderStatistics);
            Commit();
        }
    }
}
