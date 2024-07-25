using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HienTangToc.Models
{
    public class NguoiHienModel
    {
        [Key]
        public int IdNH { get; set; }
        public required string HoTenNH { get; set; }
        public required DateTime NgayHien { get; set; } // Thiết lập giá trị mặc định
        public required string EmailNH { get; set; }
        public required int DoDaiTocNH { get; set; }
        public required string SDTNH{ get; set; }
        public required bool GioiTinhNH { get; set; }
        public required string DiaChiNH { get; set; }

        [ForeignKey("FormDKhienModel")]
        public required int MaFormHien { get; set; }
        public required virtual FormDKhienModel FormDKhienModel { get; set; }

    }

}
