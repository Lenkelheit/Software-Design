namespace Pagination
{
    /// <summary>
    /// Determines first and last pagination pages
    /// </summary>
    public struct PaginationLimit
    {
        /// <summary>
        /// Determines from which page pagination should start
        /// </summary>
        public int StartPage { get; set; }
        /// <summary>
        /// Determines on which page pagination should end
        /// </summary>
        public int EndPage { get; set; }
    }
}
