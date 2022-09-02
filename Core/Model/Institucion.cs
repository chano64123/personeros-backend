using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model {
    public class Institucion {
        [Key]
        public int idInstitucion { get; set; }
        [Required]
        public int idDistrito { get; set; }
        [Required]
        public string nombre { get; set; }
        [Required]
        public string direccion { get; set; }
        [Required]
        public int cantidadMesas { get; set; }
        [Required]
        public int cantidadElectores { get; set; }
        [ForeignKey("idDistrito")]
        public Distrito distrito { get; set; }
    }
}
