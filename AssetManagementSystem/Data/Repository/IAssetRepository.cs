using AssetManagementSystem.Models;

namespace AssetManagementSystem.Data.Repository
{
    public interface IAssetRepository
    {
        Task<List<Asset>> GetAllAsync();

        Task<Asset> GetAssetByIdAsync(string assetID, bool useNoTracking = false);

        Task<string> AddAsync(Asset dbRecord);

        Task<string> UpdateAsync(Asset dbRecord);

        Task<bool> DeleteAsync(Asset dbRecord);
        Task<bool> AssignToEmployeeAsync(string assetId, int employeeId);
    }
}
