using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.Data.Repositories.APIInterfaces
{
    interface ICodeForeces<T>
    {
        T GetProblem(string OnlineJudge,string ProblemId);
        string GetVerdict(string OnlineJudge,string ProblemId,string solution);
    }
}
