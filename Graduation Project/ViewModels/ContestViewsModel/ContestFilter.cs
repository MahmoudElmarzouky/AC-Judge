using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.ViewModels.ContestViewsModel
{
    public class ContestFilter
    {
        public string contestTitle { get; set; } = "";
        public string PrepeardBy { get; set; } = "";
        public string ContestStatus { get; set; } = "";
        public string ContestType { get; set; } = "";
        public string ContestX { get; set; } = "";
        public string ContestPrivacy { get; set; } = "";
        public string Reset { get; set; }
        public int userId { get; set; }
    }
}
