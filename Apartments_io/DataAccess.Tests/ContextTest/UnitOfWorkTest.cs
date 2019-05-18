using Xunit;

using DataAccess.Entities;
using DataAccess.Repositories;

using Moq.Protected;

using Microsoft.EntityFrameworkCore;

namespace DataAccess.Tests.ContextTest
{
    public class UnitOfWorkTest
    {
        [Fact]
        public void CreateRepoTest()
        {
            // Arrange
            Moq.Mock<DbContext> mockContext = new Moq.Mock<DbContext>();

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
    }
}
