using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EcommerceLiteUI.Models
{
    public class CartViewModel
    {
        public int Id { get; set; }
        public DateTime RegisterDate { get; set; } = DateTime.Now;
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Ürün adının uzunluğu 2 ile 50 karakter aralığında olmalıdır!")]
        public string ProductName { get; set; }
        [StringLength(500, ErrorMessage = "Ürün açlıklamasının uzunluğu en fazla 500 karakter olmalıdır!")]
        public string Description { get; set; }

        [StringLength(8, ErrorMessage = "Ürün kodunun uzunluğu en fazla 8 karakter olmalıdır!")]
        public string ProductCode { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int CategoryId { get; set; }

    }
}