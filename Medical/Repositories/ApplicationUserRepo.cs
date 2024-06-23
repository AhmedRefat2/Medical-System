
using Medical.Data;
using Medical.Data.Entities;
using Medical.Specifications;
using Medical.Specifications.ApplicationUser_;
using Microsoft.EntityFrameworkCore;

namespace Medical.Repositories
{
    public class ApplicationUserRepo(ApplicationDbContext dbContext)
    {
        private readonly ApplicationDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

        public async Task AddAsync(ApplicationUser user)
        {
            ArgumentNullException.ThrowIfNull(user);
            await _dbContext.Users.AddAsync(user);
        }

        public async Task AddAsync(IList<ApplicationUser> users)
        {
            ArgumentNullException.ThrowIfNull(users);
            await _dbContext.Users.AddRangeAsync(users);
        }

        public async Task<int> CommitAsync()
        => await _dbContext.SaveChangesAsync();

        public async Task<int> CountAsync(ApplicationUserSpecifications spec)
        {
            ApplicationUserSpecifications specsCp = new()
            {
                Criteria = spec.Criteria,
            };
            var query = ApplySpecificationEvaluator(_dbContext.Users, specsCp);
            return await query.CountAsync();
        }

        public void Delete(ApplicationUser user)
        {
            ArgumentNullException.ThrowIfNull(user);
            _dbContext.Users.Remove(user);
        }

        public async Task<Pagination<ApplicationUser>?> GetAllAsync(ApplicationUserSpecifications spec, PaginationSpecs pagination)
        {
            var query = ApplySpecificationEvaluator(_dbContext.Users, spec);
            var entities = await query.ToListAsync();
            return new()
            {
                Count = CountAsync(spec).Result,
                Page = pagination.PageIndex,
                Size = pagination.PageSize,
                Items = entities
            };
        }
        public async Task<IEnumerable<ApplicationUser>?> GetAllAsync(ApplicationUserSpecifications spec)
        {
            var query = ApplySpecificationEvaluator(_dbContext.Users, spec);
            var entities = await query.ToListAsync();
            return entities;
        }

        public async Task<ApplicationUser?> GetByIdAsync(int id)
        {
            return await _dbContext.Users.FindAsync(id);
        }

        public async Task<ApplicationUser?> GetAsync(ApplicationUserSpecifications spec)
        {
            var query = ApplySpecificationEvaluator(_dbContext.Users, spec);
            return await query.FirstOrDefaultAsync();
        }

        public void Update(ApplicationUser user)
        {
            ArgumentNullException.ThrowIfNull(user);
            _dbContext.Users.Update(user);
        }

        private IQueryable<ApplicationUser> ApplySpecificationEvaluator(IQueryable<ApplicationUser> query, ApplicationUserSpecifications specs)
        {
            return ApplicationUserSpecificationsEvaluator.Evaluate(query, specs);
        }
    }
}
