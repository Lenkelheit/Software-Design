using Xunit;

using DataAccess.Entities;
using DataAccess.Repositories;

using Microsoft.EntityFrameworkCore;

using Moq;

using System.Threading.Tasks;

namespace DataAccess.Tests.ContextTest
{
    public class UnitOfWorkTest
    {
        [Fact]
        public void CreateRepoTest()
        {
            // Arrange
            Mock<DbContext> mockContext = new Mock<DbContext>();

            Context.UnitOfWork unitOfWork = new Context.UnitOfWork(mockContext.Object);

            // Act
            GenericRepository<Bill> firstBillRepos = unitOfWork.GetRepository<Bill, GenericRepository<Bill>>();
            GenericRepository<Bill> secondBillRepos = unitOfWork.GetRepository<Bill, GenericRepository<Bill>>();
            GenericRepository<Notification> notificationRepos = unitOfWork.GetRepository<Notification, GenericRepository<Notification>>();

            // Assert
            Assert.NotNull(firstBillRepos);
            Assert.NotNull(secondBillRepos);
            Assert.NotNull(notificationRepos);

            Assert.Same(firstBillRepos, secondBillRepos);
            Assert.NotSame(firstBillRepos, notificationRepos);
        }
        [Fact]
        public void DisposeTest()
        {
            // Arrange
            Mock<DbContext> mockContext = new Mock<DbContext>();

            Context.UnitOfWork unitOfWork = new Context.UnitOfWork(mockContext.Object);

            // Act
            unitOfWork.Dispose();

            // Assert
            mockContext.Verify(c => c.Dispose());
        }
        [Fact]
        public void SaveChangesTest()
        {
            // Arrange
            Mock<DbContext> mockContext = new Mock<DbContext>();

            Context.UnitOfWork unitOfWork = new Context.UnitOfWork(mockContext.Object);

            // Act
            unitOfWork.Save();

            // Assert
            mockContext.Verify(c => c.SaveChanges());
        }

        [Fact]
        public async Task SaveChangesAsyncTest()
        {
            // Arrange
            Mock<DbContext> mockContext = new Mock<DbContext>();

            Context.UnitOfWork unitOfWork = new Context.UnitOfWork(mockContext.Object);

            // Act
            await unitOfWork.SaveAsync();

            // Assert
            mockContext.Verify(c => c.SaveChangesAsync(default(System.Threading.CancellationToken)));
        }
    }
}
