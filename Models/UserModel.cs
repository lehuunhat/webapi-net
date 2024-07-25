using System.ComponentModel.DataAnnotations;

namespace HienTangToc.Models
{
    public class UserModel
    {
        [Key]
        public int Id { get; set; }
        public required string FullName { get; set; }
        public required string UserName { get; set; }
        [EmailAddress]
        public required string Email { get; set; }
        [MinLength(8)]
        public required string Password { get; set; }

        public required string Phone { get; set; }
    }
}
