using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Graduation_Project.Data.Models
{
    public class Contest
    {
        public Contest()
        {
            UserContest = new HashSet<UserContest>(); 
        }
        public int contestId { get; set; }
        public string contestTitle { get; set; }
        public DateTime contestStartTime { get; set; }
        public int contestDuration { get; set; }
        public DateTime creationTime { get; set; }
        public string contestVisabilty { get; set; }
        public virtual ICollection<UserContest> UserContest { get; set; }
        public virtual ICollection<Submission> Submissions { get; set; }
        public int groupId { get; set; }
        public Group group { get; set; }
        public virtual ICollection<ContestProblem> ContestProblems { get; set; }
    }
}
