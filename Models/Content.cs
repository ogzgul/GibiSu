using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace GibiSu.Models
{
    public class Content
    {
        public long Id { get; set; }

        [Column(TypeName = "nchar(50)")]
        [Required(ErrorMessage = "Bu alan gerekli")]
        [DisplayName("Başlık")]
        [StringLength(50, ErrorMessage = "En fazla 50 karakter")]
        public string Title { get; set; }

        [Column(TypeName = "ntext")]
        [Required(ErrorMessage = "Bu alan gerekli")]
        [DisplayName("İçerik")]
        public string Text { get; set; }

        [DisplayName("Görsel")]
        [NotMapped]
        [Required(ErrorMessage = "Bu alan zorunludur.")]
        public IFormFile FormImage { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur.")]
        [Column(TypeName = "image")]
        public byte[] Image { get; set; }
    }
}
