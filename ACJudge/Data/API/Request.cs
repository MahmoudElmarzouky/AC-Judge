using System;
using System.Net;

namespace ACJudge.Data.API;

public class Request
{
    public static string GetRequestData(string url)
    {
        var content = "";
        try
        {
            using var client = new WebClient();
            content = client.DownloadString(url);
        }
        catch (Exception)
        {
            return "";
        }
        return content;
    }
}