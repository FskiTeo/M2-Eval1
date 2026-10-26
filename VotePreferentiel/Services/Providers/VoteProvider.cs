using VotePreferentiel.Model;
using System.Net.Http.Json;

namespace VotePreferentiel.Services.Providers;

public class VoteProvider(HttpClient httpClient) : IVoteProvider
{
    public async Task<VoteModel?> GetVotesAsync(string url)
    {
        return await httpClient.GetFromJsonAsync<VoteModel>(url);
    }
}