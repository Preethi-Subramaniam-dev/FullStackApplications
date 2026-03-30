using AssetManagementSystem.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AssetManagementSystem.Data.Repository
{
    public class AssetRepository : IAssetRepository
    {

        private readonly AssetManagementDBContext _context;

        public AssetRepository(AssetManagementDBContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<string> AddAsync(Asset asset)
        {
            if(asset.Status == null) {
                var status = await _context.Statuses.FirstOrDefaultAsync(s => s.StatusName == "Available");
                asset.Status = status ?? throw new InvalidOperationException("Default status 'Available' not found.");
            }

            if(asset.WarrantyCard == null) {
                asset.WarrantyCard = new WarrantyCard
                {
                    Provider = asset.Name + " _Warranty", // Example provider name based on asset
                    StartDate = DateTime.UtcNow,
                    ExpiryDate = DateTime.UtcNow.AddYears(1), // Default 1 year warranty
                };

                await _context.WarrantyCards.AddAsync(asset.WarrantyCard);
            }
            // Ensure we have either a valid Id or a name we can resolve
            //if (asset.StatusId != 0)
            //{
            //    var status = await _context.Statuses.FindAsync(asset.StatusId);
            //    if (status == null)
            //        throw new InvalidOperationException($"StatusId '{asset.StatusId}' not found.");

            //    // If client supplied a Status nav with a different name, prefer DB value or reject:
            //    // if (!string.IsNullOrEmpty(asset.Status?.StatusName) && asset.Status.StatusName != status.StatusName)
            //    //     throw new InvalidOperationException("Provided StatusName does not match StatusId.");

            //    // Prevent EF from inserting a new Status — keep the FK only
            //    asset.Status = null;
            //}
            //else if (!string.IsNullOrEmpty(asset.Status?.StatusName))
            //{
            //    var status = await _context.Statuses
            //        .FirstOrDefaultAsync(s => s.StatusName == asset.Status.StatusName);

            //    if (status == null)
            //        throw new InvalidOperationException($"Status '{asset.Status.StatusName}' not found.");

            //    asset.StatusId = status.StatusId;
            //    asset.Status = null;
            //}
            //else
            //{
            //    throw new InvalidOperationException("Asset must include a valid StatusId or StatusName.");
            //}

            _context.Assets.Add(asset);
            await _context.SaveChangesAsync();
            asset.WarrantyCard.AssetId = asset.Id; // Set FK after asset has Id
            return asset.AssetId;
        }

        public async Task<bool> DeleteAsync(Asset asset)    
        {
            _context.Assets.Remove(asset);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Asset>> GetAllAsync()
        {
            return await _context.Assets
                .Include(a => a.Status)
                .Include(a => a.Employee)
                .Include(a => a.WarrantyCard)
                .Include(a => a.AssetSoftwares)
                .ThenInclude(asw => asw.SoftwareLicense)
                .ToListAsync();
        }

        public Task<Asset> GetAssetByIdAsync(string assetId, bool useNoTracking = false)
        {
            var query = _context.Assets
               .Include(a => a.Employee)
               .Include(a => a.WarrantyCard)
               .Include(a => a.AssetSoftwares).ThenInclude(asw => asw.SoftwareLicense)
               .AsQueryable();

            return useNoTracking
                ? query.AsNoTracking().FirstOrDefaultAsync(a => a.AssetId == assetId)
                : query.FirstOrDefaultAsync(a => a.AssetId == assetId);
        }

        public async Task<string> UpdateAsync(Asset asset)
        {
            _context.Update(asset);
            await _context.SaveChangesAsync();
            return asset.AssetId;
        }

        public async Task<bool> AssignToEmployeeAsync(string assetId, int employeeID)
        {
            var asset = await _context.Assets
                .Include(a => a.Employee)
                .Include(a => a.Status)
                .FirstOrDefaultAsync(a => a.AssetId == assetId);

            if (asset == null)
                throw new InvalidOperationException($"Asset '{assetId}' not found.");

            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeID);

            if (employee == null)
                throw new InvalidOperationException($"Employee '{employeeID}' not found.");

            var assignedStatus = await _context.Statuses
                .FirstOrDefaultAsync(s => s.StatusName == "Assigned");

            if (assignedStatus == null)
                throw new InvalidOperationException("Status 'Assigned' not found.");

            // Associate employee and status on the tracked asset
            asset.EmployeeId = employee.EmployeeId;
            asset.Employee = employee;             // optional: tracked entity is fine
            asset.StatusId = assignedStatus.StatusId;
            asset.Status = assignedStatus;

            _context.Update(asset);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}