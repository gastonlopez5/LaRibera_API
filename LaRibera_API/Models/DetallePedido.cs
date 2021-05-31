using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LaRibera_API.Models
{
    public class DetallePedido
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        public int Estado { get; set; }

        public int PedidoId { get; set; }

        public int ProductoId { get; set; }

        [ForeignKey("PedidoId")]
        public Pedido Pedido { get; set; }

        [ForeignKey("ProductoId")]
        public Producto Producto { get; set; }
    }
}
