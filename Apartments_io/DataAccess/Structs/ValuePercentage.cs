namespace DataAccess.Structs
{
    /// <summary>
    /// Contains data and its percentage value
    /// </summary>
    public struct ValuePercentage
    {
        /// <summary>
        /// Real value
        /// </summary>
        public int Value { get; set; }
        /// <summary>
        /// Percentage of current value
        /// </summary>
        public double Percentage { get; set; }
    }
}
