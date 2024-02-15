using System;
using System.Net;
using System.Net.Http;

namespace ACJudge.Data.API;

public class Request
{
    public static string GetRequestData(string url)
    {
        var content = "";
        try
        {
            using var client = new HttpClient();
            content = "";
            //client.DownloadString(url);
        }
        catch (Exception)
        {
            return "";
        }
        return content;
    }
}