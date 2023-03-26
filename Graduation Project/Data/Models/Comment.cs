using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GraduationProject.Data.Models
{
    public class Comment
    {
        public Comment()
        {
            CommentVotes = new HashSet<CommentVote>();
        }
        public int commentId { get; set; }
        [MaxLength(50)]
        public string content { get; set; }
        public int upvote { get; set; }
        public int downvote { get; set; }
        public DateTime creationTime { get; set; }
        public int blogId { get; set; }
        public Blog blog { get; set; }
        public ICollection<CommentVote> CommentVotes { get; set; }
    }
}
