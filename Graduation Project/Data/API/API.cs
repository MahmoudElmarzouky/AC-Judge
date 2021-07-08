using GraduationProject.Data.Repositories.APIInterfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GraduationProject.Data.API
{
    public class API : IAPI<API>
    {
        public ProblemIfo GetProblem(string OnlineJudge, string ProblemId)
        {
            ProblemIfo problemIfo = new ProblemIfo();
            if (OnlineJudge == "Codefores")
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
                problemIfo = JsonConvert.DeserializeObject<ProblemIfo>(json);
                //if (problemIfo.problem == null)
                //{
                //    Console.WriteLine("NotFound");
                //}
                //else
                //{
                //    //Added in Database
                //    Console.WriteLine(problemIfo.rate);
                //}
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
        public string GetVerdict(string OnlineJudge, string ProblemId, string solution)
        {
            throw new NotImplementedException();
        }
    }
}
