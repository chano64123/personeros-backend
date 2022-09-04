using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model {
    public class Usuario {
        [Key]
        public int idUsuario { get; set; }
        [Required]
        public int idTipoUsuario { get; set; }
        [Required]
        public int idPersona { get; set; }
        [Required]
        public string nombreUsuario { get; set; }
        [Required]
        public string clave { get; set; }
        [Required]
        public string cantidadMaximaMesas { get; set; }
        [Required]
        public string cantidadMaximaInstituciones { get; set; }
        [ForeignKey("idTipoUsuario")]
        public TipoUsuario tipoUsuario { get; set; }
        [ForeignKey("idPersona")]
        public Persona persona { get; set; }
    }
}
