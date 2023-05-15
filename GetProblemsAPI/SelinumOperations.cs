using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace GetProblemsAPI;

public class SelinumOperations: ISelinum
{
    private readonly ChromeDriver _driver;
    private readonly WebDriverWait _wait;
    private const string SubmitUrl = "https://codeforces.com/problemset/submit";
    public SelinumOperations()
    {
        _driver = new ChromeDriver();
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
    }

    ~SelinumOperations() => _driver.Quit();

    public void Login()
    {
        LoadCookies();
        const string email = "TheWaterIsHot";
        const string password = "0112609288";
        const string url = "https://codeforces.com/enter";
        const string emailXpath = "//*[@id=\"handleOrEmail\"]";
        const string passwordXPath = "//*[@id=\"password\"]";
        const string loginButtonXPath = "//*[@id=\"enterForm\"]/table/tbody/tr[4]/td/div[1]/input";
        const string myProfileXPath = "//*[@id=\"header\"]/div[2]/div[2]/a[1]";
        while (true)
        {
            try
            {
                 _wait.Until(driver => driver.FindElement(By.XPath(myProfileXPath)));
            }
            catch
            {
                if (_driver.Url.Equals(url) == false)
                    _driver.Navigate().GoToUrl(url);
            
                _wait.Until(driver => driver.FindElement(By.XPath(emailXpath)));
                
                _driver.FindElement(By.XPath(emailXpath)).SendKeys(email);
                _driver.FindElement(By.XPath(passwordXPath)).SendKeys(password);
                _driver.FindElement(By.Id("remember")).Click();
                _driver.FindElement(By.XPath(loginButtonXPath)).Submit();
                
            
                CookieOperations.SaveCookies(_driver.Manage().Cookies.AllCookies);
                continue;
            }

            break;
        }
    }

    public void LoadCookies()
    {
        var cookies = CookieOperations.GetCookies();
        if (cookies.Count == 0)
        {
            Login();
            return;
        }
        _driver.Navigate().GoToUrl("https://codeforces.com/");
        foreach (var cookie in cookies)
        {
            _driver.Manage().Cookies.AddCookie(cookie);
        }
        _driver.Navigate().Refresh();
    }

    public async Task<SubmissionStatus> Submit(string problemName, string code, string language, string fileName)
    {
        
        await File.WriteAllTextAsync(fileName, code);
        var path = Directory.GetCurrentDirectory() + "/" + fileName;
        _driver.Navigate().GoToUrl(SubmitUrl);
        while (true)
        {
            try
            {
                _wait.Until(driver => driver.FindElement(By.Name("submittedProblemCode")));
                break;
            }
            catch
            {
            }
        }

        _driver.FindElement(By.Name("sourceFile")).SendKeys(path);
        _driver.FindElement(By.Name("submittedProblemCode")).SendKeys(problemName);
        var dropDown = new SelectElement(_driver.FindElement(By.Name("programTypeId")));
        dropDown.SelectByValue(language);
        _driver.FindElement(By.Id("singlePageSubmitButton")).Submit();
        
        while (true)
        {
            try
            {
                _wait.Until(driver=>driver.FindElement(By.ClassName("status-frame-datatable")));
                break;
            }
            catch
            {
            }
        }
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
            _driver.Close();
            _driver.Quit();
            return new SubmissionStatus(verdict, time, space);
        }
    }

    public ProblemInfo GetCodeForcesProblem(string contestId, string problemId)
    {
        var url = $"https://www.codeforces.com/problemset/problem/{contestId}/{problemId}";
        _driver.Navigate().GoToUrl(url);
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
        return p;
    }
}
