using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.Data.Models
{
    public class Contest
    {
        public Contest()
        {
            UserContest = new HashSet<UserContest>();
            Submissions = new HashSet<Submission>();
            ContestProblems = new HashSet<ContestProblem>(); 
        }
        public int contestId { get; set; }
        public string contestTitle { get; set; }
        public DateTime contestStartTime { get; set; }
        public int contestDuration { get; set; }
        public DateTime creationTime { get; set; }
        public string contestVisabilty { get; set; }
        public virtual ICollection<UserContest> UserContest { get; set; }
        public virtual ICollection<Submission> Submissions { get; set; }
        public virtual ICollection<ContestProblem> ContestProblems { get; set; }
        public int? groupId { get; set; }
        public Boolean InGroup { get { return groupId != null;  } set { } }
        public string Password { get; set; } = "";
        public int contestStatus
        {
            get
            {
                // -1 upcomming 
                // 0 running 
                // 1 ended 
                if (contestStartTime > DateTime.Now)
                    return -1;
                if (contestStartTime.AddMinutes(contestDuration) < DateTime.Now)
                    return 1;
                return 0;
            }
            set { }
        }
    }
}
