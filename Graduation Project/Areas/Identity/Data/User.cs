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
        public int BirthDate { get; set; } // birth year 
        [PersonalData]
        public string Country { get; set; }
    }
}
