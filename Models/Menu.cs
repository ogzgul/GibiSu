using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace GibiSu.Models
{
    public class Menu
    {
        public short Id { get; set; }

        [Column(TypeName = "nchar(100)")]
        [Required(ErrorMessage = "Bu alan gerekli")]
        [DisplayName("İsim")]
        [StringLength(100, ErrorMessage = "En fazla 100 karakter")]
        public string Name { get; set; }

        public List<Page>? Pages { get; set; }

    }
}
