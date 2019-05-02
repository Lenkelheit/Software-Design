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
            public PaginationFluentBuilder()
            {
                pagination = new Pagination();
            }

            // METHODS
            public PaginationFluentBuilder SetLinksAmountOnPage(int linksAmountOnPage)
            {
                pagination.linksAmountOnPage = linksAmountOnPage;
                return this;
            }
            public PaginationFluentBuilder SetTotalRecordsAmount(int recordsAmount)
            {
                pagination.totalRecordsAmount = recordsAmount;
                return this;
            }
            public PaginationFluentBuilder SetRecordsAmountPerPage(int recordsAmountPerPage)
            {
                pagination.recordsAmountPerPage = recordsAmountPerPage;
                return this;
            }
            public PaginationFluentBuilder SetCurrentPage(int currentPage)
            {
                this.currentPage = currentPage;
                return this;
            }
            public PaginationFluentBuilder SetPaginationBuilder(Interfaces.IPaginationBuilder paginationBuilder)
            {
                pagination.PaginationBuilder = paginationBuilder;
                return this;
            }
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
            public Pagination Build()
            {
                this.Building();
                return pagination;
            }
            public static implicit operator Pagination(PaginationFluentBuilder builder)
            {
                builder.Building();
                return builder.pagination;
            }
        }
    }
}
