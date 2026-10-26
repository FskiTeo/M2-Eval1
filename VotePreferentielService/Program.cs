namespace VotePreferentielService;
using VotePreferentiel;
using VotePreferentiel.Services.Providers;
using VotePreferentiel.Services;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        
        builder.Services.AddHttpClient();
        
        builder.Services.AddTransient<IVoteProvider, VoteProvider>();
        builder.Services.AddTransient<IVoteCounter, VoteCounter>();

        builder.Services.AddHostedService<Worker>();
        
        var host = builder.Build();
        host.Run();
    }
}