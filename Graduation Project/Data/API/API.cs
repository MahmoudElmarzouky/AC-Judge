using GraduationProject.Data.API;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

public static class APi
{
    public static ProblemIfo GetProblem(string OnlineJudge, string ProblemId)
    {
        ProblemIfo problemIfo = new ProblemIfo();
        if (OnlineJudge == "CodeForces")
        {
            Boolean flag = true;
            string id = "";
            string c = "";
            foreach (var item in ProblemId)
            {
                if (Char.IsLetter(item))
                {
                    flag = false;
                    c += item;
                    continue;
                }
                _ = (flag == true) ? id += item : c += item;
            }
            string url = "http://157.90.173.120/cf/" + id + "/" + c;
            string json = GetUrlToString(url);
            if (json == null) return null;
            problemIfo = JsonConvert.DeserializeObject<ProblemIfo>(json);

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

