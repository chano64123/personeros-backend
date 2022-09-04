using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model {
    public class TipoEleccion {
        [Key]
        public int idTipoEleccion { get; set; }
        [Required]
        public int idPersonaCandidato { get; set; }
        [Required]
        public string nombre { get; set; }
        [ForeignKey("idPersonaCandidato")]
        public Persona personaCandidato { get; set; }
    }
}
