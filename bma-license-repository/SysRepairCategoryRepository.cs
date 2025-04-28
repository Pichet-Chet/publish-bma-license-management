using bma_license_repository.CustomModel.SysRepairCetegory;
using bma_license_repository.Dto;
using bma_license_repository.Helper;

namespace bma_license_repository
{
    public interface ISysRepairCategoryRepository
    {
        Task<List<GetSysRepairCategory>> GetAll();
        Task<List<GetSysRepairCategory>> GetAll(bool isActive);
        Task<GetSysRepairCategory> GetById(Guid id);
    }

    public class SysRepairCategoryRepository: ISysRepairCategoryRepository
    {
        private readonly DevBmaContext _context;

        public SysRepairCategoryRepository(DevBmaContext context)
        {
            _context = context;
        }

        public async Task<List<GetSysRepairCategory>> GetAll()
        {
            return await _context.SysRepairCategories.ConvertToListAsync();
        }

        public async Task<List<GetSysRepairCategory>> GetAll(bool isActive)
        {
            return await _context.SysRepairCategories.Where(a=> a.IsActive == isActive).ConvertToListAsync();
        }

        public async Task<GetSysRepairCategory> GetById(Guid id)
        {
            return await _context.SysRepairCategories.Where(a => a.Id == id && a.IsActive).ConvertToFirstOrDefaultAsync();
        }
    }
}
