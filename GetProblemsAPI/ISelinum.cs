namespace GetProblemsAPI;

public interface ISelinum
{
    void Login();
    bool LoadCookies();
    Task<SubmissionStatus> Submit(string problemName, string code, string language, string fileName);
    ProblemInfo GetCodeForcesProblem(string contestId, string problemId);
}