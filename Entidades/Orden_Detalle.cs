using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Orden_Detalle
    {

        [Key]
        public int Id { get; set; }


        public int Id_orden { get; set; }


        public int Id_Producto { get; set; }

        public int Cantidad { get; set; }



        [ForeignKey("Id_orden")]
        public Ordenes Ordenes { get; set; }

        [ForeignKey("Id_Producto")]
        public Producto Productos { get; set; }


    }
}
