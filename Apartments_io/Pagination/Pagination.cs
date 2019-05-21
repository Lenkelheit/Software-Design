using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using static System.Math;

namespace Pagination
{
    /// <summary>
    /// A model class for pagination
    /// </summary>
    public partial class Pagination
    {
        // INNER CLASSES
        /// <summary>
        /// Builds <see cref="Pagination"/>
        /// </summary>
        public partial class PaginationFluentBuilder { };

        // FIELDS
        private int linksAmountOnPage;
        private int currentPage;
        private int totalRecordsAmount;
        private int recordsAmountPerPage;
        private int totalPageAmount;

        private string pageKey;
        private IDictionary<string, object> fragments;

        // CONSTRUCTORS
        private Pagination()
        {
            this.linksAmountOnPage = 7;
            this.totalRecordsAmount = -1;
            this.recordsAmountPerPage = 10;
            this.pageKey = "page";
            this.fragments = new Dictionary<string, object>();

            this.PaginationBuilder = new PaginationBuilder.DefaultPaginationBuilder();
            this.BuildStrategy = new BuildStrategy.DefaultBuildStrategy();
            this.UrlInfo = null;
        }
        /// <summary>
        /// Gets <see cref="PaginationFluentBuilder"/>
        /// </summary>
        public static PaginationFluentBuilder GetBuilder => new PaginationFluentBuilder();

        // PROPERTIES
        /// <summary>
        /// Gets total pages amount
        /// </summary>
        public int TotalPagesAmount => totalPageAmount;
        /// <summary>
        /// Determines if there is previous page
        /// </summary>
        public bool HasPreviousPage => currentPage > 1;
        /// <summary>
        /// Determines if there is next page
        /// </summary>
        public bool HasNextPage => currentPage < totalPageAmount;
        /// <summary>
        /// Gets current page number
        /// </summary>
        public int CurrentPage => currentPage;
        /// <summary>
        /// Gets page key
        /// </summary>
        public string PageKey => pageKey;
        /// <summary>
        /// Gets fragments collection
        /// </summary>
        public IDictionary<string, object> Fragments => fragments;
        /// <summary>
        /// Gets pagination builder
        /// </summary>
        public Interfaces.IPaginationBuilder PaginationBuilder { get; set; }
        /// <summary>
        /// Gets building strategy
        /// </summary>
        public Interfaces.IBuildStrategy BuildStrategy { get; set; }
        /// <summary>
        /// Gets data to build url
        /// </summary>
        public DataTransferObject.UrlInfo UrlInfo { get; set; }

        // METHODS
        /// <summary>
        /// Build pagination
        /// </summary>
        /// <param name="urlInfo">
        /// An instance of class with data for URL
        /// </param>
        /// <returns>
        /// The class that can create HTML
        /// </returns>
        public TagBuilder Build(DataTransferObject.UrlInfo urlInfo)
        {
            UrlInfo = urlInfo;
            return BuildStrategy.Build(this);
        }
        /// <summary>
        /// Calculate first and last pagination pages
        /// </summary>
        /// <returns>
        /// An instance of <see cref="PaginationLimit"/> that determines first and last pagination pages
        /// </returns>
        public PaginationLimit CalcPagintaionLimits()
        {
            // вираховуємо посилання зліва, щоб активне посиланння було посередині
            int left = this.currentPage - (int)Round(linksAmountOnPage / (float)2);
        
            // початок відрахунку
            int start = left > 0 ? left: 1;
            int end;

            if (start + linksAmountOnPage <= totalPageAmount) 
            {
                end = start > 1 ? start + linksAmountOnPage : linksAmountOnPage;
            } 
            else 
            {
                end = totalPageAmount;
                start = totalPageAmount - linksAmountOnPage > 0 ? totalPageAmount - linksAmountOnPage : 1;
            }

            return new PaginationLimit
            {
                StartPage = start,
                EndPage = end
            };
        }
        private void SetCurrentPage(int currentPage)
        {
            this.currentPage = currentPage;

            if (this.currentPage > 0) 
            {
                if (this.currentPage > this.totalPageAmount)
                {
                    this.currentPage = this.totalPageAmount;
                }
            } 
            else
            {
                this.currentPage = 1;
            }
        }
        private int CalcTotalPageAmount()
        {
            this.totalPageAmount = (int)Ceiling(totalRecordsAmount / (float)recordsAmountPerPage);
            return totalPageAmount;
        }

    }
}
