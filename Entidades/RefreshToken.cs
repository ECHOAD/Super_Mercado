using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }


        public int Id_Usuario { get; set; }

        [MaxLength(200)]

        public string Token { get; set; }

        public DateTime? Expiry_Date { get; set; }


        [ForeignKey("Id_Usuario")]
        public Usuario Usuario { get; set; }
    }
}
