using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Graduation_Project.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Graduation_Project.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the User class
    public class User : IdentityUser
    {
        [PersonalData]
        public string photoUrl { get; set; }
        [PersonalData]
        public string firstName { get; set; }
        [PersonalData]
        public string lastName { get; set; }
        [PersonalData]
        public Boolean gender { get; set; }
        [PersonalData]
        public DateTime dateOfJoin { get; set; }
        [PersonalData]
        public int BirthDate { get; set; }
        [PersonalData]
        public string Country { get; set; }
        [PersonalData]
        public int MyProperty { get; set; }
        public Handle handle { get; set; }
        public userStatistics userStatistic { get; set; }
        public CodeforcesStatistics codeforcesStatistic { get; set; }
        public atcoderStatistics atcoderStatistic { get; set; }
        public virtual ICollection<Submission> submissions { get; set; }
    }
}
