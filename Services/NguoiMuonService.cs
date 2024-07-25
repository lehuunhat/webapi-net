using HienTangToc.Data;
using HienTangToc.Models;
using Microsoft.EntityFrameworkCore;

namespace HienTangToc.Services
{
    public class NguoiMuonService
    {
        private readonly MyDbContext _context;

        public NguoiMuonService(MyDbContext context)
        {
            _context = context;
        }

        public async Task<List<NguoiMuonModel>> GetAllNguoiMuonsAsync(int pageNumber = 1, int pageSize = 10)
        {
            return await _context.NguoiMuon
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<(NguoiMuonModel nguoiMuon, string message)> CreateNguoiMuonAsync(NguoiMuonModel nguoiMuon)
        {
            var existingNguoiMuonByName = await _context.NguoiMuon
                .FirstOrDefaultAsync(sp => sp.HoTenNM == nguoiMuon.HoTenNM);

            if (existingNguoiMuonByName != null)
            {
                return (null, "Tên sản phẩm đã tồn tại.");
            }

/*            if (imageFile == null && string.IsNullOrEmpty(nguoiMuon.Anh_SP))
            {
                return (null, "Bạn phải nhập ảnh sản phẩm hoặc chọn tệp ảnh.");
            }

            if (imageFile != null)
            {
                if (imageFile.Length > 0)
                {
                    var fileExtension = Path.GetExtension(imageFile.FileName).ToLower();
                    if (fileExtension != ".jpg" && fileExtension != ".jpeg")
                    {
                        return (null, "File ảnh phải có định dạng JPG.");
                    }

                    var fileName = $"{Guid.NewGuid()}{fileExtension}";
                    var filePath = Path.Combine("Data/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    sanPham.Anh_SP = $"/images/{fileName}";
                }
                else
                {
                    return (null, "File ảnh không hợp lệ.");
                }
            }
*/
            _context.NguoiMuon.Add(nguoiMuon);
            await _context.SaveChangesAsync();

            return (nguoiMuon, "Tạo sản phẩm thành công.");
        }
        public async Task<(NguoiMuonModel originalNguoiMuon, NguoiMuonModel updatedNguoiMuon, string message)> UpdateNguoiMuonAsync(int idnm, NguoiMuonModel nguoiMuonUpdate)
        {
            var existingNguoiMuon = await _context.NguoiMuon.FindAsync(idnm);
            if (existingNguoiMuon == null)
            {
                return (null, null, "Người mượn không tồn tại.");
            }

            var nguoiMuonWithSameName = await _context.NguoiMuon
                .FirstOrDefaultAsync(nm => nm.HoTenNM == nguoiMuonUpdate.HoTenNM && nm.IdNM != idnm);

            if (nguoiMuonWithSameName != null)
            {
                return (null, null, "Tên người mượn đã tồn tại.");
            }

            var originalNguoiMuon = new NguoiMuonModel
            {
                HoTenNM = existingNguoiMuon.HoTenNM,
                SDTNM = existingNguoiMuon.SDTNM,
                EmailNM = existingNguoiMuon.EmailNM,
                GioiTinhNM = existingNguoiMuon.GioiTinhNM,
                DiaChiNM = existingNguoiMuon.DiaChiNM
            };

            existingNguoiMuon.HoTenNM = nguoiMuonUpdate.HoTenNM;
            existingNguoiMuon.SDTNM = nguoiMuonUpdate.SDTNM;
            existingNguoiMuon.EmailNM = nguoiMuonUpdate.EmailNM;
            existingNguoiMuon.GioiTinhNM = nguoiMuonUpdate.GioiTinhNM;
            existingNguoiMuon.DiaChiNM = nguoiMuonUpdate.DiaChiNM;

            _context.NguoiMuon.Update(existingNguoiMuon);
            await _context.SaveChangesAsync();

            return (originalNguoiMuon, existingNguoiMuon, "Cập nhật người mượn thành công.");
        }

        public async Task<DeleteNguoiMuonResponse> DeleteNguoiMuonAsync(int idnm)
        {
            var existingNguoiMuon = await _context.NguoiMuon.FindAsync(idnm);
            if (existingNguoiMuon == null)
            {
                return new DeleteNguoiMuonResponse
                {
                    Success = false,
                    Message = "Không tìm thấy người mượn"
                };
            }

            _context.NguoiMuon.Remove(existingNguoiMuon);
            await _context.SaveChangesAsync();

            return new DeleteNguoiMuonResponse
            {
                Success = true,
                Message = "Xóa thành công"
            };
        }

        public async Task<(List<NguoiMuonModel> nguoiMuons, string message)> SearchNguoiMuonAsync(string nameorPrice)
        {
            var query = _context.NguoiMuon.AsQueryable();

            if (!string.IsNullOrWhiteSpace(nameorPrice))
            {
                if (decimal.TryParse(nameorPrice, out decimal price))
                {
                    query = query.Where(nm => nm.IdNM == price);
                }
                else
                {
                    query = query.Where(nm => nm.HoTenNM.Contains(nameorPrice));
                }
            }

            var nguoiMuons = await query.ToListAsync();

            if (!nguoiMuons.Any())
            {
                return (nguoiMuons, "Không tìm thấy người mượn nào khớp với điều kiện tìm kiếm.");
            }

            return (nguoiMuons, "Tìm kiếm thành công.");
        }



        public class DeleteNguoiMuonResponse
        {
            public bool Success { get; set; }
            public string Message { get; set; }
        }
    }
}
