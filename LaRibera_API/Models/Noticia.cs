using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LaRibera_API.Models
{
    public class Noticia
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Titulo { get; set; }

        [Required]
        public string Detalle { get; set; }

        [Required]
        public string Fecha { get; set; }

        [Required]
        public Boolean Activo { get; set; }

        [Required]
        public Boolean Estado { get; set; }

        public int UsuarioId { get; set; }

        public int CategoriaId { get; set; }

        [ForeignKey("UsuariId")]
        public Usuario Usuario { get; set; }

        [ForeignKey("CategoriaId")]
        public Categoria Categoria { get; set; }
    }
}
