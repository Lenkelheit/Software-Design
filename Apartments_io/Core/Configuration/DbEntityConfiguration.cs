namespace Core.Configuration
{
    /// <summary>
    /// Consists of all important configuration to DataBase.
    /// </summary>
    public static class DbEntityConfiguration
    {
        // USER
        #region USER
        /// <summary>
        /// Max length of user's first name
        /// </summary>
        public static readonly int USER_FIRST_NAME_MAX_LENGTH = 10;
        /// <summary>
        /// Max length of user's last name
        /// </summary>
        public static readonly int USER_LAST_NAME_MAX_LENGTH = 10;
        /// <summary>
        /// Max length of user's email
        /// </summary>
        public static readonly int USER_EMAIL_MAX_LENGTH = 25;
        /// <summary>
        /// Max length of user's password
        /// </summary>
        public static readonly int USER_PASSWORD_MAX_LENGTH = 20;
        #endregion

        // NOTIFICATION
        #region NOTIFICATION
        /// <summary>
        /// Max length of notification's description
        /// </summary>
        public static readonly int NOTIFICATION_DESCRIPTION_MAX_LENGTH = 256;
        #endregion

        // APARTMENT
        #region APARTMENT
        /// <summary>
        /// Max length of apartment's name
        /// </summary>
        public static readonly int APARTMENT_NAME_MAX_LENGTH = 50;
        /// <summary>
        /// Max length of apartment's description
        /// </summary>
        public static readonly int APARTMENT_DESCRIPTION_MAX_LENGTH = 1024;
        /// <summary>
        /// Max length of apartment's photo path
        /// </summary>
        public static readonly int APARTMENT_PHOTO_PATH_MAX_LENGTH = 256;
        #endregion
    }
}
