
using bma_license_repository.CustomModel.TransEquipment;
using bma_license_repository.Dto;
using bma_license_repository.Helper;
using Microsoft.EntityFrameworkCore;

namespace bma_license_repository
{
    public interface ITransEquipmentRepository
    {
        Task<List<GetTransEquipment>> GetAll();
        Task<List<GetTransEquipment>> GetAll(bool isActive);
        Task<List<GetTransEquipment>> GetByTransKeyId(Guid transKeyId);
        Task<GetTransEquipment> GetById(Guid id);
        Task<GetTransEquipment> GetByEquipmentCode(string equipmentCode);
        Task<TransEquipment> GetForUpdate(Guid id);
        Task<List<TransEquipment>> GetForUpdate(List<Guid> listId);
        Task InsertAsync(TransEquipment trans);
        Task InsertAsync(List<TransEquipment> list);
        Task UpdateAsync(TransEquipment trans);
        Task UpdateAsync();
        Task<bool> CheckUseEquipment(Guid transKeyId);
        Task<TransEquipment> GetForUpdate(string equipmentCode);
    }


    public class TransEquipmentRepository : ITransEquipmentRepository
    {
        private readonly DevBmaContext _context;

        public TransEquipmentRepository(DevBmaContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckUseEquipment(Guid transKeyId)
        {
            return await _context.TransEquipments.Where(a => a.IsActive && a.TransKeyId == transKeyId).CountAsync() > 1;
        }

        public async Task<List<GetTransEquipment>> GetAll()
        {
            return await _context.TransEquipments.ConvertToListAsync();
        }

        public async Task<List<GetTransEquipment>> GetAll(bool isActive)
        {
            return await _context.TransEquipments.Where(a => a.IsActive == isActive).ConvertToListAsync();
        }

        public async Task<GetTransEquipment> GetByEquipmentCode(string equipmentCode)
        {
            return await _context.TransEquipments.Where(a => a.EquipmentCode == equipmentCode).ConvertToFirstOrDefaultAsync();
        }

        public async Task<GetTransEquipment> GetById(Guid id)
        {
            return await _context.TransEquipments.Where(a => a.Id == id).ConvertToFirstOrDefaultAsync();
        }

        public async Task<List<GetTransEquipment>> GetByTransKeyId(Guid transKeyId)
        {
            return await _context.TransEquipments.Where(a => a.TransKeyId == transKeyId).ConvertToListAsync();
        }

        public async Task<TransEquipment> GetForUpdate(Guid id)
        {
            return await _context.TransEquipments.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<TransEquipment>> GetForUpdate(List<Guid> listId)
        {
            return await _context.TransEquipments.Where(a => listId.Contains(a.Id)).ToListAsync();
        }

        public async Task<TransEquipment> GetForUpdate(string equipmentCode)
        {
            return await _context.TransEquipments.Where(a => a.EquipmentCode == equipmentCode).FirstOrDefaultAsync();
        }

        public async Task InsertAsync(TransEquipment trans)
        {
            await _context.AddAsync(trans);
            await _context.SaveChangesAsync();
        }

        public async Task InsertAsync(List<TransEquipment> list)
        {
            await _context.TransEquipments.AddRangeAsync(list);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TransEquipment trans)
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
