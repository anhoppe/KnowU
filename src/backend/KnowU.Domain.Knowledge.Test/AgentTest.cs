using KnowU.Domain.Knowledge.Contract;
using KnowU.Domain.Storage.Contract;
using Moq;
using NUnit.Framework;

namespace KnowU.Domain.Knowledge.Test;

[TestFixture]
public class AgentTest
{
    private Agent _sut = null!;
    private OntologyProvider _ontologyProvider = null!;

    [SetUp]
    public void SetUp()
    {
        _ontologyProvider = new OntologyProvider(@"c:\repo\KnowU\data\onto.json");
        var mockStorage = new Mock<IStorage>();
        
        // Load the technical_pm persona system prompt
        var systemPrompt = @"Role: You are a Senior Technical Project Manager (TPM).
Persona: You are an expert in Agile and SDLC. You understand the nuances of software engineering and dependencies.
Objective: Extract technical requirements and engineering constraints.
Extraction_Rules:
  - Identify technical blockers or risks mentioned.
  - Extract specific version numbers, library names, or infrastructure requirements.
  - List 'Decisions Made' versus 'Open Questions'.";

        _sut = new Agent(systemPrompt, _ontologyProvider, mockStorage.Object)
        {
            Id = "technical_pm",
            Name = "Senior Technical Project Manager"
        };
    }

    [Test]
    public async Task ProcessAsync_WhenClaimsAreGenerated_ThenClaimsAreStored()
    {
        throw new NotImplementedException("Implement when refactoring of Agent is done");
    }

    [Test, Category("System")]
    [Explicit("This is a system test that requires LLM model to be available")]
    public async Task ProcessAsync_WhenGivenSampleNote_ThenExtractsClaims()
    {
        // Arrange
        var noteContent = File.ReadAllText("Assets/SampleNote.txt");
        var document = new TestDocument
        {
            Content = noteContent,
            Id = "sample_note_1"
        };

        // Act
        var claims = await _sut.ProcessAsync(document);

        // Assert
        Assert.That(claims, Is.Not.Null);
        Assert.That(claims.Count, Is.GreaterThan(0), "Expected at least one claim to be extracted");
        
        // Verify claims have proper structure
        foreach (var claim in claims)
        {
            Assert.That(claim.Subject, Is.Not.Null);
            Assert.That(claim.Subject.Id, Is.Not.Empty);
            Assert.That(claim.Predicate, Is.Not.Null);
            Assert.That(claim.Predicate.Id, Is.Not.Empty);
            Assert.That(claim.Object, Is.Not.Null);
            Assert.That(claim.Object.Id, Is.Not.Empty);
        }

        // Print extracted claims for verification
        Console.WriteLine($"Extracted {claims.Count} claims:");
        foreach (var claim in claims)
        {
            Console.WriteLine($"  {claim.Subject.Id} --[{claim.Predicate.Label}]--> {claim.Object.Id}");
        }
    }

    [TearDown]
    public void TearDown()
    {
        _sut = null!;
    }

    private class TestDocument : IDocument
    {
        public string Id { get; init; } = string.Empty;
        public string Content { get; init; } = string.Empty;
    }
}
