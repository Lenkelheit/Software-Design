using Microsoft.AspNetCore.Mvc.Rendering;
using static System.Math;

namespace Pagination
{
    public partial class Pagination
    {
        // INNER CLASSES
        public partial class PaginationFluentBuilder { };

        // FIELDS
        private int linksAmountOnPage;
        private int currentPage;
        private int totalRecordsAmount;
        private int recordsAmountPerPage;
        private int totalPageAmount;

        // CONSTRUCTORS
        public Pagination()
        {
            this.linksAmountOnPage = 7;
            this.totalRecordsAmount = -1;
            this.recordsAmountPerPage = 10;

            this.PaginationBuilder = new PaginationBuilder.DefaultPaginationBuilder();
            this.BuildStrategy = new BuildStrategy.DefaultBuildStrategy();
            this.UrlInfo = null;
        }
        public static PaginationFluentBuilder GetBuilder => new PaginationFluentBuilder();

        // PROPERTIES
        public bool HasPreviousPage => currentPage > 1;
        public bool HasNextPage => currentPage < totalPageAmount;
        public int CurrentPage => currentPage;
        public Interfaces.IPaginationBuilder PaginationBuilder { get; set; }
        public Interfaces.IBuildStrategy BuildStrategy { get; set; }
        public DataTransferObject.UrlInfo UrlInfo { get; set; }

        // METHODS
        public TagBuilder Build(DataTransferObject.UrlInfo urlInfo)
        {
            UrlInfo = urlInfo;
            return BuildStrategy.Build(this);
        }
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
