using Medical.Data.Entities;
using System.Linq.Expressions;

namespace Medical.Specifications.ApplicationUser_
{
    /// <summary>
    /// Represents base specifications for querying ApplicationUser entities.
    /// </summary>
    public class ApplicationUserSpecifications
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationUserSpecifications"/> class without any criteria.
        /// </summary>
        /// <remarks>
        /// This constructor is used when no specific criteria is needed for querying entities.
        /// It indicates that all ApplicationUser entities will be retrieved.
        /// </remarks>
        public ApplicationUserSpecifications(BaseGlobalSpecs<ApplicationUserNavigations, ApplicationUserSearch>? specs)
        {
            if (specs != null)
            {
                if (specs.Search != null)
                    Criteria = x =>
                        (!specs.Search.Id.HasValue || specs.Search.Id == x.Id) &&
                        (!specs.Search.UserRole.HasValue || specs.Search.UserRole == x.UserRole) &&
                        (string.IsNullOrEmpty(specs.Search.NationalId) || x.NationalId.Contains(specs.Search.NationalId)) &&
                        (string.IsNullOrEmpty(specs.Search.Name) || x.Name.Contains(specs.Search.Name)) &&
                        (string.IsNullOrEmpty(specs.Search.UserName) || x.UserName.Contains(specs.Search.UserName)) &&
                        (string.IsNullOrEmpty(specs.Search.Email) || string.IsNullOrEmpty(x.Email) || x.Email.Contains(specs.Search.Email)) &&
                        (string.IsNullOrEmpty(specs.Search.PhoneNumber) || string.IsNullOrEmpty(x.PhoneNumber) || x.PhoneNumber.Contains(specs.Search.PhoneNumber));

                if (specs.Navigations != null && specs.Navigations.EnableMedicalsReports)
                    AddInclude(x => x.Medicals);

                if(specs.Pagination != null)
                {
                    ProcessPagination(specs.Pagination);
                }

            }
        }

        public ApplicationUserSpecifications()
        {
        }

        /// <summary>
        /// Gets or sets the criteria expression.
        /// </summary>
        public Expression<Func<ApplicationUser, bool>> Criteria { get; set; }

        /// <summary>
        /// Gets or sets the list of include expressions.
        /// </summary>
        public List<Expression<Func<ApplicationUser, object>>> Includes { get; set; } = new List<Expression<Func<ApplicationUser, object>>>();

        /// <summary>
        /// Adds an include expression.
        /// </summary>
        /// <param name="include">The include expression.</param>
        public void AddInclude(Expression<Func<ApplicationUser, object>> include) => Includes.Add(include);

        /// <summary>
        /// Gets or sets a value indicating whether pagination is applied.
        /// </summary>
        public bool IsPagination { get; set; }

        /// <summary>
        /// Gets or sets the number of items to skip.
        /// </summary>
        public int Skip { get; set; }

        /// <summary>
        /// Gets or sets the number of items to take.
        /// </summary>
        public int Take { get; set; }

        /// <summary>
        /// Applies pagination.
        /// </summary>
        /// <param name="skip">The number of items to skip.</param>
        /// <param name="take">The number of items to take.</param>
        public void ApplyPagination(int skip, int take)
        {
            IsPagination = true;
            Skip = skip;
            Take = take;
        }


        /// <summary>
        /// Processes pagination specifications.
        /// </summary>
        /// <param name="specs">The pagination specifications.</param>
        private protected void ProcessPagination(PaginationSpecs specs)
        {
            if (specs == null) return;

            IsPagination = true;
            ApplyPagination((specs.PageIndex - 1) * specs.PageSize, specs.PageSize);
        }
    }
}
