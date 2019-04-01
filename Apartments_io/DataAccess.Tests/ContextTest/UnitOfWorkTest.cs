using Xunit;

using DataAccess.Entities;
using DataAccess.Repositories;

using Moq.Protected;

using Microsoft.EntityFrameworkCore;

namespace DataAccess.Tests.ContextTest
{
    public class UnitOfWorkTest
    {
        [Fact(Skip = "Stupid mocking")]
        public void CreateRepoTest()
        {
            // Arrange
            Moq.Mock<Context.DataBaseContext> mockContext = new Moq.Mock<Context.DataBaseContext>();

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
        }
    }
}
