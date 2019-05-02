namespace Pagination
{
    public partial class Pagination
    {
        public partial class PaginationFluentBuilder
        {
            // FIELDS
            private Pagination pagination;
            int currentPage;

            // CONSTRUCTOR
            /// <summary>
            /// Initializes a new instance of <see cref="PaginationFluentBuilder"/>
            /// </summary>
            public PaginationFluentBuilder()
            {
                pagination = new Pagination();
            }

            // METHODS
            /// <summary>
            /// Sets links amount on page
            /// </summary>
            /// <param name="linksAmountOnPage">
            /// Determines maximum amount of links shown in page
            /// </param>
            /// <returns>
            /// An instance of see <see cref="PaginationFluentBuilder"/>
            /// </returns>
            public PaginationFluentBuilder SetLinksAmountOnPage(int linksAmountOnPage)
            {
                pagination.linksAmountOnPage = linksAmountOnPage;
                return this;
            }
            /// <summary>
            /// Sets total records amount
            /// </summary>
            /// <param name="recordsAmount">
            /// Determines total records amount
            /// </param>
            /// <returns>
            /// An instance of see <see cref="PaginationFluentBuilder"/>
            /// </returns>
            public PaginationFluentBuilder SetTotalRecordsAmount(int recordsAmount)
            {
                pagination.totalRecordsAmount = recordsAmount;
                return this;
            }
            /// <summary>
            /// Sets amount of records on current page
            /// </summary>
            /// <param name="recordsAmountPerPage">
            /// Determines amount of records on current page
            /// </param>
            /// <returns>
            /// An instance of see <see cref="PaginationFluentBuilder"/>
            /// </returns>
            public PaginationFluentBuilder SetRecordsAmountPerPage(int recordsAmountPerPage)
            {
                pagination.recordsAmountPerPage = recordsAmountPerPage;
                return this;
            }
            /// <summary>
            /// Sets current page number
            /// </summary>
            /// <param name="currentPage">
            /// Determines current page number
            /// </param>
            /// <returns>
            /// An instance of see <see cref="PaginationFluentBuilder"/>
            /// </returns>
            public PaginationFluentBuilder SetCurrentPage(int currentPage)
            {
                this.currentPage = currentPage;
                return this;
            }
            /// <summary>
            /// Sets pagination builder
            /// </summary>
            /// <param name="paginationBuilder">
            /// Determines pagination builder
            /// </param>
            /// <returns>
            /// An instance of see <see cref="PaginationFluentBuilder"/>
            /// </returns>
            public PaginationFluentBuilder SetPaginationBuilder(Interfaces.IPaginationBuilder paginationBuilder)
            {
                pagination.PaginationBuilder = paginationBuilder;
                return this;
            }
            /// <summary>
            /// Sets build strategy
            /// </summary>
            /// <param name="buildStrategy">
            /// Determines build strategy
            /// </param>
            /// <returns>
            /// An instance of see <see cref="PaginationFluentBuilder"/>
            /// </returns>
            public PaginationFluentBuilder SetBuildStrategy(Interfaces.IBuildStrategy buildStrategy)
            {
                pagination.BuildStrategy = buildStrategy;
                return this;
            }

            // BUILDING
            private void Building()
            {
                pagination.CalcTotalPageAmount();
                pagination.SetCurrentPage(currentPage);
            }
            /// <summary>
            /// Builds <see cref="Pagination"/>
            /// </summary>
            /// <returns>
            /// An instance of <see cref="Pagination"/>
            /// </returns>
            public Pagination Build()
            {
                this.Building();
                return pagination;
            }
            /// <summary>
            /// Implicitly convert <see cref="PaginationFluentBuilder"/> to <see cref="Pagination"/>
            /// </summary>
            /// <param name="builder">
            /// An instance of <see cref="PaginationFluentBuilder"/>
            /// </param>
            public static implicit operator Pagination(PaginationFluentBuilder builder)
            {
                builder.Building();
                return builder.pagination;
            }
        }
    }
}
