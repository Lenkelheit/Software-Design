using Xunit;

using Moq;

using DataAccess.Entities;
using DataAccess.Repositories;
using DataAccess.Interfaces;
using DataAccess.Enums;

using Apartments_io.Areas.Resident.Controllers;
using Apartments_io.Areas.Resident.ViewModels.Bills;

using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Apartments_io.Tests.AreasTest.ResidentTest.ControllersTest
{
    public class BillsControllerTest
    {
        [Fact]
        public void List_ViewResult()
        {
            // Arrange
            int userId = 1;
            IEnumerable<Bill> presentBills = new List<Bill>
            {
                new Bill { Id = 1 },
                new Bill { Id = 2 }
            };
            IEnumerable<Bill> pastBills = new List<Bill>
            {
                new Bill { Id = 3 },
                new Bill { Id = 4 }
            };

            Mock<BillRepository> mockBillRepository = new Mock<BillRepository>();
            mockBillRepository
                .Setup(br => br.GetPresentBills(It.IsAny<int>(), It.IsAny<PaymentStatus>()))
                .Returns(presentBills);
            mockBillRepository
                .Setup(br => br.GetPastBills(It.IsAny<int>(), It.IsAny<PaymentStatus>()))
                .Returns(pastBills);

            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(u => u.GetRepository<Bill, BillRepository>())
                .Returns(mockBillRepository.Object);

            ControllerBase controller = new BillsController(mockUnitOfWork.Object);

            Mock<ClaimsPrincipal> userMock = new Mock<ClaimsPrincipal>();
            userMock
                .Setup(p => p.FindFirst(It.IsAny<string>()))
                .Returns(new Claim(nameof(User.Id), userId.ToString()));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext() { User = userMock.Object }
            };

            // Act
            IActionResult result = (controller as BillsController)?.List();

            // Assert
            Assert.NotNull(result);
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);
            ListViewModel listViewModel = Assert.IsType<ListViewModel>(viewResult.Model);
            Assert.Same(presentBills, listViewModel.PresentBills);
            Assert.Same(pastBills, listViewModel.PastBills);
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        public async void PayBillBillIsNull_BadRequest()
        {
            // Arrange
            Bill bill = new Bill { Id = 3 };

            Mock<BillRepository> mockBillRepository = new Mock<BillRepository>();
            mockBillRepository
                .Setup(br => br.GetAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult(null as Bill));

            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(u => u.GetRepository<Bill, BillRepository>())
                .Returns(mockBillRepository.Object);

            BillsController controller = new BillsController(mockUnitOfWork.Object);

            // Act
            IActionResult result = await controller.PayBill(bill.Id);

            // Assert
            Assert.NotNull(result);
            BadRequestObjectResult badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("There is no such bill", badRequestObjectResult.Value.ToString());
        }

        [Fact]
        public async void PayBill_OkResult()
        {
            // Arrange
            Bill bill = new Bill { Id = 3, Apartment = new Apartment() };

            Mock<BillRepository> mockBillRepository = new Mock<BillRepository>();
            mockBillRepository
                .Setup(br => br.GetAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult(bill));

            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(u => u.GetRepository<Bill, BillRepository>())
                .Returns(mockBillRepository.Object);

            BillsController controller = new BillsController(mockUnitOfWork.Object);

            // Act
            IActionResult result = await controller.PayBill(bill.Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }
    }
}
