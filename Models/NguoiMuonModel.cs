using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HienTangToc.Models
{
    public class NguoiMuonModel
    {
        [Key]
        public int IdNM { get; set; }
        public required string HoTenNM { get; set; }
        public required string SDTNM { get; set; }
        public required string EmailNM { get; set; }
        public required string DiaChiNM { get; set; }
        public required bool GioiTinhNM { get; set; }
        [ForeignKey("FormDKmuonModel")]
        public required int MaFormMuon { get; set; }
        public required virtual FormDKmuonModel FormDKmuonModel { get; set; }
    }
}
