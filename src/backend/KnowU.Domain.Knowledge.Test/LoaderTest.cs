using NUnit.Framework;

namespace KnowU.Domain.Knowledge.Test;

[TestFixture]
public class LoaderTest
{
    private Loader _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _sut = new Loader();
    }

    [Test]
    public void LoadOnto_WhenLoaded_EntitiesAccessible()
    {
        // Arrange
        var ontology = _sut.Load();
        
        // Act
        
        // Assert
        Assert.That(ontology, Is.Not.Null);
        Assert.That(ontology.Classes, Is.Not.Empty);
        Assert.That(ontology.Properties, Is.Not.Empty);
    }
}
