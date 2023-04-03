using System;

namespace ACJudge.Data.API;

public class CodeForcesGetProblemApi: IGrabProblem
{
    public ProblemInfo GetProblem(string url)
    {
        var requestData = Request.GetRequestData(url);
        // [TODO] Implement the method 
        throw new NotImplementedException();
    }

    private ProblemInfo get()
    {
        /*
        var url = "http://95.216.185.187/cf/" + contestId + "/" + problemIndex;
        var json = GetPageContent(url);
        var problemInfo = JsonConvert.DeserializeObject<ProblemIfo>(json);
        return problemInfo.Problem != null? problemInfo: null;
        */
        return null;
    }
}