namespace Apartments_io.Areas.Resident.Extensions
{
    public static class EmergencyLevelExtension
    {
        public static string ToCssHeaderClass(this DataAccess.Enums.EmergencyLevel emergencyLevel)
        {
            switch (emergencyLevel)
            {
                case DataAccess.Enums.EmergencyLevel.Danger:    return "header-danger";
                case DataAccess.Enums.EmergencyLevel.Info:      return "header-info";
                case DataAccess.Enums.EmergencyLevel.Success:   return "header-success";
                case DataAccess.Enums.EmergencyLevel.Warning:   return "header-warning";

                default: throw new System.InvalidOperationException("Wrong emergency level");
            }
        }
        public static string ToCssContainerClass(this DataAccess.Enums.EmergencyLevel emergencyLevel)
        {
            switch (emergencyLevel)
            {
                case DataAccess.Enums.EmergencyLevel.Danger:    return "notification-danger";
                case DataAccess.Enums.EmergencyLevel.Info:      return "notification-info";
                case DataAccess.Enums.EmergencyLevel.Success:   return "notification-success";
                case DataAccess.Enums.EmergencyLevel.Warning:   return "notification-warning";

                default: throw new System.InvalidOperationException("Wrong emergency level");
            }
        }
    }
}
