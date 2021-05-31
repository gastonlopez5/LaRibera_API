using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LaRibera_API.Models
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string NombreGrupo { get; set; }

        [Required]
        public string Descripcion { get; set; }

        [Required]
        public double Precio { get; set; }
    }
}
