using Graduation_Project.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Graduation_Project.Data.Models
{
    public class atcoderStatistics
    {
        public int atcoderStatisticsId { get; set; }
        public int SolvedCount { get; set; }
        public DateTime lastCheckSubmission { get; set; }
        public int userId { get; set; }
        public User user { get; set; }
    }
}
