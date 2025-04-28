
using bma_license_repository.CustomModel.SysJobRerpairStatus;
using bma_license_repository.Dto;
using bma_license_repository.Helper;

namespace bma_license_repository
{
    public interface ISysJobRepairStatusRepository
    {
        Task<List<GetSysJobRepairStatus>> GetAll();
        Task<List<GetSysJobRepairStatus>> GetAll(bool isActive);
        Task<GetSysJobRepairStatus> GetById(Guid id);
        Task<GetSysJobRepairStatus> GetBySeq(int seq);
    }
    public class SysJobRepairStatusRepository: ISysJobRepairStatusRepository
    {
        private readonly DevBmaContext _context;

        public SysJobRepairStatusRepository(DevBmaContext context)
        {
            _context = context;
        }

        public async Task<List<GetSysJobRepairStatus>> GetAll()
        {
            return await _context.SysJobRepairStatuses.ConvertToListAsync();
        }

        public async Task<List<GetSysJobRepairStatus>> GetAll(bool isActive)
        {
            return await _context.SysJobRepairStatuses.Where(a=> a.IsActive == isActive).ConvertToListAsync();
        }

        public async Task<GetSysJobRepairStatus> GetById(Guid id)
        {
            return await _context.SysJobRepairStatuses.ConvertToFirstOrDefaultAsync();
        }

        public async Task<GetSysJobRepairStatus> GetBySeq(int seq)
        {
            return await _context.SysJobRepairStatuses.Where(a => a.Seq == seq).ConvertToFirstOrDefaultAsync();
        }
    }
}
