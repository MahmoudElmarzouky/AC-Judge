using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACJudge.Data.Models
{
    public class UserBlog
    {
        public UserBlog()
        {
            
        }

        public UserBlog(int userId)
        {
            UserId = userId;
            IsFavourite = false;
            VoteValue = 0;
            BlogOwner = true;
        }
        [Key, Column(Order = 0)]
        public int UserId { get; set; }
        [Key, Column(Order = 1)]
        public int BlogId { get; set; }
        public bool IsFavourite { get; set; }
        public int VoteValue { get; set; }
        public bool BlogOwner { get; set; } 
        public virtual User User { get; set; }
        public virtual Blog Blog { get; set; }
    }
}
