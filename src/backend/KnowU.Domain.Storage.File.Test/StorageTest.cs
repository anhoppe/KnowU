using KnowU.Domain.Storage.Contract;
using KnowU.Infrastructure.Io.Contract;
using Moq;
using NUnit.Framework;

namespace KnowU.Domain.Storage.File.Test;

[TestFixture]
public class StorageTest
{
    private Mock<IJsonSerializer> _serializerMock = null!;

    private Storage _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _serializerMock = new Mock<IJsonSerializer>();
        _sut = new Storage(_serializerMock.Object);
    }

    [Test]
    public void StoreDocument_WhenContentIsEmpty_ThenExceptionIsThrown()
    {
        // Arrange
        var content = string.Empty;

        // Act / Assert
        Assert.Throws<ArgumentException>(() => _sut.StoreDocument(content));
    }

    [Test]
    public void StoreDocument_WhenContentIsPassed_ThenDocumentIsSerialized()
    {
        // Arrange
        var content = "foobar";

        // Act
        _sut.StoreDocument(content);

        // Assert
        _serializerMock.Verify(m => m.Serialize(It.Is<Document>(p => p.Content == content)),
            Times.Once);
    }

    [Test]
    public void StoreDocument_WhenContentIsPassed_ThenDocumentIsAdded()
    {
        // Arrange
        var content = "foobar";
        
        // Act
        _sut.StoreDocument(content);
        
        // Assert
        Assert.That(_sut.Documents.Count, Is.EqualTo(1));
    }
}