using GraduationProject.Data.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.Data.Repositories.APIInterfaces
{
    interface IAPI<T>
    {
        ProblemIfo GetProblem(string OnlineJudge,string ProblemId);
        string GetVerdict(string OnlineJudge,string ProblemId,string solution);
    }
}
