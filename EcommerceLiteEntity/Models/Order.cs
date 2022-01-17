using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceLiteEntity.Models
{
    [Table("Orders")]
    public class Order : TheBase<int>
    {
        [Required]
        [StringLength(7,ErrorMessage ="Satış Numarası gereklidir!")]
        public string OrderNumber { get; set; }
        public string CustomerTcNumber { get; set; }

        [ForeignKey("CustomerTcNumber")]
        public virtual Customer Customer { get; set; }

        public virtual List<OrderDetail> OrderDetailList { get; set; }
    }
}
