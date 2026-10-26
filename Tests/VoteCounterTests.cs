using VotePreferentiel.Model;
using VotePreferentiel.Services;

namespace Tests;

[TestClass]
public class VoteCounterTests
{
    private readonly VoteCounter _voteCounter = new();

    [TestMethod]
    public void GetWinner_CasNominal_ReturnsChloe()
    {
        // Arrange
        var model = new VoteModel
        {
            Votes = 
            [
                new Vote { Choices = ["Alice", "Bilal", "Chloé"] },
                new Vote { Choices = ["Alice", "Chloé", "Bilal"] },
                new Vote { Choices = ["Bilal", "Chloé", "Alice"] },
                new Vote { Choices = ["Chloé", "Bilal", "Alice"] },
                new Vote { Choices = ["Chloé", "Alice", "Bilal"] }
            ]
        };

        // Act
        var winner = _voteCounter.GetWinner(model);

        // Assert
        Assert.AreEqual("Chloé", winner);
    }

    [TestMethod]
    public void GetWinner_UnSeulCandidat_ReturnsAlice()
    {
        var model = new VoteModel
        {
            Votes = [new Vote { Choices = ["Alice"] }]
        };

        var winner = _voteCounter.GetWinner(model);

        Assert.AreEqual("Alice", winner);
    }

    [TestMethod]
    public void GetWinner_ReportDesPreferences_ReturnsChloe()
    {
        var model = new VoteModel { Votes = [] };
        
        for (int i = 0; i < 4; i++) 
            model.Votes.Add(new Vote { Choices = ["Alice", "Bilal", "Chloé", "Diane"] });
            
        for (int i = 0; i < 3; i++) 
            model.Votes.Add(new Vote { Choices = ["Bilal", "Chloé", "Alice", "Diane"] });
            
        for (int i = 0; i < 3; i++) 
            model.Votes.Add(new Vote { Choices = ["Chloé", "Alice", "Bilal", "Diane"] });
            
        model.Votes.Add(new Vote { Choices = ["Diane", "Chloé", "Bilal", "Alice"] });

        var winner = _voteCounter.GetWinner(model);

        Assert.AreEqual("Chloé", winner);
    }

    [TestMethod]
    public void GetWinner_EgaliteEnDernierePosition_ReturnsChloe()
    {
        var model = new VoteModel
        {
            Votes = 
            [
                new Vote { Choices = ["Alice", "Chloé", "Bilal"] },
                new Vote { Choices = ["Bilal", "Chloé", "Alice"] },
                new Vote { Choices = ["Chloé", "Alice", "Bilal"] },
                new Vote { Choices = ["Chloé", "Bilal", "Alice"] }
            ]
        };

        var winner = _voteCounter.GetWinner(model);

        Assert.AreEqual("Chloé", winner);
    }

    [TestMethod]
    public void GetWinner_BulletinVide_ThrowsException()
    {
        var model = new VoteModel
        {
            Votes = [new Vote { Choices = [] }]
        };

        Assert.Throws<InvalidOperationException>(() => { _voteCounter.GetWinner(model); });
    }
}