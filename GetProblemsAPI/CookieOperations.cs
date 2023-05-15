using System.Collections.ObjectModel;
using OpenQA.Selenium;

namespace GetProblemsAPI;

public static class CookieOperations
{
    private const string Delimiter = "!@#$%";
    private const string CookieFileName = "cookie.txt";
    public static void SaveCookies(ReadOnlyCollection<Cookie> cookies)
    {
        var cookiesString = "";
        
        foreach (var cookie in cookies)
        {
            cookiesString += cookie.Name + Delimiter + cookie.Value + Delimiter +
                             cookie.Domain + Delimiter + cookie.Path + Delimiter + cookie.Expiry + Delimiter +
                             cookie.Secure + Delimiter + cookie.IsHttpOnly + Delimiter + cookie.SameSite +
                             "\n";
        }
        File.WriteAllText(CookieFileName, cookiesString);
    }

    public static Collection<Cookie> GetCookies()
    {
        Collection<Cookie> cookies = new();
        try
        {
            var lines = File.ReadLines(CookieFileName);
            foreach (var line in lines)
            {
                var parts = line.Split(Delimiter);
                var expire = DateTime.Now.AddDays(2);
                if (parts[4].Equals("") == false)
                {
                    expire = DateTime.Parse(parts[4]);
                }

                var currentCookie = new Cookie(parts[0], parts[1], parts[2],
                    parts[3], expire, parts[5].Equals("True"),
                    parts[6].Equals("True"),
                    parts[7]);
                cookies.Add(currentCookie);
            }

            return cookies;
        }
        catch
        {
            return cookies;
        }
    }
}