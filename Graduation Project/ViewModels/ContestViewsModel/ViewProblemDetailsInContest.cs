﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraduationProject.ViewModels.ProblemViewsModel;

namespace GraduationProject.ViewModels.ContestViewsModel
{
    public class ViewProblemDetailsInContest: ViewProblemDetails
    {
        public int? groupId { get; set; }
        public int contestId { get; set; }

        public string contestTitle { get; set; }
        public DateTime contestStartTime { get; set; }
        public int contestDuration { get; set; }
        public Boolean IsCurrentUserOwner { get; set; }
    }
}
