using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AwesomeGamingRacing.Models
{
    public class User
    {
        public long RowId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }
        public Uri Image { get; set; }
        public object ImageBlob { get; set; }
        public string Bio { get; set; }
        public string Role { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        public byte[] Salt { get; set; }
    }
}
