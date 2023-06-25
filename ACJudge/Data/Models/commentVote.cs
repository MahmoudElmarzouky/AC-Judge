using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACJudge.Data.Models
{
    public class CommentVote
    {
        public CommentVote()
        {
            
        }

        public CommentVote(int userId)
        {
            UserId = userId;
            Value = 0;
            IsFavourite = false;
        }
        [Key, Column(Order = 0)]
        public int CommentId { get; set; }
        [Key, Column(Order = 1)]
        public int UserId { get; set; }
        public int Value { get; set; }
        public bool IsFavourite { get; set; }
        public User User { get; set; }
        public Comment Comment { get; set; }
    }
}
