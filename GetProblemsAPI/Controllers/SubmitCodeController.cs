using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.IO;

namespace GetProblemsAPI.Controllers;
[ApiController]
[Route("[Controller]")]
public class SubmitCodeController : Controller
{
    
    private readonly ISelinum _driver;
    public SubmitCodeController(ISelinum driver)
    {
        _driver = driver;
    }
    [HttpGet]
    public ActionResult<SubmissionStatus> Submit(string problemName, string code, string language)
    {
        try
        {
            
            _driver.Login();
            return _driver.Submit(problemName, code, language);
        }
        catch
        {
            return new SubmissionStatus();
        }
    }
}