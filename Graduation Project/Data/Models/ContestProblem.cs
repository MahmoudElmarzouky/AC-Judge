using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.Data.Models
{
    public class ContestProblem
    {
        public int contestId { get; set; }
        public int problemId { get; set; }
        public int order { get; set; } // order the problem in contest
        public string PlatForm { get; set; }
        public string Alias { get; set; }
        public string ProblemSourceId { get; set; }
        public Contest contest { get; set; }
        public Problem  problem { get; set; }
    }
}
