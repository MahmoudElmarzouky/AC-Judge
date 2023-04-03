namespace ACJudge.Data.API;

public interface IGrabProblem
{
    public ProblemInfo GetProblem(string url);
}