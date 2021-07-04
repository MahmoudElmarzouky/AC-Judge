using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.ViewModels
{
    public class CreateGroupModel
    {
        public int OwnerId { get; set; }
        public int GroupId { get; set; }
        public string GroupTitle { get; set; }
        public string GroupDescription { get; set; }
        public Boolean Visable { get; set; }
        public string visable { get; set; }
        public string Password { get; set; }

    }
}
