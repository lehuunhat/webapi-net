using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HienTangToc.Models
{
    public class FormDKhienModel
    {
        [Key]
        public int MaFormHien { get; set; }

        [ForeignKey("NguoiHien")]
        public int IdNH { get; set; }
        public required virtual NguoiHienModel NguoiHien { get; set; }

        public string? TinhNH { get; set; }
        public string? QuanNH { get; set; }
        public string? SoLanHien { get; set; }
        public DateTime NgaySinhNH { get; set; }
        public string? MaVanDon { get; set; }
        public string? DichVuChuyenPhat { get; set; }
        public bool? XacNhan { get; set; }
    }
}
