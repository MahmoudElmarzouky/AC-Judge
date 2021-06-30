using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraduationProject.Data.Models;

namespace GraduationProject.ViewModels
{
    public class ViewGroupModel
    {
        public ViewGroupModel()
        {
            Contests = new HashSet<Contest>();
            UserGroup = new HashSet<UserGroup>(); 
        }
        public int GroupId { get; set; }
        public string GroupTitle { get; set; }
        public string GroupDescription { get; set; }
        public int NumberOfMembers { get; set; }
        public string UserRole { get; set; }
        public DateTime creationTime { get; set; }
        public int NumberOfUpCommingContests { get; set; }
        public int NumberOfRunningContests { get; set; }
        public int NumberOfEndedContests { get; set; }
        public string GroupStatus { get; set; }
        public virtual ICollection<Contest> Contests { get; set; }
        public virtual ICollection<UserGroup> UserGroup { get; set; }

    }
}
