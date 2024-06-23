using Medical.Data.Entities;
using Medical.Specifications.PatientMedical_;
using System.Linq.Expressions;

namespace Medical.Specifications.PatientMedical_
{
    /// <summary>
    /// Represents base specifications for querying PatientMedical entities.
    /// </summary>
    public class PatientMedicalSpecifications
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientMedicalSpecifications"/> class without any criteria.
        /// </summary>
        /// <remarks>
        /// This constructor is used when no specific criteria is needed for querying entities.
        /// It indicates that all PatientMedical entities will be retrieved.
        /// </remarks>
        public PatientMedicalSpecifications(BaseGlobalSpecs<PatientMedicalNavigations, PatientMedicalSearch>? specs)
        {
            if (specs != null)
            {
                if (specs.Search != null)
                    Criteria = x =>
                        (!specs.Search.Id.HasValue || specs.Search.Id == x.Id) &&
                        (!specs.Search.PatientId.HasValue || specs.Search.PatientId == x.PatientId) &&
                        (string.IsNullOrEmpty(specs.Search.Text) || x.Text.Contains(specs.Search.Text)) &&
                        (string.IsNullOrEmpty(specs.Search.Type) || x.Type.Contains(specs.Search.Type));

                if (specs.Navigations != null && specs.Navigations.EnablePatient)
                    AddInclude(x => x.Patient);

                if (specs.Pagination != null)
                {
                    ProcessPagination(specs.Pagination);
                }

            }
        }

        public PatientMedicalSpecifications()
        {
        }

        /// <summary>
        /// Gets or sets the criteria expression.
        /// </summary>
        public Expression<Func<PatientMedical, bool>> Criteria { get; set; }

        /// <summary>
        /// Gets or sets the list of include expressions.
        /// </summary>
        public List<Expression<Func<PatientMedical, object>>> Includes { get; set; } = new List<Expression<Func<PatientMedical, object>>>();

        /// <summary>
        /// Adds an include expression.
        /// </summary>
        /// <param name="include">The include expression.</param>
        public void AddInclude(Expression<Func<PatientMedical, object>> include) => Includes.Add(include);

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
