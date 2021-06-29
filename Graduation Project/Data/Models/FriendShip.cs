using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.Data.Models
{
    public class FriendShip
    {
        public int UserId1 { get; set; }
        public User user1 { get; set; }
        public int UserId2 { get; set; }
        public User user2 { get; set; }
    }
}
