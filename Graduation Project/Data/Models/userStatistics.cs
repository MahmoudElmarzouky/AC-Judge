using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Graduation_Project.Data.Models
{
    public class userStatistics
    {
        public int userStatisticsId { get; set; }
        public int SolvedCount { get; set; }
        public DateTime lastCheckSubmission { get; set; }
    }
}
