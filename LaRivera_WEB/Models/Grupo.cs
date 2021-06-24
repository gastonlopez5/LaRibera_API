﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LaRivera_WEB.Models
{
    public class Grupo
    {
        [Key]
        public int Id { get; set; }

        public String NombreGrupo { get; set; }

        public int IdResponsable { get; set; }


        [ForeignKey("IdResponsable")]
        public Usuario Usuario { get; set; }
    }
}
