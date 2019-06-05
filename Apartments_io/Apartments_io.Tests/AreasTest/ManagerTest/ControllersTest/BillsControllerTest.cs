using Xunit;

using Moq;

using DataAccess.Entities;
using DataAccess.Repositories;
using DataAccess.Interfaces;

using Apartments_io.Areas.Manager.Controllers;
using Apartments_io.Areas.Manager.ViewModels.Bills;

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Apartments_io.Tests.AreasTest.ManagerTest.ControllersTest
{
    public class BillsControllerTest
    {
        [Fact]
        public void Index_ViewResult()
        {
            // Arrange
            int userId = 1;
            IEnumerable<Bill> bills = new List<Bill>
            {
                new Bill { Id = 1 }, new Bill { Id = 2 }
            };
            int totalRecordsAmount = bills.Count();

            IEnumerable<User> users = new List<User>
            {
                new User { Id = 1 }, new User { Id = 2 }
            };

            Mock<GenericRepository<Bill>> mockBillRepository = new Mock<GenericRepository<Bill>>();
            mockBillRepository
                .Setup(br => br.Count(It.IsAny<Expression<Func<Bill, bool>>>()))
                .Returns(totalRecordsAmount);
            mockBillRepository
                .Setup(br => br.Get(It.IsAny<Expression<Func<Bill, bool>>>(),
                                    It.IsAny<Func<IQueryable<Bill>, IOrderedQueryable<Bill>>>(),
                                    It.IsAny<string>(), It.IsAny<int>(),
                                    It.IsAny<int>()))
                .Returns(bills);

            Mock<UserRepository> mockUserRepository = new Mock<UserRepository>();
            mockUserRepository
                .Setup(ur => ur.Get(It.IsAny<Expression<Func<User, bool>>>(), null, It.IsAny<string>(), null, null))
                .Returns(users);

            Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            mockIUnitOfWork
                .Setup(u => u.GetRepository<Bill, GenericRepository<Bill>>())
                .Returns(mockBillRepository.Object);
            mockIUnitOfWork
                .Setup(u => u.GetRepository<User, UserRepository>())
                .Returns(mockUserRepository.Object);

            BillsController controller = new BillsController(mockIUnitOfWork.Object);

            Mock<ClaimsPrincipal> mockManager = new Mock<ClaimsPrincipal>();
            mockManager
                .Setup(p => p.FindFirst(It.IsAny<string>()))
                .Returns(new Claim(nameof(User.Id), userId.ToString()));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext() { User = mockManager.Object }
            };

            // Act
            IActionResult result = controller.Index(null, null);

            // Assert
            Assert.NotNull(result);
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);
            IndexViewModel indexViewModel = Assert.IsType<IndexViewModel>(viewResult.Model);
            Assert.Same(bills, indexViewModel.Bills);
            Assert.Same(users, indexViewModel.Renters);
            Assert.Equal(totalRecordsAmount, indexViewModel.TotalRecordsAmount);
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        public async void CreateNewBill_OkResult()
        {
            // Arrange
            int residentId = 1, apartmentId = 1;

            Mock<GenericRepository<Bill>> mockBillRepository = new Mock<GenericRepository<Bill>>();

            Mock<UserRepository> mockUserRepository = new Mock<UserRepository>();

            Mock<ApartmentRepository> mockApartmentRepository = new Mock<ApartmentRepository>();

            Mock<GenericRepository<Notification>> mockNotificationRepository = new Mock<GenericRepository<Notification>>();

            Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            mockIUnitOfWork
                .Setup(u => u.GetRepository<Bill, GenericRepository<Bill>>())
                .Returns(mockBillRepository.Object);
            mockIUnitOfWork
                .Setup(u => u.GetRepository<User, UserRepository>())
                .Returns(mockUserRepository.Object);
            mockIUnitOfWork
                .Setup(u => u.GetRepository<Apartment, ApartmentRepository>())
                .Returns(mockApartmentRepository.Object);
            mockIUnitOfWork
                .Setup(u => u.GetRepository<Notification, GenericRepository<Notification>>())
                .Returns(mockNotificationRepository.Object);

            BillsController controller = new BillsController(mockIUnitOfWork.Object);

            // Act
            IActionResult result = await controller.CreateNewBill(residentId, apartmentId, new DateTime());

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async void UpdateBillBillIsNull_BadRequest()
        {
            // Arrange
            int billId = 1;

            Mock<GenericRepository<Bill>> mockBillRepository = new Mock<GenericRepository<Bill>>();
            mockBillRepository
                .Setup(br => br.GetAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(System.Threading.Tasks.Task.FromResult(null as Bill));
            
            Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            mockIUnitOfWork
                .Setup(u => u.GetRepository<Bill, GenericRepository<Bill>>())
                .Returns(mockBillRepository.Object);

            BillsController controller = new BillsController(mockIUnitOfWork.Object);

            // Act
            IActionResult result = await controller.UpdateBill(billId, DataAccess.Enums.PaymentStatus.Paid, new DateTime());

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async void UpdateBill_OkResult()
        {
            // Arrange        
            Bill bill = new Bill { Id = 1, Renter = new User { Id = 1 } };

            Mock<GenericRepository<Bill>> mockBillRepository = new Mock<GenericRepository<Bill>>();
            mockBillRepository
                .Setup(br => br.GetAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(System.Threading.Tasks.Task.FromResult(bill));

            Mock<GenericRepository<Notification>> mockNotificationRepository = new Mock<GenericRepository<Notification>>();

            Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            mockIUnitOfWork
                .Setup(u => u.GetRepository<Bill, GenericRepository<Bill>>())
                .Returns(mockBillRepository.Object);
            mockIUnitOfWork
                .Setup(u => u.GetRepository<Notification, GenericRepository<Notification>>())
                .Returns(mockNotificationRepository.Object);

            BillsController controller = new BillsController(mockIUnitOfWork.Object);

            // Act
            IActionResult result = await controller.UpdateBill(bill.Id, DataAccess.Enums.PaymentStatus.Paid, new DateTime());

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }
    }
}
