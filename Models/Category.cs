using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GibiSu.Models
{
	public class Category
	{
		public short Id { get; set; }

		[Required(ErrorMessage = "Boş bırakılamaz!")]
		[Column(TypeName = "nchar(50)")]
		[Display(Name = "İsim")]
		[StringLength(50, ErrorMessage = "En fazla 50 karakter!")]
		public string Name { get; set; }

		[Column(TypeName = "nchar(100)")]
		[Display(Name = "Bilgi")]
		[StringLength(100, ErrorMessage = "En fazla 100 karakter!")]
		public string? Info { get; set; }

		[Display(Name = "Ana Kategori (Opsiyonel) ")]
		public short? TopCategoryId { get; set; }

		[Display(Name = "Ana Kategori")]
		[ForeignKey("TopCategoryId")]
		public TopCategory? TopCategory { get; set; }
	}
}
