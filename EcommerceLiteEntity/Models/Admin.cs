using EcommerceLiteEntity.IdentityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceLiteEntity.Models
{
    [Table("Admins")]
   public class Admin:PersonBase
    {
        public string UserId { get; set; } //Identity Model'in Id değeri burada Foreign Key olacaktır

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
