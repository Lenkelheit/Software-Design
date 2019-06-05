namespace Pagination.DataTransferObject
{
    /// <summary>
    /// Determines parameters for link: text, page, isActive, isDisabled etc
    /// <summary>
    public class LinkInfo
    {
        /// <summary>
        /// Gets or sets to which page link is
        /// <summary>
        public int Page { get; set; }
        /// <summary>
        /// Gets or sets text on link
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Gets or sets is current link active
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Gets or sets is current link disabled
        /// </summary>
        public bool IsDisabled { get; set; }
        /// <summary>
        /// Gets or sets page key that is used in get request
        /// </summary>
        public string PageKey { get; set; }
        /// <summary>
        /// Gets or sets collection with fragments names and values
        /// </summary>
        public System.Collections.Generic.IDictionary<string, object> Fragments { get; set; }
    }
}
