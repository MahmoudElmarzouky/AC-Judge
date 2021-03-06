using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.Data.Models
{
    public class User
    {
        public User()
        {
            UserContest = new HashSet<UserContest>();
            UserGroup = new HashSet<UserGroup>();
            submissions = new HashSet<Submission>();
            ProblemUsers = new HashSet<ProblemUser>();
            userBlog = new HashSet<UserBlog>();
        }
        public string UserIdentityId { get; set; }
        public int UserId { get; set; }
        public int BirthDate { get; set; } // birth year 
        public string Country { get; set; }
        public string UserName { get; set; }
        public string PhotoUrl { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Boolean Gender { get; set; }
        public DateTime DateOfJoin { get; set; }
        public virtual ICollection<UserContest> UserContest { get; set; }
        public virtual ICollection<UserGroup> UserGroup { get; set; }
        public Handle handle { get; set; }
        public userStatistics userStatistic { get; set; }
        public CodeforcesStatistics codeforcesStatistic { get; set; }
        public atcoderStatistics atcoderStatistic { get; set; }
        public virtual ICollection<Submission> submissions { get; set; }
        public virtual ICollection<UserBlog> userBlog { get; set; }
        public virtual ICollection<commentVote> CommentVotes { get; set; }
        public virtual ICollection<ProblemUser> ProblemUsers { get; set; }
        public int TotalNumberOfSolvedProblems { get {
                int userCount = userStatistic == null? 0 :userStatistic.SolvedCount;
                int cfCount = codeforcesStatistic == null? 0 :codeforcesStatistic.SolvedCount;
                int atcoderCount = atcoderStatistic == null? 0: atcoderStatistic.SolvedCount;
                return userCount + cfCount + atcoderCount; 
            } set { } }

    }
}
