using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LaRibera_API.Models
{
    public class Cuota
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Mes { get; set; }

        [Required]
        public string Comprobante { get; set; }

        [Required]
        public int Total { get; set; }

        public int UsuarioId { get; set; }

        [Required]
        public Boolean Estado { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; }
    }
}
