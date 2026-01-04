using NUnit.Framework;

namespace KnowU.Domain.Knowledge.Test;

[TestFixture]
public class AgentRespondJsonTest
{
    private AgentRespondJson _agentRespondJson = null!;

    [SetUp]
    public void SetUp()
    {
        _agentRespondJson = new AgentRespondJson();
    }

    [Test]
    public void ExtractJson_WhenExplanationBeforeFence_ThenReturnsJson()
    {
        // Arrange
        _agentRespondJson.AppendText("""
                                     How to ensure the DTS data is persisted
                                     ```json
                                     {
                                       "claims": [
                                         {
                                           "subject": { "id": "1" }
                                         }
                                       ]
                                     }
                                     ```
                                     """);

        // Act
        var result = _agentRespondJson.ExtractJson();

        // Assert
        Assert.That(result, Does.Contain("\"claims\""));
        Assert.That(result, Does.Not.Contain("```"));
        Assert.That(result, Does.Not.Contain("How to ensure"));
    }

    [Test]
    public void ExtractJson_WhenJsonWithFences_ThenReturnsJson()
    {
        // Arrange
        _agentRespondJson.AppendText("""
                                     ```json
                                     {
                                       "claims": [
                                         {
                                           "subject": { "id": "1" }
                                         }
                                       ]
                                     }
                                     ```
                                     """);

        // Act
        var result = _agentRespondJson.ExtractJson();

        // Assert
        Assert.That(result, Does.Contain("\"claims\""));
        Assert.That(result, Does.Not.Contain("```"));
    }

    [Test]
    public void ExtractJson_WhenJsonWithGenericFences_ThenReturnsJson()
    {
        // Arrange
        _agentRespondJson.AppendText("""
                                     ```
                                     {
                                       "claims": [
                                         {
                                           "subject": { "id": "1" }
                                         }
                                       ]
                                     }
                                     ```
                                     """);

        // Act
        var result = _agentRespondJson.ExtractJson();

        // Assert
        Assert.That(result, Does.Contain("\"claims\""));
        Assert.That(result, Does.Not.Contain("```"));
    }

    [Test]
    public void ExtractJson_WhenPlainJson_ThenReturnsJson()
    {
        // Arrange
        _agentRespondJson.AppendText("""
                                     {
                                       "claims": [
                                         {
                                           "subject": { "id": "1" }
                                         }
                                       ]
                                     }
                                     """);

        // Act
        var result = _agentRespondJson.ExtractJson();

        // Assert
        Assert.That(result, Does.Contain("\"claims\""));
        Assert.That(result, Does.Not.Contain("```"));
    }

    [Test]
    public void ExtractJson_WhenWhitespaceAroundJson_ThenReturnsTrimmedJson()
    {
        // Arrange
        _agentRespondJson.AppendText("""


                                     ```json
                                     {
                                       "claims": []
                                     }
                                     ```


                                     """);

        // Act
        var result = _agentRespondJson.ExtractJson();

        // Assert
        Assert.That(result, Does.StartWith("{"));
        Assert.That(result, Does.EndWith("}"));
    }
}