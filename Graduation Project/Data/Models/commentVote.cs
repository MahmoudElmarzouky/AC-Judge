using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.Data.Models
{
    public class commentVote
    {
        [Key, Column(Order = 0)]
        public int commentId { get; set; }
        [Key, Column(Order = 1)]
        public int userId { get; set; }
        public int value { get; set; }
        public Boolean isFavourite { get; set; }
        public User User { get; set; }
        public Comment Comment { get; set; }
    }
}
