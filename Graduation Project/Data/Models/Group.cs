using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Graduation_Project.Data.Models
{
    public class Group
    {
        public int GroupId { get; set; }
        public DateTime creationTime { get; set; }
        public string GroupDescription { get; set; }
        public Boolean Visable { get; set; }
        public string Password { get; set; }
    }
}
