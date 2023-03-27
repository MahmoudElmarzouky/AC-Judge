using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACJudge.Data.Models;
using ACJudge.ViewModels.ProblemViewsModel;

namespace ACJudge.ViewModels.User
{
    public class FavoriteViewModel
    {
        public IEnumerable<ViewProblemModel> pu { get; set; }
        public IEnumerable<UserContest> uc { get; set; }
        public IEnumerable<UserGroup> ug { get; set; }
        public IEnumerable<UserBlog> ub { get; set; }

    }
}
