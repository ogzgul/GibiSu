using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace GibiSu.Models
{
    public class Page
    {
        [Key] 
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(100, ErrorMessage = "En fazla 100 karakter")]
        public string Url { get; set; }

        [DisplayName("Banner")]
        [NotMapped]
        [Required(ErrorMessage = "Bu alan zorunludur.")]
        public IFormFile FormImage { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur.")]
        [Column(TypeName = "image")]
        public byte[] Banner { get; set; }

        [DisplayName("Menü (opsiyonel)")]
        public short? MenuId { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur.")]
        public List<Content>? Contents { get; set; }

        [DisplayName("Menü")]
        [ForeignKey("MenuId")]
        public Menu? Menu { get; set; }

    }
}
