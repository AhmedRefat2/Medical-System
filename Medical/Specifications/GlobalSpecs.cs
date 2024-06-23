using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Medical.Specifications
{
    public class GlobalSpecs { }


    /// <summary>
    /// Defines sorting preferences for brand queries.
    /// </summary>
    public class BaseGlobalSort : GlobalSpecs
    {
        public List<SortCriteria> Sorts { get; set; } = [];
    }

    /// <summary>
    /// Aggregates specifications including navigation properties, search filters, sort preferences, and pagination details.
    /// </summary>
    public class BaseGlobalSpecs<TNavigations, TSearch> : GlobalSpecs
    {
        /// <summary>
        /// Gets or sets the navigation specifications.
        /// </summary>
        public TNavigations? Navigations { get; set; }

        /// <summary>
        /// Gets or sets the search criteria.
        /// </summary>
        public TSearch? Search { get; set; }

        /// <summary>
        /// Gets or sets the sort preferences.
        /// </summary>
        public BaseGlobalSort? Sort { get; set; }

        /// <summary>
        /// Gets or sets the pagination specifications.
        /// </summary>
        [Required]
        public PaginationSpecs Pagination { get; set; } = new PaginationSpecs();
    }


    /// <summary>
    /// Represents sorting criteria for brand queries.
    /// </summary>
    public class SortCriteria
    {
        [Required]
        public string Property { get; set; } = string.Empty;

        public bool Ascending { get; set; } = true;

        [Range(1, int.MaxValue, ErrorMessage = "Priority must be greater than 0.")]
        public int Priority { get; set; } = int.MaxValue;
    }


    public class SortCriteria<T>
    {
        public Expression<Func<T, object>> OrderBy { get; set; }
        public bool Ascending { get; set; }
        public int Priority { get; set; } // Represents the sort priority or level
    }
}
