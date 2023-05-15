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
    private static int _userCount = 1;
    private readonly int _currentUser;
    public SubmitCodeController(ISelinum driver)
    {
        _currentUser = _userCount++;
        _driver = driver;
    }
    [HttpGet]
    public async Task<ActionResult<SubmissionStatus>> Submit(string problemName, string code, string language)
    {
        try
        {
            _driver.Login();
            return await _driver.Submit(problemName, code, language, _currentUser.ToString());
        }
        catch
        {
            return new SubmissionStatus();
        }
    }
}