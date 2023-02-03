using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GibiSu.Models
{
    public class Product
    {
        public int Id { get; set; }

        [DisplayName("İsim")]
        [Required(ErrorMessage = "Bu alan zorunludur.")]
        [Column(TypeName = "nchar(100)")]
        [StringLength(100, ErrorMessage = "En fazla 100 karakter")]
        public string Name { get; set; }

        [Column(TypeName = "ntext")]
        [DisplayName("Açıklama")]
        [Required(ErrorMessage = "Bu alan zorunludur.")]
        public string Description { get; set; }

        [DisplayName("Fiyat")]
        [Required(ErrorMessage = "Bu alan zorunludur.")]
        public float Price { get; set; }

        [NotMapped]
        [Required(ErrorMessage ="Bu alan zorunludur.")]
        [DisplayName("Görsel")]
        public IFormFile FormImage { get; set; }

        [Required(ErrorMessage ="Bu alan zorunludur.")]
        [Column(TypeName = "image")]
        public byte[] Image { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Bu alan zorunludur.")]
        [DisplayName("Küçük Görsel")]
        public IFormFile SmallFormImage { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur.")]
        [Column(TypeName = "image")]
        public byte[] SmallImage { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur.")]
        [DisplayName("Satışa Kapalı")]
        public bool IsInactive { get; set; }

        [DisplayName("Hacim")]
        [Required(ErrorMessage = "Bu alan zorunludur.")]
        public float Volume { get; set; }

        [DisplayName("Malzeme Türü")]
        [Required(ErrorMessage = "Bu alan zorunludur.")]
        public bool Material { get; set; }
    }
}
