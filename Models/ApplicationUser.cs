using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace GibiSu.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Column(TypeName = "nchar(100)")]
        [Required(ErrorMessage = "Bu alan gerekli")]
        [DisplayName("Ad Soyad*")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "En az 5, En fazla 100 karakter")]
        public string Name { get; set; }

        public bool Deleted { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Bu alan gerekli")]
        [DisplayName("Şifre*")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "En az 8, En fazla 100 karakter")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Bu alan gerekli")]
        [DisplayName("Şifre(tekrar)*")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "En az 8, En fazla 100 karakter")]
        [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Bu alan gerekli")]
        [DisplayName("Üyelik sözleşmesini kabul ediyorum.")]
        public bool Agreed { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur.")]
        [DisplayName("Adres")]
        [Column(TypeName = "nchar(500)")]
        [StringLength(500, ErrorMessage = "En fazla 500 karakter")]
        public string Address { get; set; }

        public List<Order> Orders { get; set; }
    }

}
