using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.ViewModels.GroupViewsModel
{
    public class EditGroupModel
    {
        public int groupId { get; set; }
        public string groupTitle { get; set; }
        public string groupDescription { get; set; }
        public string oldPassword { get; set; }
        public string newPassword { get; set; }
        public Boolean Visable { get; set; }
        public string visable { get; set; }
    }
}
