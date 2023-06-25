using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ACJudge.Data.Models
{
    public sealed class Comment
    {
        public Comment()
        {
            
        }

        public Comment(string content, int blogId, int creatorId)
        {
            Content = content;
            BlogId = blogId;
            Upvote = DownVote = 0;
            CommentVotes.Add(new CommentVote(creatorId));
        }

        
        public int CommentId { get; set; }
        [MaxLength(50)]
        public string Content { get; set; }
        public int Upvote { get; set; }
        public int DownVote { get; set; }
        public DateTime CreationTime { get; set; } = DateTime.Now;
        public int BlogId { get; set; }
        public Blog Blog { get; set; }
        public ICollection<CommentVote> CommentVotes { get; set; } = new HashSet<CommentVote>();
    }
}
