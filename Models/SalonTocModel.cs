using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HienTangToc.Models
{
    public class SalonTocModel
    {
        [Key]
        public int IdSalon { get; set; }
        public required string TenSalon { get; set; }
        public required string TinhSalon { get; set; }
        public required string QuanSalon { get; set; }
        public required string DiaChiSalon { get; set; }

        [ForeignKey("FormDKsalonModel")]
        public required int MaFormS { get; set; }
        public required virtual FormDKsalonModel FormDKsalonModel { get; set; }
    }
}
