using GraduationProject.Data.API;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

public static class APi
{
    public static ProblemIfo GetProblem(string OnlineJudge, string Contestid,string charproblemm)
    {
        ProblemIfo problemIfo = new ProblemIfo();
        if (OnlineJudge == "CodeForces")
        {
            
            string url = "http://157.90.173.120/cf/" + Contestid + "/" + charproblemm;
            string json = GetUrlToString(url);
            problemIfo = JsonConvert.DeserializeObject<ProblemIfo>(json);
            if (problemIfo.problem == null) return null;
            return problemIfo;

        }
        return problemIfo;
    }
    internal static string GetUrlToString(string url)
    {
        String Response = null;

        try
        {
            using (WebClient client = new WebClient())
            {
                Response = client.DownloadString(url);
            }
        }
        catch (Exception)
        {
            return null;
        }

        return Response;
    }
    public static string GetVerdict(string OnlineJudge, string ProblemId, string solution)
    {
        throw new NotImplementedException();
    }
}

