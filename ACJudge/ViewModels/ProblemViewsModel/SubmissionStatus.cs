namespace ACJudge.ViewModels.ProblemViewsModel;

public class SubmissionStatus
{
    public string Verdict { get; set; } = "";
    public string Time { get; set; } = "";
    public string Space { get; set; } = "";

    public SubmissionStatus()
    {
        
    }
    public SubmissionStatus(string verdict, string time, string space)
    {
        Verdict = verdict;
        Time = time;
        Space = space;
    }
}