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
    public class SoftwareController : ControllerBase
    {

        private readonly AssetManagementDBContext _context;
        private readonly IMapper _mapper;
        public SoftwareController(IMapper mapper, AssetManagementDBContext dBContext)
        {
            _mapper = mapper;
            _context = dBContext;
        }

        [HttpGet]
        [Route("all", Name = "GetAllSoftwares")]
        public async Task<ActionResult<IEnumerable<SoftwareLicenseDTO>>> GetSoftwareListAsync()
        {
            var softwares = await _context.SoftwareLicenses.ToListAsync();
            var softwareDTOs = _mapper.Map<List<SoftwareDTO>>(softwares);
            return Ok(softwareDTOs);
        }
    }
}
