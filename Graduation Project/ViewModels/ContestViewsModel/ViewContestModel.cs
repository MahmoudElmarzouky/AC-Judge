using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraduationProject.Data.Models;

namespace GraduationProject.ViewModels.ContestViewsModel
{
    public class ViewContestModel
    {
        public ViewContestModel()
        {
            UserContest = new HashSet<UserContest>();
            Submissions = new HashSet<Submission>();
            Problems = new HashSet<Problem>();
        }
        public int contestId { get; set; }
        public string contestTitle { get; set; }
        public DateTime contestStartTime { get; set; }
        public int contestDuration { get; set; }
        public DateTime creationTime { get; set; }
        public string contestVisabilty { get; set; }
        public string contestStatus { get; set; } // upcoming, running, ended 
        public  ICollection<UserContest> UserContest { get; set; }
        public  ICollection<Submission> Submissions { get; set; }
        public  ICollection<Problem> Problems { get; set; }
        public int? groupId { get; set; }
    }
}
