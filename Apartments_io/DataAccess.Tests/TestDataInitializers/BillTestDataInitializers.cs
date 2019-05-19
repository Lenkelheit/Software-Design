using DataAccess.Entities;

namespace DataAccess.Tests.TestDataInitializers
{
    class BillTestDataInitializers : Interfaces.IDbInitializer
    {
        // PROPERTIES
        public int BillsCount => 6;

        public int PaidBillsCount => 1;
        public int PaidWithDelayBillsCount => 1;
        public int WaitingForPaymentBillsCount => 3;
        public int OverdueBillsCount => 1;

        // METHODS
        public void Seed(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
        {
            #region Bills
            Bill bill1 = new Bill { Id = 1, PaymentStatus = Enums.PaymentStatus.Paid };

            Bill bill2 = new Bill { Id = 2, PaymentStatus = Enums.PaymentStatus.PaidWithDelay };

            Bill bill3 = new Bill { Id = 3, PaymentStatus = Enums.PaymentStatus.WaitingForPayment };
            Bill bill4 = new Bill { Id = 4, PaymentStatus = Enums.PaymentStatus.WaitingForPayment };
            Bill bill5 = new Bill { Id = 5, PaymentStatus = Enums.PaymentStatus.WaitingForPayment };

            Bill bill6 = new Bill { Id = 6, PaymentStatus = Enums.PaymentStatus.Overdue };
            #endregion

            #region Seed
            modelBuilder.Entity<Bill>().HasData(bill1, bill2, bill3, bill4, bill5, bill6);
            #endregion
        }
    }
}
