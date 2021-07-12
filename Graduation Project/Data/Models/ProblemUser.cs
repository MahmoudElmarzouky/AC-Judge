using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.Data.Models
{
    public class ProblemUser
    {
        [Key, Column(Order = 0)]
        public int UserId { get; set; }

        [Key, Column(Order = 1)]
        public int ProblemId { get; set; }
        public Problem problem { get; set; }

        [Key, Column(Order = 2)]
        public Boolean IsFavourite { get; set; }

    }
}
