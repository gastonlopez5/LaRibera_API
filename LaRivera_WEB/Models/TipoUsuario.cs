using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LaRivera_WEB.Models
{
    public class TipoUsuario
    {
        [Key]
        public int Id { get; set; }
        public String Rol { get; set; }
    }
}
