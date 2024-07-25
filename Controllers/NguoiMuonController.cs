using HienTangToc.Data;
using HienTangToc.Models;
using HienTangToc.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HienTangToc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NguoiMuonController : ControllerBase
    {
        private readonly NguoiMuonService _nguoiMuonService;
 /*       private readonly HocVanService _hocVanService;
        private readonly HopDongLaoDongService _hopDongLaoDongService;
        private readonly PhongBanService _phongBanService;
 */
        private readonly MyDbContext _context;

        public NguoiMuonController(NguoiMuonService nguoiMuonService, MyDbContext context)
        {
            _nguoiMuonService = nguoiMuonService;
            _context = context;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var nguoiMuons = await _nguoiMuonService.GetAllNguoiMuonsAsync();
            return Ok(nguoiMuons);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateNguoiMuon createNguoiMuon)
        {
            var nguoiMuon = new NguoiMuonModel
            {
                HoTenNM = createNguoiMuon.HoTenNM,
                SDTNM = createNguoiMuon.SDTNM,
                EmailNM = createNguoiMuon.EmailNM,
                GioiTinhNM = createNguoiMuon.GioiTinhNM,
                DiaChiNM = createNguoiMuon.DiaChiNM,
               

            };

            var result = await _nguoiMuonService.CreateNguoiMuonAsync(nguoiMuon);

            if (result.nguoiMuon == null)
            {
                return BadRequest(new { message = result.message });
            }

            var nguoiMuonWithDetails = await _context.NguoiMuon
                            .Include(nv => nv.FormDKmuonModel)
                            .FirstOrDefaultAsync(nh => nh.IdNM == nguoiMuon.IdNM);

            return Ok(new
            {
                data = new
                {
                    nguoiMuon = nguoiMuonWithDetails,
                    formDKmuonModel = nguoiMuonWithDetails.FormDKmuonModel

                },
                message = result.message
            });

        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(int idnm, [FromBody] NguoiMuonModel nguoiMuon)
        {
            var result = await _nguoiMuonService.UpdateNguoiMuonAsync(idnm, nguoiMuon);

            if (result.updatedNguoiMuon == null)
            {
                return BadRequest(new { message = result.message });
            }

            return Ok(new
            {
                originalData = result.originalNguoiMuon,
                data = result.updatedNguoiMuon,
                message = result.message
            });
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int idnm)
        {
            var result = await _nguoiMuonService.DeleteNguoiMuonAsync(idnm);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search([FromQuery] string nameOrCCCD)
        {
            var result = await _nguoiMuonService.SearchNguoiMuonAsync(nameOrCCCD);

            if (!result.nguoiMuons.Any())
            {
                return NotFound(new { message = result.message });
            }

            return Ok(new
            {
                data = result.nguoiMuons,
                message = result.message
            });
        }
        public class CreateNguoiMuon
        {
            public string HoTenNM { get; set; }
            public string SDTNM { get; set; }
            public string EmailNM { get; set; }
            public bool GioiTinhNM { get; set; }
            public string DiaChiNM { get; set; }
            
        }
    }
}
