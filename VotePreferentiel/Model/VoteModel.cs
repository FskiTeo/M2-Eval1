namespace VotePreferentiel.Model;

public class VoteModel
{
    public List<Vote> Votes { get; set; } = [];
}

public class Vote
{
    public List<string> Choices { get; set; } = [];
}