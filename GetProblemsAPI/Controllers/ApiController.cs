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
        var pageHtml = driver.PageSource;
        var p = new ProblemInfo {
            Problem = pageHtml,
            Rate = 1900,
            ProblemId = $"{contestId}{problemId}",
            Source = "Codeforces",
            Tags = new List<string>{ "dp", "binary search", "greedy" },
            Title = title
        };
        driver.Close();
        return p;
    }
}
