namespace Core.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="System.String"/>.
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Remove first occurence of word
        /// </summary>
        /// <param name="currentStr">
        /// An instance of <see cref="System.String"/> that has been extended
        /// </param>
        /// <param name="removeStr">
        /// String that should be removed
        /// </param>
        /// <returns>
        /// A string without removed string
        /// </returns>
        public static string Remove(this string currentStr, string removeStr)
        {
            return currentStr.Remove(currentStr.IndexOf(removeStr), removeStr.Length);
        }
    }
}
