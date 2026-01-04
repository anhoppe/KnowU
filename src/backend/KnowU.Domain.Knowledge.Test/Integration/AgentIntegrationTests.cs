using KnowU.Domain.Knowledge.Contract;
using KnowU.Domain.Storage.Contract;
using Moq;
using NUnit.Framework;

namespace KnowU.Domain.Knowledge.Test.Integration;

[TestFixture]
public class AgentIntegrationTests
{
    private IAiCore _aiCore = null!;
    private Mock<IStorage> _mockStorage = null!;
    private OntologyProvider _ontologyProvider = null!;
    private Agent _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _ontologyProvider = new OntologyProvider(@"c:\repo\KnowU\data\onto.json");
        _mockStorage = new Mock<IStorage>();
        _aiCore = new AiCore();

        // Load the technical_pm persona system prompt
        var systemPrompt = @"Role: You are a Senior Technical Project Manager (TPM).
Persona: You are an expert in Agile and SDLC. You understand the nuances of software engineering and dependencies.
Objective: Extract technical requirements and engineering constraints.
Extraction_Rules:
  - Identify technical blockers or risks mentioned.
  - Extract specific version numbers, library names, or infrastructure requirements.
  - List 'Decisions Made' versus 'Open Questions'.";

        _sut = new Agent(systemPrompt, _ontologyProvider, _mockStorage.Object, _aiCore)
        {
            Id = "technical_pm",
            Name = "Senior Technical Project Manager"
        };
    }

    [TearDown]
    public void TearDown()
    {
        (_aiCore as IDisposable)?.Dispose();
        _sut?.Dispose();
    }

    [Test]
    [Category("Integration")]
    [Explicit("This is an integration test that requires LLM model to be available")]
    public async Task ProcessAsync_WhenGivenSampleNote_ThenExtractsClaims()
    {
        // Arrange
        var noteContent = File.ReadAllText("Assets/SampleNote.txt");
        var document = new Document
        {
            Content = noteContent,
            Id = "sample_note_1",
            Tags = new List<string>()
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
}
