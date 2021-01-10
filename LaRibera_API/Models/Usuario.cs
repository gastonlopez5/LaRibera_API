using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LaRibera_API.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        public string Email { get; set; }

        public string Clave { get; set; }

        public int RolId { get; set; }

        [ForeignKey("RolId")]
        public TipoUsuario TipoUsuario { get; set; }

        public int GrupoId { get; set; }

        [ForeignKey("GrupoId")]
        public Grupo Grupo { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string Telefono { get; set; }

        public string Dni { get; set; }

        public string FotoPerfil { get; set; }

        public Boolean Estado { get; set; }
    }
}
