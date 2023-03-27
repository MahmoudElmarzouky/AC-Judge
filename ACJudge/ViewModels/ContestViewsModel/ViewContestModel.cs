using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACJudge.Data.Models;

namespace ACJudge.ViewModels.ContestViewsModel
{
    public class ProblemInfo
    {
        public int ProblemId { get; set; }
        public string Origin { get; set; }
        public string originUrl { get; set; }
        public string PropblemTitle { get; set; }
        public Boolean Solved { get; set; }
        public Boolean HasAttempt { get; set; }
        public string SolvedClass { get { return Solved ? "bg-success" : HasAttempt? "bg-danger": "unsolved"; } set { } }
        public int NumberOfAccepted { get; set; }
        public int NumberOfSubmissions { get; set; }
        public string OriginName { get; set; }

    }
    public class ViewContestModel
    {
        public ViewContestModel()
        {
            UserContest = new HashSet<UserContest>();
            Submissions = new HashSet<Submission>();
            Problems = new List<ProblemInfo>();
        }
        public Boolean IsFavourite { get; set; }
        public Boolean IsCurrentUserOwner { get; set; }

        public string FavouuriteClass { get {
                return IsFavourite ? "fas fa-heart active" : "fas fa-heart"; 
            } set { } }
        public int contestId { get; set; }
        public int currentUserId { get; set; }
        public string contestTitle { get; set; }
        public DateTime contestStartTime { get; set; }
        public int contestDuration { get; set; }
        public DateTime creationTime { get; set; }
        public string contestVisabilty { get; set; }
        public string VisableClass { get { return contestVisabilty == "Public" ? "fas fa-users" : "fas fa-users-slash"; } set { } }
        public string contestStatus { get; set; } // upcoming, running, ended 
        public string contestStatusClass { get {
                switch (contestStatus)
                {
                    case "Upcoming":
                        return "scheduled"; 
                    case "Running":
                        return "running";
                    case "Ended":
                        return "ended"; 
                }
                return ""; 
            } set { } }
        public int PreparedById { get; set; }
        public string PreparedBy { get; set; }
        public  ICollection<UserContest> UserContest { get; set; }
        public  ICollection<Submission> Submissions { get; set; }
        public  IList<ProblemInfo> Problems { get; set; }
        public int? groupId { get; set; }
        public string GlobalTime { get
            {
                return string.Format("https://www.timeanddate.com/worldclock/fixedtime.html?day={0}&month={1}&year={2}&hour={3}&min={4}&sec={5}&p1=166", contestStartTime.Day, contestStartTime.Month, contestStartTime.Year, contestStartTime.Hour, contestStartTime.Minute, contestStartTime.Second);
            }
            set { } }
        public ContestFilter Filter { get; set; }

    }
}
