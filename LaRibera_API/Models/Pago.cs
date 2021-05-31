using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LaRibera_API.Models
{
    public class Pago
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Comprobante { get; set; }

        public int UsuarioId { get; set; }

        public int PedidoId { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; }

        [ForeignKey("PedidoId")]
        public Pedido Pedido { get; set; }
    }
}
