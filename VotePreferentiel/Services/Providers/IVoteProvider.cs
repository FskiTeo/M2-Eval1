using VotePreferentiel.Model;

namespace VotePreferentiel.Services.Providers;


public interface IVoteProvider
{
    Task<VoteModel?> GetVotesAsync(string url);
}