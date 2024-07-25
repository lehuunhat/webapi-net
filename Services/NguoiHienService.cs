using HienTangToc.Data;
using HienTangToc.Models;
using Microsoft.EntityFrameworkCore;

namespace HienTangToc.Services
{
    public class NguoiHienService
    {
        private readonly MyDbContext _context;

        public NguoiHienService(MyDbContext context)
        {
            _context = context;
        }

        public async Task<List<NguoiHienModel>> GetAllNguoiHiensAsync(int pageNumber = 1, int pageSize = 10)
        {
            return await _context.NguoiHien
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<(NguoiHienModel nguoiHien, string message)> CreateNguoiHienAsync(NguoiHienModel nguoiHien)
        {
            var existingNguoiHienByName = await _context.NguoiHien
                .FirstOrDefaultAsync(sp => sp.HoTenNH == nguoiHien.HoTenNH);

            if (existingNguoiHienByName != null)
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
            _context.NguoiHien.Add(nguoiHien);
            await _context.SaveChangesAsync();

            return (nguoiHien, "Tạo người hiến thành công.");
        }
        public async Task<(NguoiHienModel originalNguoiHien, NguoiHienModel updatedNguoiHien, string message)> UpdateNguoiHienAsync(int idnm, NguoiHienModel nguoiHienUpdate)
        {
            var existingNguoiHien = await _context.NguoiHien.FindAsync(idnm);
            if (existingNguoiHien == null)
            {
                return (null, null, "Người hiến không tồn tại.");
            }

            var nguoiHienWithSameName = await _context.NguoiHien
                .FirstOrDefaultAsync(nm => nm.HoTenNH == nguoiHienUpdate.HoTenNH && nm.IdNH != idnm);

            if (nguoiHienWithSameName != null)
            {
                return (null, null, "Tên người hiến đã tồn tại.");
            }

            var originalNguoiHien = new NguoiHienModel
            {
                HoTenNH = existingNguoiHien.HoTenNH,
                SDTNH = existingNguoiHien.SDTNH,
                EmailNH = existingNguoiHien.EmailNH,
                GioiTinhNH = existingNguoiHien.GioiTinhNH,
                DiaChiNH = existingNguoiHien.DiaChiNH
            };

            existingNguoiHien.HoTenNH = nguoiHienUpdate.HoTenNH;
            existingNguoiHien.SDTNH = nguoiHienUpdate.SDTNH;
            existingNguoiHien.EmailNH = nguoiHienUpdate.EmailNH;
            existingNguoiHien.GioiTinhNH = nguoiHienUpdate.GioiTinhNH;
            existingNguoiHien.DiaChiNH = nguoiHienUpdate.DiaChiNH;

            _context.NguoiHien.Update(existingNguoiHien);
            await _context.SaveChangesAsync();

            return (originalNguoiHien, existingNguoiHien, "Cập nhật người hiến thành công.");
        }

        public async Task<DeleteNguoiHienResponse> DeleteNguoiHienAsync(int idnh)
        {
            var existingNguoiHien = await _context.NguoiHien.FindAsync(idnh);
            if (existingNguoiHien == null)
            {
                return new DeleteNguoiHienResponse
                {
                    Success = false,
                    Message = "Không tìm thấy người hiến"
                };
            }

            _context.NguoiHien.Remove(existingNguoiHien);
            await _context.SaveChangesAsync();

            return new DeleteNguoiHienResponse
            {
                Success = true,
                Message = "Xóa thành công"
            };
        }

        public async Task<(List<NguoiHienModel> nguoiHiens, string message)> SearchNguoiHienAsync(string nameorPrice)
        {
            var query = _context.NguoiHien.AsQueryable();

            if (!string.IsNullOrWhiteSpace(nameorPrice))
            {
                if (decimal.TryParse(nameorPrice, out decimal price))
                {
                    query = query.Where(nm => nm.IdNH == price);
                }
                else
                {
                    query = query.Where(nm => nm.HoTenNH.Contains(nameorPrice));
                }
            }

            var nguoiHiens = await query.ToListAsync();

            if (!nguoiHiens.Any())
            {
                return (nguoiHiens, "Không tìm thấy người mượn nào khớp với điều kiện tìm kiếm.");
            }

            return (nguoiHiens, "Tìm kiếm thành công.");
        }



        public class DeleteNguoiHienResponse
        {
            public bool Success { get; set; }
            public string Message { get; set; }
        }
    }
}
