using VotePreferentiel.Services;
using VotePreferentiel.Services.Providers;

namespace VotePreferentielService;

public class Worker(
    IVoteProvider voteProvider,
    IVoteCounter voteCounter,
    IHostApplicationLifetime hostApplicationLifetime,
    ILogger<Worker> logger
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var url = "https://raw.githubusercontent.com/JLou/dotnet-course/refs/heads/main/eval-1/votes.json";
            
            logger.LogInformation("Récupération des votes en cours...");
            var voteModel = await voteProvider.GetVotesAsync(url);

            if (voteModel is not null)
            {
                var winner = voteCounter.GetWinner(voteModel);
                logger.LogInformation("La personne gagnante est : {Winner}", winner);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erreur lors du dépouillement.");
        }
        finally
        {
            // Stop once finsihed
            hostApplicationLifetime.StopApplication();
        }
    }
}