namespace GetProblemsAPI;

public class ProblemInfo
{
    public string Title { get; set; }
    public string Source { get; set; }
    public string ProblemId { get; set; }
    public int Rate { get; set; }
    public IList<string> Tags { get; set; }
    public string Problem { get; set; }
}