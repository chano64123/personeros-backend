using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model {
    public class Persona {
        [Key]
        public int idPersona { get; set; }
        [Required]
        public int idDistritoDireccion { get; set; }
        [Required]
        public int idInstitucionVotacion { get; set; }
        [Required]
        public string dni { get; set; }
        [Required]
        public string nombres { get; set; }
        [Required]
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        [Required]
        public string celular { get; set; }
        [Required]
        public string direccion { get; set; }
        [ForeignKey("idDistritoDireccion")]
        public Distrito distritoResidencia { get; set; }
        [ForeignKey("idInstitucionVotacion")]
        public Institucion institucionVotacion { get; set; }
    }
}
