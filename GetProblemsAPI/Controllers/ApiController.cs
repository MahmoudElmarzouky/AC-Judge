using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace GetProblemsAPI.Controllers;
[ApiController]
[Route("[controller]")]
public class ApiController : ControllerBase
{
    private readonly ISelinum _driver;
    public ApiController(ISelinum driver)
    {
        _driver = driver;
    }
    
    [HttpGet(Name = "GetProblemInfo")]
    public ActionResult<ProblemInfo> GetCodeForcesProblem(string contestId, string problemId)
    {
        return _driver.GetCodeForcesProblem(contestId, problemId);
    }
}
