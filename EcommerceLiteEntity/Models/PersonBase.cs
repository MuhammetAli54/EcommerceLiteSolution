using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceLiteEntity.Models
{
    public class PersonBase : IPerson
    {
        [Key]
        [Column(Order =1)]
        [MinLength(11)]
        [StringLength(11,ErrorMessage ="Tc Kimlik Numarası 11 haneli olmalıdır!")]
        public string TcNumber { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime LastActiveTime { get; set; }
    }
}
