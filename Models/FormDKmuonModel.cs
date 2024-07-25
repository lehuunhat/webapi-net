using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HienTangToc.Models
{
    public class FormDKmuonModel
    {
        [Key]
        public int MaFormMuon { get; set; }

        [ForeignKey("NguoiMuon")]
        public int IdNM { get; set; }
        public required virtual NguoiMuonModel NguoiMuon { get; set; }

        public string? TinhNM { get; set; }
        public string? QuanNM { get; set; }
        [StringLength(10)]
        public required string SDTNguoiThan { get; set; }
        public string? HoTenNguoiThan { get; set; }
        public DateTime TgPhatBenh { get; set; }
        public string? BenhDieuTri { get; set; }
        public string? BSDieuTri { get; set; }
        public string? ChanDoan { get; set; }
        public string? GiaiDoan { get; set; }
        public string? Tinhtrang { get; set; }
        public string? TgSuDung { get; set; }
        public DateTime NgayMuonToc { get; set; }
        public DateTime NgayTraToc { get; set; }
        public bool? CamKetHinhAnh { get; set; }
        public bool? CSHinhAnh { get; set; }
        public bool? CamKet { get; set; }
    }
}
