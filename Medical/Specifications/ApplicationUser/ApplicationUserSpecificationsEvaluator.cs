using Medical.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Medical.Specifications.ApplicationUser_
{
    public static class ApplicationUserSpecificationsEvaluator
    {
        public static IQueryable<ApplicationUser> Evaluate(IQueryable<ApplicationUser> query, ApplicationUserSpecifications specs)
        {
            var result = query;

            if (specs == null)
                return result;

            // Apply criteria
            if (specs.Criteria != null)
                result = result.Where(specs.Criteria);

            // Apply Pagination
            if (specs.IsPagination)
                result = result.Skip(specs.Skip).Take(specs.Take);

            // Apply includes
            if (specs.Includes.Count > 0)
                result = specs.Includes.Aggregate(result, (current, include) => current.Include(include));

            return result;
        }
    }
}
