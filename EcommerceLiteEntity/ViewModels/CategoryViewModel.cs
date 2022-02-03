using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceLiteEntity.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "Kayıt Tarihi")]
        public DateTime RegisterDate { get; set; } = DateTime.Now;
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Kategori adının uzuluğu 2 ile 50 karakter arasında olmalıdır!")]
        public string CategoryName { get; set; }
        [StringLength(500, ErrorMessage = "Kategori açıklmasının uzunluğu en fazla 500 karakter olmalıdır!")]
        public string CategoryDescription { get; set;}
        public int? BaseCategoryId { get; set; } 
    }
}
