using System.Linq.Expressions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace GetProblemsAPI;

public class SeleniumOperations: ISelinum
{
    private readonly ChromeDriver _driver;
    private readonly WebDriverWait _wait;
    private const string SubmitUrl = "https://codeforces.com/problemset/submit";
    private const string LoginUrl = "https://codeforces.com/enter";
    private const string EmailInputXPath = "//*[@id=\"handleOrEmail\"]";
    private const string PasswordInputXPath = "//*[@id=\"password\"]";
    private const string LoginButtonXPath = "//*[@id=\"enterForm\"]/table/tbody/tr[4]/td/div[1]/input";
    private const string MyProfileHrefXPath = "//*[@id=\"header\"]/div[2]/div[2]/a[1]";
    private const string Email = "TheWaterIsHot";
    private const string Password = "0112609288";
    
    public SeleniumOperations()
    {
        _driver = new ChromeDriver();
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
    }
    public void Login()
    {
        if (LoadCookies())
            return;
        while (true)
        {
            try
            {
                 _wait.Until(driver => driver.FindElement(By.XPath(MyProfileHrefXPath)));
            }
            catch
            {
                _goTo(LoginUrl);
            
                _wait.Until(driver => driver.FindElement(By.XPath(EmailInputXPath)));
                
                _driver.FindElement(By.XPath(EmailInputXPath)).SendKeys(Email);
                _driver.FindElement(By.XPath(PasswordInputXPath)).SendKeys(Password);
                _driver.FindElement(By.Id("remember")).Click();
                _driver.FindElement(By.XPath(LoginButtonXPath)).Submit();
                
                CookieOperations.SaveCookies(_driver.Manage().Cookies.AllCookies);
                continue;
            }
            break;
        }
    }

    private void _goTo(string url)
    {
        while (_driver.Url.Equals(url) == false)
            _driver.Navigate().GoToUrl(url);
    }
    public bool LoadCookies()
    {
        var cookies = CookieOperations.GetCookies();
        if (cookies.Count == 0)
            return false;
        _goTo("https://codeforces.com/");
        foreach (var cookie in cookies)
        {
            _driver.Manage().Cookies.AddCookie(cookie);
        }
        _driver.Navigate().Refresh();
        return true;
    }
    private void _waitObjectToLoad(By accessType)
    {
        while (true)
        {
            try
            {
                _wait.Until(driver => driver.FindElement(accessType));
                break;
            }
            catch
            {
                // ignored
            }
        }
    }
    public async Task<SubmissionStatus> Submit(string problemName, string code, string language, string fileName)
    {
        await File.WriteAllTextAsync(fileName, code);
        var path = Directory.GetCurrentDirectory() + "/" + fileName;
        _goTo(SubmitUrl);
        _waitObjectToLoad(By.Name("submittedProblemCode"));

        _driver.FindElement(By.Name("sourceFile")).SendKeys(path);
        _driver.FindElement(By.Name("submittedProblemCode")).SendKeys(problemName);
        var dropDown = new SelectElement(_driver.FindElement(By.Name("programTypeId")));
        dropDown.SelectByValue(language);
        _driver.FindElement(By.Id("singlePageSubmitButton")).Submit();

        _waitObjectToLoad(By.ClassName("status-frame-datatable"));
        
        File.Delete(fileName);
        var firstRow = _driver.
            FindElement(By.ClassName("status-frame-datatable")).
            FindElements(By.TagName("tr"))[1];
        while (true)
        {
            var list = firstRow.FindElements(By.TagName("td"));
            var size = list.Count;
            var verdict = list[size-3].Text;
            var time = list[size - 2].Text;
            var space =  list[size - 1].Text;
            if (verdict.Contains("Running") || verdict.Contains("queue"))
                continue;
            _finish();
            return new SubmissionStatus(verdict, time, space);
        }
    }
    private void _finish()
    {
        _driver.Close();
        _driver.Quit();
    }
    public ProblemInfo GetCodeForcesProblem(string contestId, string problemId)
    {
        var url = $"https://www.codeforces.com/problemset/problem/{contestId}/{problemId}";
        _goTo(url);
        var title = _driver.FindElement(By.ClassName("title")).Text;
        var pageHtml = _driver.FindElement(By.ClassName("problem-statement")).
            GetAttribute("outerHTML");
        var tags = _driver.FindElements(By.ClassName("tag-box"))
            .Select(e => e.Text).ToList();
        var rate = 0;
        if (tags.Any(e => e.StartsWith("*")))
        {
            rate = int.Parse(tags.First(t => t.StartsWith("*")).Substring(1));
        }
        var p = new ProblemInfo {
            Problem = pageHtml,
            Rate = rate,
            ProblemId = $"{contestId}{problemId}",
            Source = "Codeforces",
            Tags = tags,
            Title = title
        };
        _finish();
        return p;
    }
}
