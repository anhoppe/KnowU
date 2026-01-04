using KnowU.Domain.Knowledge.Contract;
using Moq;
using NUnit.Framework;

namespace KnowU.Domain.Knowledge.Test;

[TestFixture]
public class AgentLoaderTest
{
    private AgentLoader _sut = null!;

    [SetUp]
    public void SetUp()
    {
        var mockFactory = new Mock<IAgentFactory>();
        mockFactory.Setup(f =>
                f.CreateAgent(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<IOntologyProvider>()))
            .Returns((string systemPrompt, string id, string name, IOntologyProvider ontologyProvider) =>
            {
                var mockAgent = new Mock<IAgent>();
                mockAgent.SetupGet(a => a.Id).Returns(id);
                mockAgent.SetupGet(a => a.Name).Returns(name);
                return mockAgent.Object;
            });

        var mockOntology = new Mock<IOntologyProvider>();

        _sut = new AgentLoader("Assets/personas.yaml", mockFactory.Object, mockOntology.Object);
    }

    [Test]
    public void Agents_WhenCtorExecuted_ThenExpectedAgentsAvailable()
    {
        // Arrange / Act
        var agents = _sut.Agents;

        // Assert
        Assert.That(agents.Count, Is.EqualTo(4));
        Assert.That(agents[0].Id, Is.EqualTo("executive_assistant"));
    }
}