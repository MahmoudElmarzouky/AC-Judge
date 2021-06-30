using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.ViewModels
{
    public class ViewGroupModel
    {
        public int GroupId { get; set; }
        public string GroupTitle { get; set; }
        public string GroupDescription { get; set; }
        public int NumberOfMembers { get; set; }
        public string OwnerName { get; set; }
        public int NumberOfUpCommingContests { get; set; }
        public int NumberOfRunningContests { get; set; }
        public int NumberOfEndedContests { get; set; }
    }
}
