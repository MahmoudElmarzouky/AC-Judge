using GraduationProject.Data.Models;
using GraduationProject.ViewModels.ProblemViewsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.ViewModels.User
{
    public class FavoriteViewModel
    {
        public IEnumerable<ViewProblemModel> pu { get; set; }
        public IEnumerable<UserContest> uc { get; set; }
        public IEnumerable<UserGroup> ug { get; set; }
        public IEnumerable<UserBlog> ub { get; set; }

    }
}
