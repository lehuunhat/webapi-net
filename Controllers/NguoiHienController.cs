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
    public class NguoiHienController : ControllerBase
    {
        private readonly NguoiHienService _nguoiHienService;
        /*       private readonly HocVanService _hocVanService;
               private readonly HopDongLaoDongService _hopDongLaoDongService;
               private readonly PhongBanService _phongBanService;
        */
        private readonly MyDbContext _context;

        public NguoiHienController(NguoiHienService nguoiHienService, MyDbContext context)
        {
            _nguoiHienService = nguoiHienService;
            _context = context;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var nguoiHiens = await _nguoiHienService.GetAllNguoiHiensAsync();
            return Ok(nguoiHiens);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateNguoiHien createNguoiHien)
        {
            var nguoiHien = new NguoiHienModel
            {
                HoTenNH = createNguoiHien.HoTenNH,
                SDTNH = createNguoiHien.SDTNH,
                EmailNH = createNguoiHien.EmailNH,
                GioiTinhNH = createNguoiHien.GioiTinhNH,
                DiaChiNH = createNguoiHien.DiaChiNH,


            };

            var result = await _nguoiHienService.CreateNguoiHienAsync(nguoiHien);

            if (result.nguoiHien == null)
            {
                return BadRequest(new { message = result.message });
            }
          var nguoiHienWithDetails = await _context.NguoiHien
                           .Include(nv => nv.FormDKhienModel)
                           .FirstOrDefaultAsync(nh => nh.IdNH == nguoiHien.IdNH);

                       return Ok(new
                       {
                           data = new
                           {
                               nguoiHien = nguoiHienWithDetails,
                               formDKhienModel = nguoiHienWithDetails.FormDKhienModel
                        
                           },
                           message = result.message
                       });
           
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(int idnh, [FromBody] NguoiHienModel nguoiHien)
        {
            var result = await _nguoiHienService.UpdateNguoiHienAsync(idnh, nguoiHien);

            if (result.updatedNguoiHien == null)
            {
                return BadRequest(new { message = result.message });
            }

            return Ok(new
            {
                originalData = result.originalNguoiHien,
                data = result.updatedNguoiHien,
                message = result.message
            });
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int idnh)
        {
            var result = await _nguoiHienService.DeleteNguoiHienAsync(idnh);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search([FromQuery] string nameOrCCCD)
        {
            var result = await _nguoiHienService.SearchNguoiHienAsync(nameOrCCCD);

            if (!result.nguoiHiens.Any())
            {
                return NotFound(new { message = result.message });
            }

            return Ok(new
            {
                data = result.nguoiHiens,
                message = result.message
            });
        }
        public class CreateNguoiHien
        {
            public string HoTenNH { get; set; }
            public string SDTNH { get; set; }
            public string EmailNH { get; set; }
            public bool GioiTinhNH { get; set; }
            public string DiaChiNH { get; set; }

        }
    }
}
