using VotePreferentiel.Model;

namespace VotePreferentiel.Services;

public class VoteCounter : IVoteCounter
{
    public HashSet<string> GetCandidates(VoteModel voteModel)
    {
        return [.. voteModel.Votes.SelectMany(v => v.Choices).Distinct()];
    }

    public string GetWinner(VoteModel voteModel)
    {
        if (voteModel.Votes.Count == 0 || voteModel.Votes.Any(v => v.Choices.Count == 0))
            throw new InvalidOperationException("Le scrutin contient des bulletins vides ou aucun vote.");

        var activeCandidates = GetCandidates(voteModel);

        while (activeCandidates.Count > 1)
        {
            var firstChoices = GetAllFirstChoices(voteModel);
            var counts = CountFirstChoices(firstChoices);

            foreach (var candidate in activeCandidates)
            {
                counts.TryAdd(candidate, 0);
            }

            var eliminated = GetCandidatesWithLeastVotes(counts);

            foreach (var candidate in eliminated)
            {
                activeCandidates.Remove(candidate);
            }
            
            foreach (var vote in voteModel.Votes)
            {
                vote.Choices.RemoveAll(c => eliminated.Contains(c));
            }
        }

        return activeCandidates.First();
    }

    public List<string> GetAllFirstChoices(VoteModel voteModel)
    {
        return voteModel.Votes
            .Where(v => v.Choices.Count > 0)
            .Select(v => v.Choices.First())
            .ToList();
    }

    public IDictionary<string, int> CountFirstChoices(List<string> firstChoices)
    {
        return firstChoices
            .GroupBy(c => c)
            .ToDictionary(g => g.Key, g => g.Count());
    }

    public string[] GetCandidatesWithLeastVotes(IDictionary<string, int> firstChoiceCounts)
    {
        int minVotes = firstChoiceCounts.Values.Min();
        
        return firstChoiceCounts
            .Where(c => c.Value == minVotes)
            .Select(c => c.Key)
            .ToArray();
    }
}