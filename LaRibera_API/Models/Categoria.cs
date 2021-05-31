using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LaRibera_API.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string NombreCategoria { get; set; }
    }
}
