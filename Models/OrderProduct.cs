using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GibiSu.Models
{
    public class OrderProduct
    {
        [Required(ErrorMessage = "Bu alan zorunludur.")]
        [DisplayName("Ürün")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur.")]
        [DisplayName("Sipariş Numarası")]
        public long OrderId { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur.")]
        [DisplayName("Adet")]
        public int Amount { get; set; }

        [DisplayName("Birim Fiyat")]
        public float Price { get; set; }

        [DisplayName("Toplam Fiyat")]
        public float TotalPrice { get; set; }

        [DisplayName("Ürün")]
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

        [DisplayName("Sipariş Numarası")]
        [ForeignKey("OrderId")]
        public Order? Order { get; set; }
    }
}
