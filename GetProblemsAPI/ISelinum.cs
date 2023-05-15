namespace GetProblemsAPI;

public interface ISelinum
{
    void Login();
    void LoadCookies();
    SubmissionStatus Submit(string problemName, string code, string language);
    ProblemInfo GetCodeForcesProblem(string contestId, string problemId);
}