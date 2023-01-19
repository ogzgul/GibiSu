using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GibiSu.Models
{
    public class Order
    {
        public long Id { get; set; }

        [DisplayName("Sipariş Tarihi")]
        public DateTime? OrderDate { get; set; }

        [DisplayName("Toplam Fiyat")]
        public float TotalPrice { get; set; }

        [DisplayName("Teslim Tarihi")]
        public DateTime? DeliveryDate { get; set; }

        [Required(ErrorMessage ="Bu alan zorunludur.")]
        [DisplayName("Adres")]
        [Column(TypeName = "nchar(500)")]
        [StringLength(500, ErrorMessage = "En fazla 500 karakter")]
        public string Address { get; set; }

        [Required(ErrorMessage ="Bu alan zorunludur.")]
        [Column(TypeName = "nchar(10)")]
        [DisplayName("Tel No")]
        public string PhoneNumber { get; set; }

        [DisplayName("Müşteri")]
        public string? UserId { get; set; }

        [DisplayName("Müşteri")]
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        public List<OrderProduct>? OrderProducts { get; set; }
    }
}
