using Medical.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Medical.Specifications.PatientMedical_
{
    public static class PatientMedicalSpecificationsEvaluator
    {
        public static IQueryable<PatientMedical> Evaluate(IQueryable<PatientMedical> query, PatientMedicalSpecifications specs)
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
