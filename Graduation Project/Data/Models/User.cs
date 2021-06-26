using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Graduation_Project.Data.Models
{
    public class User
    {
        public User()
        {
            UserContest = new HashSet<UserContest>();
            UserGroup = new HashSet<UserGroup>();
        }
        public int UserId { get; set; }
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


    }
}
