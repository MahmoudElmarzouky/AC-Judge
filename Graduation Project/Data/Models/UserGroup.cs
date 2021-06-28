using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.Data.Models
{
    public class UserGroup
    {
        [Key, Column(Order = 0)]
        public int UserId { get; set; }
        [Key, Column(Order = 1)]
        public int GroupId { get; set; }
        public string UserRole { get; set; }
        public DateTime MemberSince { get; set; }
        public Boolean isFavourite { get; set; }
        public virtual User User { get; set; }
        public virtual Group Group { get; set; }
    }
}
