using AssetManagementSystem.Data;
using AssetManagementSystem.Data.Repository;
using AssetManagementSystem.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetController : ControllerBase
    {
        private readonly AssetManagementDBContext _context;
        private readonly IMapper _mapper;
        private readonly IAssetRepository _assetRepository;
        private readonly ILogger<AssetController> _logger;

        public AssetController(IMapper mapper, IAssetRepository assetRepository, ILogger<AssetController> logger, AssetManagementDBContext dBContext)
        {
            _assetRepository = assetRepository; 
            _mapper = mapper;
            _logger = logger;
            _context = dBContext;
        }

        [HttpGet]
        [Route("all", Name = "GetAllAssets")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<AssetDTO>>> GetAssetsAsyn()
        {
            try
            {
                _logger.LogInformation("GetStudents method started");

                var assets = await _assetRepository.GetAllAsync();

                // repository returned null -> treat as not found
                if (assets == null)
                {
                    _logger.LogWarning("No assets found in the repository.");
                    return NotFound("No assets found.");
                }

                var assetDtos = _mapper.Map<List<AssetDTO>>(assets);

                // empty list -> no content
                if (assetDtos == null || assetDtos.Count == 0)
                {
                    _logger.LogInformation("No assets found after mapping to DTOs.");
                     return NoContent();
                }

                return Ok(assetDtos);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "ArgumentException occurred while retrieving assets.");
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "InvalidOperationException occurred while retrieving assets.");
                return StatusCode(StatusCodes.Status409Conflict, ex.Message);
            }
            catch (Exception)
            {
                _logger.LogError("An unexpected error occurred while retrieving assets.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred while retrieving assets.");
            }
        }

        [HttpPost]
        [Route("add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AssetDTO>> AddAssetAsync([FromBody] AddAssetDTO assetDto)
        {
            try
            {
                if (assetDto == null)
                {
                    _logger.LogWarning("AddAsset called with null body.");
                    return BadRequest("Asset data is null.");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("AddAsset called with invalid model state.");
                    return BadRequest(ModelState);
                }

                _logger.LogInformation("AddAsset method started for asset: {Name}", assetDto.Name);

                var asset = _mapper.Map<Asset>(assetDto);
                var id = await _assetRepository.AddAsync(asset);

                var createdAssetDto = _mapper.Map<AssetDTO>(asset);

                _logger.LogInformation("Asset created successfully with ID: {AssetId}", id);
                return CreatedAtRoute("GetAllAssets", new { id }, createdAssetDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while adding an asset.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred while adding the asset.");
            }
        }

        [HttpPut("edit/{assetId}", Name = "UpdateAssetById")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateAssetAsync(string assetId, [FromBody] AddAssetDTO assetDto)
        {
            try
            {
                if (string.IsNullOrEmpty(assetId))
                {
                    _logger.LogWarning("UpdateAsset called with null or empty assetId.");
                    return BadRequest("Asset ID is required.");
                }

                if (assetDto == null)
                {
                    _logger.LogWarning("UpdateAsset called with null body for assetId: {AssetId}", assetId);
                    return BadRequest("Asset data is null.");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("UpdateAsset called with invalid model state for assetId: {AssetId}", assetId);
                    return BadRequest(ModelState);
                }

                _logger.LogInformation("UpdateAsset method started for assetId: {AssetId}", assetId);

                var existingAsset = await _assetRepository.GetAssetByIdAsync(assetId, true);
                if (existingAsset == null)
                {
                    _logger.LogWarning("Asset not found with ID: {AssetId}", assetId);
                    return NotFound($"Asset with ID '{assetId}' not found.");
                }

                _mapper.Map(assetDto, existingAsset);
                if (existingAsset.WarrantyCard != null)
                    existingAsset.WarrantyCard.Id = existingAsset.Id;

                if (assetDto.EmployeeName != null)
                {
                    var employee = await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeId == existingAsset.EmployeeId);
                    if (employee != null)
                    {
                        employee.EmployeeName = assetDto.EmployeeName;
                        existingAsset.Employee = employee;
                    }
                }

                await _assetRepository.UpdateAsync(existingAsset);

                _logger.LogInformation("Asset updated successfully with ID: {AssetId}", assetId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while updating asset with ID: {AssetId}", assetId);
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred while updating the asset.");
            }
        }

        [HttpDelete("Delete/{id}", Name = "DeleteAssetById")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteAssetAsync(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    _logger.LogWarning("DeleteAsset called with null or empty id.");
                    return BadRequest("Asset ID is required.");
                }

                _logger.LogInformation("DeleteAsset method started for id: {AssetId}", id);

                var existingAsset = await _assetRepository.GetAssetByIdAsync(id, true);
                if (existingAsset == null)
                {
                    _logger.LogWarning("Asset not found with ID: {AssetId}", id);
                    return NotFound($"Asset with ID '{id}' not found.");
                }

                await _assetRepository.DeleteAsync(existingAsset);

                _logger.LogInformation("Asset deleted successfully with ID: {AssetId}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while deleting asset with ID: {AssetId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred while deleting the asset.");
            }
        }

        [HttpPut("assign/{assetid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AssignAsset([FromBody] AssetAssignmentDTO dto)
        {
            try
            {
                if (dto == null)
                {
                    _logger.LogWarning("AssignAsset called with null body.");
                    return BadRequest("Assignment data is null.");
                }

                if (string.IsNullOrWhiteSpace(dto.AssetId) || dto.EmployeeId <= 0)
                {
                    _logger.LogWarning("AssignAsset called with invalid data. AssetId: {AssetId}, EmployeeId: {EmployeeId}", dto.AssetId, dto.EmployeeId);
                    return BadRequest("AssetId and EmployeeId are required.");
                }

                _logger.LogInformation("AssignAsset method started for AssetId: {AssetId}, EmployeeId: {EmployeeId}", dto.AssetId, dto.EmployeeId);

                await _assetRepository.AssignToEmployeeAsync(dto.AssetId, dto.EmployeeId);

                _logger.LogInformation("Asset {AssetId} assigned to employee {EmployeeId} successfully.", dto.AssetId, dto.EmployeeId);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Assignment failed for AssetId: {AssetId}, EmployeeId: {EmployeeId}", dto.AssetId, dto.EmployeeId);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while assigning asset {AssetId} to employee {EmployeeId}.", dto.AssetId, dto.EmployeeId);
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred while assigning the asset.");
            }
        }

    }
}
