
using Medical.Data;
using Medical.Data.Entities;
using Medical.Specifications;
using Medical.Specifications.PatientMedical_;
using Microsoft.EntityFrameworkCore;

namespace Medical.Repositories
{
    public class PatientMedicalRepo(ApplicationDbContext dbContext)
    {
        private readonly ApplicationDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

        public async Task AddAsync(PatientMedical pm)
        {
            ArgumentNullException.ThrowIfNull(pm);
            await _dbContext.Set<PatientMedical>().AddAsync(pm);
        }

        public async Task AddAsync(IList<PatientMedical> pms)
        {
            ArgumentNullException.ThrowIfNull(pms);
            await _dbContext.Set<PatientMedical>().AddRangeAsync(pms);
        }

        public async Task<int> CommitAsync()
        => await _dbContext.SaveChangesAsync();

        public async Task<int> CountAsync(PatientMedicalSpecifications spec)
        {
            PatientMedicalSpecifications specsCp = new()
            {
                Criteria = spec.Criteria,
            };
            var query = ApplySpecificationEvaluator(_dbContext.Set<PatientMedical>(), specsCp);
            return await query.CountAsync();
        }

        public void Delete(PatientMedical pm)
        {
            ArgumentNullException.ThrowIfNull(pm);
            _dbContext.Set<PatientMedical>().Remove(pm);
        }

        public async Task<Pagination<PatientMedical>?> GetAllAsync(PatientMedicalSpecifications spec, PaginationSpecs pagination)
        {
            var query = ApplySpecificationEvaluator(_dbContext.Set<PatientMedical>(), spec);
            var entities = await query.ToListAsync();
            return new()
            {
                Count = CountAsync(spec).Result,
                Page = pagination.PageIndex,
                Size = pagination.PageSize,
                Items = entities
            };
        }
        public async Task<IEnumerable<PatientMedical>?> GetAllAsync(PatientMedicalSpecifications spec)
        {
            var query = ApplySpecificationEvaluator(_dbContext.Set<PatientMedical>(), spec);
            var entities = await query.ToListAsync();
            return entities;
        }

        public async Task<PatientMedical?> GetByIdAsync(int id)
        {
            return await _dbContext.Set<PatientMedical>().FindAsync(id);
        }

        public async Task<PatientMedical?> GetAsync(PatientMedicalSpecifications spec)
        {
            var query = ApplySpecificationEvaluator(_dbContext.Set<PatientMedical>(), spec);
            return await query.FirstOrDefaultAsync();
        }

        public void Update(PatientMedical pm)
        {
            ArgumentNullException.ThrowIfNull(pm);
            _dbContext.Set<PatientMedical>().Update(pm);
        }

        private IQueryable<PatientMedical> ApplySpecificationEvaluator(IQueryable<PatientMedical> query, PatientMedicalSpecifications specs)
        {
            return PatientMedicalSpecificationsEvaluator.Evaluate(query, specs);
        }
    }
}
