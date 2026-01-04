using KnowU.Domain.Knowledge.Contract;
using KnowU.Domain.Storage.Contract;
using Moq;
using NUnit.Framework;

namespace KnowU.Domain.Knowledge.Test;

[TestFixture]
public class AgentTest
{
    private readonly Mock<IOntologyProvider> _mockOntologyProvider = new();
    private readonly Mock<IStorage> _mockStorage = new();
    private readonly Mock<IAiCore> _mockAiCore = new();

    [SetUp]
    public void SetUp()
    {
        _mockOntologyProvider.Reset();
        _mockStorage.Reset();
        _mockAiCore.Reset();
        
        // Setup default ontology provider behavior
        _mockOntologyProvider.Setup(o => o.GetOntologyPromptSection())
            .Returns("\nAvailable Entity Classes:\n- SoftwareModule\n\nAvailable Predicates:\n- dependsOn");
        _mockOntologyProvider.Setup(o => o.GetJsonSchemaExample())
            .Returns("JSON Schema Example");
        _mockOntologyProvider.Setup(o => o.FindPredicate("http://example.org/predicate/dependsOn"))
            .Returns(new PredicateProperty { Id = "http://example.org/predicate/dependsOn", Label = "dependsOn" });
        _mockOntologyProvider.Setup(o => o.FindClass("http://example.org/class/SoftwareModule"))
            .Returns(new EntityClass { Id = "http://example.org/class/SoftwareModule", Label = "SoftwareModule" });
    }

    [Test]
    public void Constructor_WhenCalled_ThenInitializesAiCoreWithCompleteSystemPrompt()
    {
        // Arrange
        var systemPrompt = "Custom system prompt";

        // Act
        _ = new Agent(systemPrompt, _mockOntologyProvider.Object, _mockStorage.Object, _mockAiCore.Object)
        {
            Id = "test-agent",
            Name = "Test Agent"
        };

        // Assert
        _mockAiCore.Verify(a => a.Initialize(It.Is<string>(prompt =>
                prompt.Contains(systemPrompt) &&
                prompt.Contains("Available Entity Classes:") &&
                prompt.Contains("Available Predicates:"))),
            Times.Once);
    }

    [Test]
    public async Task ProcessAsync_WhenAiCoreFindsClaims_ThenClaimObjectsCorrectlyGenerated()
    {
        // Arrange
        var testDocument = new Document
        {
            Id = "test-doc-123",
            Content = "Test content",
            Tags = new List<string>()
        };

        // Create a valid JSON response with correct ontology IDs
        var respondJson = new AgentRespondJson();
        respondJson.AppendText(@"{
            ""claims"": [
                {
                    ""subject"": {
                        ""id"": ""module1"",
                        ""name"": ""Authentication Module"",
                        ""description"": ""Handles user authentication"",
                        ""typeId"": ""http://example.org/class/SoftwareModule""
                    },
                    ""predicate"": {
                        ""id"": ""http://example.org/predicate/dependsOn""
                    },
                    ""object"": {
                        ""id"": ""module2"",
                        ""name"": ""Database Module"",
                        ""description"": ""Provides data access"",
                        ""typeId"": ""http://example.org/class/SoftwareModule""
                    }
                }
            ]
        }");

        _mockAiCore.Setup(a => a.ProcessAsync(It.IsAny<Document>()))
            .ReturnsAsync(respondJson);

        var systemPrompt = "Extract claims from documents";
        var sut = new Agent(systemPrompt, _mockOntologyProvider.Object, _mockStorage.Object, _mockAiCore.Object)
        {
            Id = "test-agent",
            Name = "Test Agent"
        };

        // Act
        var claims = await sut.ProcessAsync(testDocument);

        // Assert
        Assert.That(claims.Count, Is.EqualTo(1));
        Assert.That(claims[0].Subject.Name, Is.EqualTo("Authentication Module"));
        Assert.That(claims[0].Object.Name, Is.EqualTo("Database Module"));
        Assert.That(claims[0].Predicate.Id, Is.EqualTo("http://example.org/predicate/dependsOn"));
    }

    [Test]
    public async Task ProcessAsync_WhenClaimsAreGenerated_ThenClaimsAreStored()
    {
        // Arrange
        var testDocument = new Document
        {
            Id = "test-doc-123",
            Content = "Test content",
            Tags = new List<string>()
        };

        var respondJson = new AgentRespondJson();
        respondJson.AppendText(@"{
            ""claims"": [
                {
                    ""subject"": {
                        ""id"": ""module1"",
                        ""name"": ""Authentication Module"",
                        ""typeId"": ""http://example.org/class/SoftwareModule""
                    },
                    ""predicate"": {
                        ""id"": ""http://example.org/predicate/dependsOn""
                    },
                    ""object"": {
                        ""id"": ""module2"",
                        ""name"": ""Database Module"",
                        ""typeId"": ""http://example.org/class/SoftwareModule""
                    }
                }
            ]
        }");

        _mockAiCore.Setup(a => a.ProcessAsync(It.IsAny<Document>()))
            .ReturnsAsync(respondJson);

        var systemPrompt = "Extract claims from documents";
        var sut = new Agent(systemPrompt, _mockOntologyProvider.Object, _mockStorage.Object, _mockAiCore.Object)
        {
            Id = "test-agent",
            Name = "Test Agent"
        };

        // Act
        await sut.ProcessAsync(testDocument);

        // Assert
        _mockStorage.Verify(s => s.StoreClaim(It.IsAny<Claim>(), testDocument.Id), Times.Once);
    }
}