using NUnit.Framework;

namespace KnowU.Domain.Knowledge.Test;

[TestFixture]
public class OntologyProviderTest
{
    private OntologyProvider _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _sut = new OntologyProvider(@"c:\repo\KnowU\data\onto.json");
    }

    [Test]
    public void LoadOnto_WhenLoaded_EntitiesAccessible()
    {
        // Arrange
        var ontology = _sut.GetOntology();

        // Act

        // Assert
        Assert.That(ontology, Is.Not.Null);
        Assert.That(ontology.Classes, Is.Not.Empty);
        Assert.That(ontology.Properties, Is.Not.Empty);
    }
}
