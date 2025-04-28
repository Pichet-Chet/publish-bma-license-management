
using bma_license_repository.CustomModel.SysKeyType;
using bma_license_repository.Dto;
using bma_license_repository.Helper;

namespace bma_license_repository
{
    public interface ISysKeyTypeRepository
    {
        Task<List<GetSysKeyType>> GetAll();
        Task<List<GetSysKeyType>> GetAll(bool isActive);
        Task<GetSysKeyType> GetSysKeyTypeById(Guid id);
        Task<GetSysKeyType> GetByName(string name);
    }

    public class SysKeyTypeRepository : ISysKeyTypeRepository
    {
        private readonly DevBmaContext _context;

        public SysKeyTypeRepository(DevBmaContext context)
        {
            _context = context;
        }

        public async Task<List<GetSysKeyType>> GetAll()
        {
            return await _context.SysKeyTypes.OrderBy(a => a.Seq).ConvertToListAsync();
        }

        public async Task<List<GetSysKeyType>> GetAll(bool isActive)
        {
            return await _context.SysKeyTypes.Where(a => a.IsActive == isActive).OrderBy(a => a.Seq).ConvertToListAsync();
        }

        public async Task<GetSysKeyType> GetSysKeyTypeById(Guid id)
        {
            return await _context.SysKeyTypes.Where(a => a.Id == id).ConvertToFirstOrDefaultAsync();
        }

        public async Task<GetSysKeyType> GetByName(string name)
        {
            return await _context.SysKeyTypes.Where(a => a.Name.Contains(name)).ConvertToFirstOrDefaultAsync();
        }
        
    }
}
