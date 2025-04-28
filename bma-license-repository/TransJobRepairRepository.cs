
using bma_license_repository.CustomModel.TransJobRepair;
using bma_license_repository.Dto;
using bma_license_repository.Helper;
using bma_license_repository.Request.TransJobRepair;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace bma_license_repository
{

    public interface ITransJobRepairRepository
    {
        Task<List<GetTransJobRepair>> GetFilter(GetTransJobRepairFilterRequest param);
        Task<List<GetTransJobRepair>> GetAll();
        Task<List<GetTransJobRepair>> GetAll(bool isActive);
        Task InsertAsync(TransJobRepair transJobRepair);
        Task InsertAsync(List<TransJobRepair> list);
        Task<TransJobRepair> GetForUpdate(Guid id);
        Task UpdateAsync();
        Task<GetTransJobRepair> GetById(Guid id);
        public Task<int> CountAllAsync();
        Task<List<GetTransJobRepair>> GetByMonthYear(int month, int year);
    }

    public class TransJobRepairRepository : ITransJobRepairRepository
    {
        private readonly DevBmaContext _context;
        private readonly DateTime _datetime;
        private readonly IMemoryCache _cache;
        public TransJobRepairRepository(DevBmaContext context, IMemoryCache cache)
        {
            _context = context;
            _datetime = DateTime.Now;
            _cache = cache;
        }

        public async Task<List<GetTransJobRepair>> GetAll()
        {
            return await _context.TransJobRepairs
                .Include(a => a.CreatedByNavigation)
                .Include(a => a.SysRepairCategory)
                .Include(a => a.SysJobRepairStatus)
                .Include(a => a.TransKey)
                .Include(a => a.TransEquipment).ConvertToListAsync();
        }

        public async Task<List<GetTransJobRepair>> GetAll(bool isActive)
        {
            return await _context.TransJobRepairs.Where(a => a.IsActive == isActive)
                 .Include(a => a.CreatedByNavigation)
                .Include(a => a.SysRepairCategory)
                .Include(a => a.SysJobRepairStatus)
                .Include(a => a.TransEquipment)
                .Include(a => a.TransKey)
                .ConvertToListAsync();
        }

        public async Task<GetTransJobRepair> GetById(Guid id)
        {
            return await _context.TransJobRepairs.Where(a => a.Id == id && a.IsActive)
                 .Include(a => a.CreatedByNavigation)
                .Include(a => a.SysRepairCategory)
                .Include(a => a.SysJobRepairStatus)
                .Include(a => a.TransEquipment)
                .Include(a => a.TransKey)
                .ConvertToFirstOrDefaultAsync();
        }

        public async Task<List<GetTransJobRepair>> GetFilter(GetTransJobRepairFilterRequest param)
        {
            var query = _context.TransJobRepairs.Where(a => a.IsActive).Include(a => a.TransKey).Include(a => a.SysJobRepairStatus).Include(a => a.SysRepairCategory).AsQueryable();

            if (!string.IsNullOrEmpty(param.Id))
            {
                query = query.Where(a => a.Id == param.Id.ToGuid());
            }

            if (!string.IsNullOrEmpty(param.CategoryId))
            {
                query = query.Where(a => a.SysRepairCategoryId == param.CategoryId.ToGuid());
            }

            if (!string.IsNullOrEmpty(param.StatusId))
            {
                query = query.Where(a => a.SysJobRepairStatusId == param.StatusId.ToGuid());
            }

            if (param.DateOfRequest != null)
            {
                query = query.Where(a => a.DateOfRequest.HasValue && a.DateOfRequest >= param.DateOfRequest.Value);
            }

            if (param.DateOfFixed != null)
            {
                query = query.Where(a => a.DateOfFixed.HasValue && a.DateOfFixed >= param.DateOfFixed);
            }

            if (!string.IsNullOrEmpty(param.TextSearch))
            {
                query = query.Where(a => a.Name.Contains(param.TextSearch)
                    || a.Telephone.Contains(param.TextSearch)
                    || a.SysJobRepairStatus.Name.Contains(param.TextSearch)
                    || a.SysRepairCategory.Name.Contains(param.TextSearch));
            }

            var output = await query.AsNoTracking().ToListAsync();

            var cacheKey = "TransJobRepairCount";

            _cache.Set(cacheKey, output.Count());

            return await query
                .Include(a => a.CreatedByNavigation)
                .Include(a => a.SysRepairCategory)
                .Include(a => a.SysJobRepairStatus)
                .Include(a => a.TransEquipment)
                .Include(a => a.TransKey)
                .ConvertToListAsync();
        }

        public async Task InsertAsync(TransJobRepair transJobRepair)
        {
            await _context.TransJobRepairs.AddAsync(transJobRepair);
            await _context.SaveChangesAsync();
        }

        public async Task InsertAsync(List<TransJobRepair> list)
        {
            await _context.TransJobRepairs.AddRangeAsync(list);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<int> CountAllAsync()
        {
            // Use a caching library like MemoryCache, Redis, etc.
            var cacheKey = "TransJobRepairCount";
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

        public async Task<List<GetTransJobRepair>> GetByMonthYear(int month, int year)
        {
            return await _context.TransJobRepairs.Where(a => a.CreatedDate.Month == month && a.CreatedDate.Year == year)
                .Include(a => a.CreatedByNavigation)
                .Include(a => a.SysRepairCategory)
                .Include(a => a.SysJobRepairStatus)
                .Include(a => a.TransEquipment)
                .Include(a=> a.TransKey)
                .ConvertToListAsync();
        }

        public async Task<TransJobRepair> GetForUpdate(Guid id)
        {
            return await _context.TransJobRepairs.Where(a => a.Id == id).FirstOrDefaultAsync();
        }
    }
}
