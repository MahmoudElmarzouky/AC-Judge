using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACJudge.Data.Models
{
    public class ProblemUser
    {
        [Key, Column(Order = 0)]
        public int UserId { get; set; }
        [Key, Column(Order = 1)]
        public int ProblemId { get; set; }
        public Problem Problem { get; set; }
        [Key, Column(Order = 2)]
        public bool IsFavourite { get; set; }
    }
}
