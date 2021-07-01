using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.Data.Models
{
    public class Comment
    {
        public Comment()
        {
            CommentVotes = new HashSet<commentVote>();
        }
        public int commentId { get; set; }
        [MaxLength(50)]
        public string content { get; set; }
        public int upvote { get; set; }
        public int downvote { get; set; }
        public DateTime creationTime { get; set; }
        public int blogId { get; set; }
        public Blog blog { get; set; }
        public virtual ICollection<commentVote> CommentVotes { get; set; }
    }
}
