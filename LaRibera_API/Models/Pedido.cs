using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LaRibera_API.Models
{
    public class Pedido
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Fecha { get; set; }

        [Required]
        public double Total { get; set; }

        public int UsuarioId { get; set; }

        [ForeignKey("UsuariId")]
        public Usuario Usuario { get; set; }
    }
}
