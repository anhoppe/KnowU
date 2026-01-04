using KnowU.Domain.Knowledge.Contract;
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
        _serializerMock.Verify(m => m.Serialize(It.Is<Document>(p => p.Content == content), It.IsAny<Guid>()), Times.Once);
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
    
    [Test]
    public void StoreDocument_WhenContentIsPassed_ThenDocumentIdIsReturned()
    {
        // Arrange
        var content = "foobar";
        
        // Act
        var documentId = _sut.StoreDocument(content);
        
        // Assert
        Assert.That(documentId, Is.Not.Empty);
    }
    
    [Test]
    public void StoreClaim_WhenClaimIsNull_ThenExceptionIsThrown()
    {
        // Arrange
        Claim? claim = null;
        
        // Act / Assert
        Assert.Throws<ArgumentNullException>(() => _sut.StoreClaim(claim!));
    }
    
    [Test]
    public void StoreClaim_WhenClaimIsPassed_ThenClaimIsSerialized()
    {
        // Arrange
        var claim = Mock.Of<Claim>();

        // Act
        _sut.StoreClaim(claim);

        // Assert
        _serializerMock.Verify(m => m.Serialize(It.IsAny<Claim>()), Times.Once);
    }
    
    [Test]
    public void StoreClaim_WhenClaimIsPassed_ThenClaimIsAdded()
    {
        // Arrange
        var claim = Mock.Of<Claim>();
        
        // Act
        _sut.StoreClaim(claim);
        
        // Assert
        Assert.That(_sut.Claims.Count, Is.EqualTo(1));
    }
    
    [Test]
    public void GetClaimsByDocument_WhenDocumentIdIsEmpty_ThenExceptionIsThrown()
    {
        // Arrange
        var documentId = string.Empty;
        
        // Act / Assert
        Assert.Throws<ArgumentException>(() => _sut.GetClaimsByDocument(documentId));
    }
    
    [Test]
    public void GetClaimsByDocument_WhenClaimsExist_ThenOnlyMatchingClaimsAreReturned()
    {
        // Arrange
        var claim1 = new Claim();
        var claim2 = new Claim();
        var claim3 = new Claim();
        
        _sut.StoreClaim(claim1, "doc1");
        _sut.StoreClaim(claim2, "doc2");
        _sut.StoreClaim(claim3, "doc1");
        
        // Act
        var claims = _sut.GetClaimsByDocument("doc1");
        
        // Assert
        Assert.That(claims.Count, Is.EqualTo(2));
        Assert.That(claims, Does.Contain(claim1));
        Assert.That(claims, Does.Contain(claim3));
    }
}
