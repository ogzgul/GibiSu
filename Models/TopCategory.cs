using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GibiSu.Models
{
	public class TopCategory
	{
		public short Id { get; set; }

		[Required(ErrorMessage = "Boş bırakılamaz!")]
		[Column(TypeName = "nchar(50)")]
		[Display(Name = "İsim")]
		[StringLength(50, ErrorMessage = "En fazla 50 karakter!")]
		public string Name { get; set; }

		public List<Category>? Categories { get; set; }
	}
}
