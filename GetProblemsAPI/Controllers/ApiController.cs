using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace GetProblemsAPI.Controllers;
[ApiController]
[Route("[controller]")]
public class ApiController : ControllerBase
{
    public ApiController()
    {
        
    }
    [HttpPut(Name = "GetStatus")]
    public void Index()
    {
       
    }
    [HttpGet(Name = "GetProblemInfo")]
    public ActionResult<ProblemInfo> GetCodeForcesProblem(string contestId, string problemId)
    {
        var url = $"https://www.codeforces.com/problemset/problem/{contestId}/{problemId}";
        using var driver = new ChromeDriver();
        driver.Navigate().GoToUrl(url);
        var title = driver.FindElement(By.ClassName("title")).Text;
        var pageHtml = driver.FindElement(By.ClassName("problem-statement")).
            GetAttribute("outerHTML");
        var tags = driver.FindElements(By.ClassName("tag-box"))
            .Select(e => e.Text).ToList();
        var rate = 0;
        if (tags.Any(e => e.StartsWith("*")))
        {
            rate = int.Parse(tags.First(t => t.StartsWith("*")).Substring(1));
        }

        Console.WriteLine($"Rate is{rate}");
        var p = new ProblemInfo {
            Problem = pageHtml,
            Rate = rate,
            ProblemId = $"{contestId}{problemId}",
            Source = "Codeforces",
            Tags = tags,
            Title = title
        };
        driver.Close();
        return p;
    }
}
