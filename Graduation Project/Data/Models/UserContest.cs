using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.Data.Models
{
    public class UserContest
    {
        public int UserId { get; set; }
        public int ContestId { get; set; }
        public Boolean isFavourite { get; set; }
        public Boolean isOwner { get; set; }
        public Boolean isRegistered { get; set; }
        public virtual User User { get; set; }
        public virtual Contest Contest { get; set; }

    }
}
