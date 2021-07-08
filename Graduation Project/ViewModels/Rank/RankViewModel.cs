using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.ViewModels.Rank
{
    public class RankViewModel
    {
        public int userid { get; set; }
        public int Birthyear { get; set; }
        public string Country { get; set; }
        public string UserName { get; set; }
        public int TotalSolved { get; set; }
    }
}
