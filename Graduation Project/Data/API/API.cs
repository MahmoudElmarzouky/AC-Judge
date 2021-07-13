using GraduationProject.Data.API;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

public static class APi
{
    public static ProblemIfo GetProblem(string OnlineJudge, string Contestid, string charproblemm)
    {
        ProblemIfo problemIfo = new ProblemIfo();
        if (OnlineJudge == "CodeForces")
        {

            string url = "http://95.216.185.187/cf/" + Contestid + "/" + charproblemm;
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
    public static SubmitInfo GetVerdict(string ProblemId, string solution, string Language)
    {
        string URI = "http://95.216.185.187/SUBMIT";
        //StreamWriter sw = new StreamWriter("C:/Users/Elhabashy/Desktop/new.txt");
        //sw.WriteLine(solution);
        //sw.Close();
        WebClient wc = new WebClient();

        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(solution);
        var x= System.Convert.ToBase64String(plainTextBytes);
        wc.QueryString.Add("ProblemId", ProblemId);
        wc.QueryString.Add("SubmitText", x);
        wc.QueryString.Add("Language", "GNU G++17 7.3.0");

        var data = wc.UploadValues(URI, "POST", wc.QueryString);

        var responseString = Encoding.Default.GetString(data);
        //var responseString = UnicodeEncoding.UTF8.GetString(data);
        SubmitInfo submitInfo = null;
        try
        {

            submitInfo = System.Text.Json.JsonSerializer.Deserialize<SubmitInfo>(responseString);
        }
        catch
        {

        }

        return submitInfo;
    }
}

