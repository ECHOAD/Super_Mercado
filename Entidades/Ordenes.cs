using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Ordenes
    {
        [Key]
        public int Id { get; set; }

        public int Id_Usuario { get; set; }

        [ForeignKey("Id_Usuario")]
        public Usuario Usuarios { get; set; }
        
        
        public DateTime? Fecha { get; set; }

        public double? Total { get; set; }

        public int Estado { get; set; }




    }
}
