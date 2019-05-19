using DataAccess.Entities;

namespace DataAccess.Tests.TestDataInitializers
{
    class NotificationTestDataInitializer : Interfaces.IDbInitializer
    {
        // PROPERTIES
        public int NotificationsCount => 5;
        public int InfoNotificationsCount => 2;
        public int SuccessNotificationsCount => 1;
        public int WarningNotificationsCount => 1;
        public int DangerNotificationsCount => 1;

        // METHODS
        public void Seed(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
        {
            #region Notification
            Notification notification1 = new Notification() { Id = 1, EmergencyLevel = Enums.EmergencyLevel.Info, Description = "Notification 1" };
            Notification notification2 = new Notification() { Id = 2, EmergencyLevel = Enums.EmergencyLevel.Info, Description = "Notification 2" };

            Notification notification3 = new Notification() { Id = 3, EmergencyLevel = Enums.EmergencyLevel.Success, Description = "Notification 3" };

            Notification notification4 = new Notification() { Id = 4, EmergencyLevel = Enums.EmergencyLevel.Warning, Description = "Notification 4" };

            Notification notification5 = new Notification() { Id = 5, EmergencyLevel = Enums.EmergencyLevel.Danger, Description = "Notification 5" };
            #endregion

            #region Seed
            modelBuilder.Entity<Notification>().HasData(notification1, notification2, notification3, notification4, notification5);
            #endregion
        }
    }
}
