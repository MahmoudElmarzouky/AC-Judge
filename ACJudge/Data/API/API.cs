using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ACJudge.ViewModels.ProblemViewsModel;
using GetProblemsAPI.Controllers;
using Newtonsoft.Json;
using X.PagedList;

namespace ACJudge.Data.API;

public static class APi
{
    private static async Task<string> _getResponse(string url)
    {
        var client = new HttpClient();
        var response = await client.GetAsync(url);
        return await response.Content.ReadAsStringAsync();
    }
    
    public static async Task<ProblemInfo> GetProblem(string onlineJudge, string contestId, string problemIndex)
    {
        var url = $@"https://localhost:7222/Api?contestId={contestId}&problemId={problemIndex}";
        var responseContent = await _getResponse(url);
        var problem = JsonConvert.DeserializeObject<ProblemInfo>(responseContent);
        return problem;
    }
    /*
     * problemName -> 2A, 54B, [contestId + problemIndex]
     * solution -> code to submit
     * language -> a number for example 50-> c++ , you can see the mapping in codeforces submit page on select values
     */
    public static async Task<SubmissionStatus> GetVerdict(string problemName, string solution, 
        string language)
    {
        var encodedSolution = Uri.EscapeDataString(solution);
        var url = $"https://localhost:7222/SubmitCode?problemName={problemName}&code={encodedSolution}&language={language}";
        var responseContent = await _getResponse(url);
        var submitStatus = JsonConvert.DeserializeObject<SubmissionStatus>(responseContent);
        return submitStatus;
    }
}