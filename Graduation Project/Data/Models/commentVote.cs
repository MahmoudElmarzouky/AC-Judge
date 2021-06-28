using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.Data.Models
{
    public class commentVote
    {
        public int commentId { get; set; }
        public int userId { get; set; }
        public int value { get; set; }
        public Boolean isFavourite { get; set; }
        public User user { get; set; }
        public Comment comment { get; set; }
    }
}
