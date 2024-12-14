using Moq;
using System.Linq.Expressions;
using TravelConnect.Domain.Entities.Base;
using TravelConnect.Domain.Ports.Persistence;
namespace TestProject.Domain.Ports;

[TestFixture]
public class RepositoryTests
{
    private Mock<IRepository<IEntityBase<Guid>>> _mockRepository;

    [SetUp]
    public void Setup()
    {
        _mockRepository = new Mock<IRepository<IEntityBase<Guid>>>();
    }

    [Test]
    public async Task GetByIdAsync_WithValidId_ReturnsEntity()
    {
        var id = Guid.NewGuid();
        var expectedEntity = new Mock<IEntityBase<Guid>>();
        expectedEntity.Setup(e => e.Id).Returns(id);

        _mockRepository.Setup(repo => repo.GetByIdAsync(id, It.IsAny<Expression<Func<IEntityBase<Guid>, object>>[]>()))
                       .ReturnsAsync(expectedEntity.Object);

        var result = await _mockRepository.Object.GetByIdAsync(id);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(id));
        _mockRepository.Verify(repo => repo.GetByIdAsync(id, It.IsAny<Expression<Func<IEntityBase<Guid>, object>>[]>()), Times.Once);
    }

    [Test]
    public async Task GetAllAsync_ReturnsAllEntities()
    {
        var expectedEntities = new List<IEntityBase<Guid>>
        {
            new Mock<IEntityBase<Guid>>().Object,
            new Mock<IEntityBase<Guid>>().Object
        };

        _mockRepository.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<IEntityBase<Guid>, object>>[]>()))
                       .ReturnsAsync(expectedEntities);

        var result = await _mockRepository.Object.GetAllAsync();

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(expectedEntities.Count));
        _mockRepository.Verify(repo => repo.GetAllAsync(It.IsAny<Expression<Func<IEntityBase<Guid>, object>>[]>()), Times.Once);
    }

    [Test]
    public async Task FindAsync_WithValidPredicate_ReturnsEntities()
    {
        var predicate = It.IsAny<Expression<Func<IEntityBase<Guid>, bool>>>();
        var expectedEntities = new List<IEntityBase<Guid>>
        {
            new Mock<IEntityBase<Guid>>().Object
        };

        _mockRepository.Setup(repo => repo.FindAsync(predicate, It.IsAny<Expression<Func<IEntityBase<Guid>, object>>[]>()))
                       .ReturnsAsync(expectedEntities);

        var result = await _mockRepository.Object.FindAsync(predicate);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(expectedEntities.Count));
        _mockRepository.Verify(repo => repo.FindAsync(predicate, It.IsAny<Expression<Func<IEntityBase<Guid>, object>>[]>()), Times.Once);
    }

    [Test]
    public async Task AddAsync_WithValidEntity_CallsAddMethod()
    {
        var entityToAdd = new Mock<IEntityBase<Guid>>().Object;

        _mockRepository.Setup(repo => repo.AddAsync(entityToAdd))
                       .Returns(Task.CompletedTask);

        await _mockRepository.Object.AddAsync(entityToAdd);

        _mockRepository.Verify(repo => repo.AddAsync(entityToAdd), Times.Once);
    }

    [Test]
    public async Task UpdateAsync_WithValidEntity_CallsUpdateMethod()
    {
        var entityToUpdate = new Mock<IEntityBase<Guid>>().Object;

        _mockRepository.Setup(repo => repo.UpdateAsync(entityToUpdate))
                       .Returns(Task.CompletedTask);

        await _mockRepository.Object.UpdateAsync(entityToUpdate);

        _mockRepository.Verify(repo => repo.UpdateAsync(entityToUpdate), Times.Once);
    }

    [Test]
    public async Task DeleteAsync_WithValidEntity_CallsDeleteMethod()
    {
        var entityToDelete = new Mock<IEntityBase<Guid>>().Object;

        _mockRepository.Setup(repo => repo.DeleteAsync(entityToDelete))
                       .Returns(Task.CompletedTask);

        await _mockRepository.Object.DeleteAsync(entityToDelete);

        _mockRepository.Verify(repo => repo.DeleteAsync(entityToDelete), Times.Once);
    }
}
