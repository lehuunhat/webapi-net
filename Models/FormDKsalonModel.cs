using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HienTangToc.Models
{
    public class FormDKsalonModel
    {
        [Key]
        public int MaFormS { get; set; }
        [ForeignKey("SalonToc")]
        public int IdSalon { get; set; }
        public required virtual SalonTocModel SalonToc { get; set;}

        public string? LinkFB { get; set; }
        public string? TgHoatDong { get; set; }
        public string? Lienhe { get; set; }
        public string? Uudai { get; set; }
    }
}
