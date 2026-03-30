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
    public class EmployeeController : ControllerBase
    {

        private readonly AssetManagementDBContext _context;
        private readonly IMapper _mapper;
        private readonly IAssetRepository _assetRepository;
        public EmployeeController(IMapper mapper, AssetManagementDBContext dBContext)
        {
            //_assetRepository = assetRepository;
            _mapper = mapper;
            _context = dBContext;
        }

        [HttpGet]
        [Route("all", Name = "GetAllEmployees")]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetEmployeesAsyn()
        {
            var employees = await _context.Employees.ToListAsync();
            var employeeDTOs = _mapper.Map<List<EmployeeDTO>>(employees);
            return Ok(employeeDTOs);
        }
    }
}
