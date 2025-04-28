
using bma_license_repository.CustomModel.TransKeyHistory;
using bma_license_repository.Dto;
using bma_license_repository.Helper;
using bma_license_repository.Request.TransKeyHistory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace bma_license_repository
{
    public interface ITransKeyHistoryResitory
    {
        Task<List<GetTransKeyHistory>> GetAll();
        Task<GetTransKeyHistory> GetById(Guid id);
        Task<List<GetTransKeyHistory>> GetByTransKeyId(Guid transKeyId);
        Task<List<GetTransKeyHistory>> GetByOrg(Guid orgId);
        Task<List<GetTransKeyHistory>> GetFilter(GetTransKeyHistoryFilterRequest param);
        Task InsertAsync(TransKeyHistory transKeyHistory);
        Task<List<TransKeyHistory>> GetForUpdate(List<Guid> listId);
        Task<List<TransKeyHistory>> GetForUpdate(Guid transKeyId);
        Task UpdateAsync(TransKeyHistory transKeyHistory);
        public Task<int> CountAllAsync();
    }

    public class TransKeyHistoryResitory : ITransKeyHistoryResitory
    {
        private readonly DevBmaContext _context;
        private readonly IMemoryCache _cache;
        public TransKeyHistoryResitory(DevBmaContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<List<GetTransKeyHistory>> GetAll()
        {
            return await _context.TransKeyHistories.Include(a => a.TransKey).ThenInclude(a => a.SysKeyType).Include(a => a.SysOrgranize).ConvertToListAsync();
        }

        public async Task<GetTransKeyHistory> GetById(Guid id)
        {
            return await _context.TransKeyHistories.Where(a => a.Id == id).Include(a => a.TransKey).ThenInclude(a => a.SysKeyType).Include(a => a.SysOrgranize).Include(a=> a.ActionByNavigation).ConvertToFirstOrDefaultAsync();
        }

        public async Task<List<GetTransKeyHistory>> GetByOrg(Guid orgId)
        {
            return await _context.TransKeyHistories.Where(a => a.SysOrgranizeId == orgId).Include(a => a.TransKey).ThenInclude(a => a.SysKeyType).Include(a => a.SysOrgranize).Include(a => a.ActionByNavigation).ConvertToListAsync();
        }

        public async Task<List<GetTransKeyHistory>> GetByTransKeyId(Guid transKeyId)
        {
            return await _context.TransKeyHistories.Where(a => a.TransKeyId == transKeyId).Include(a => a.TransKey).ThenInclude(a => a.SysKeyType).Include(a => a.SysOrgranize).Include(a => a.ActionByNavigation).ConvertToListAsync();
        }

        public async Task<List<GetTransKeyHistory>> GetFilter(GetTransKeyHistoryFilterRequest param)
        {
            var query = _context.TransKeyHistories.Include(a => a.TransKey).ThenInclude(a => a.SysKeyType).Include(a => a.SysOrgranize).Include(a => a.ActionByNavigation).AsQueryable();

            if (!string.IsNullOrEmpty(param.TextSearch))
            {
                query = query.Where(a => a.SysOrgranize.Name.Contains(param.TextSearch));
            }

            query = query.OrderBy(a => a.EndDate);

            var output = await query.AsNoTracking().ToListAsync();

            var cacheKey = "TransKeyHistoryCount";

            _cache.Set(cacheKey, output.Count());

            return await query.ConvertToListAsync();
        }

        public async Task<List<TransKeyHistory>> GetForUpdate(List<Guid> listId)
        {
            return await _context.TransKeyHistories.Where(a => listId.Contains(a.Id)).ToListAsync();
        }

        public async Task InsertAsync(TransKeyHistory transKeyHistory)
        {
            await _context.TransKeyHistories.AddAsync(transKeyHistory);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TransKeyHistory transKeyHistory)
        {
            _context.TransKeyHistories.Update(transKeyHistory);
            await _context.SaveChangesAsync();
        }

        public async Task<int> CountAllAsync()
        {
            // Use a caching library like MemoryCache, Redis, etc.
            var cacheKey = "TransKeyHistoryCount";
            if (!_cache.TryGetValue(cacheKey, out int count))
            {
                count = await _context.SysOrgranizes.CountAsync();

                // Set cache options and expiration as needed
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(1));

                _cache.Set(cacheKey, count, cacheOptions);
            }

            return count;
        }

        public async Task<List<TransKeyHistory>> GetForUpdate(Guid transKeyId)
        {
            return await _context.TransKeyHistories.Where(a => a.TransKeyId == transKeyId).ToListAsync();
        }
    }
}
