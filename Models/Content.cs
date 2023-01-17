using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace GibiSu.Models
{
    public class Content
    {
        public long Id { get; set; }

        [DisplayName("Başlık")]
        [Column(TypeName = "nchar(100)")]
        [StringLength(100, ErrorMessage = "En fazla 100 karakter")]
        public string? Title { get; set; }

        [Column(TypeName = "ntext")]
        [Required(ErrorMessage = "Bu alan gerekli")]
        [DisplayName("İçerik")]
        public string Text { get; set; }

        [DisplayName("Görsel")]
        [NotMapped]
        public IFormFile? FormImage { get; set; }

        [Column(TypeName = "image")]
        public byte[]? Image { get; set; }

        [Required(ErrorMessage = "Bu alan gerekli")]
        [DisplayName("Yeri")]
        public short Order { get; set; }

        [Required(ErrorMessage = "Bu alan gerekli")]
        [DisplayName("Tür")]
        public byte Type { get; set; }

        [Required(ErrorMessage = "Bu alan gerekli")]
        [DisplayName("Sayfa")]
        public string PageUrl { get; set; }

        [DisplayName("Sayfa")]
        [ForeignKey("PageUrl")]
        public Page? Page { get; set; }
    }
}
