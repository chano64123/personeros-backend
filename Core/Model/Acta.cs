using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model {
    public class Acta {
        [Key]
        public int idActa { get; set; }
        [Required]
        public int idMesa { get; set; }
        [Required]
        public int idTipoEleccion { get; set; }
        [Required]
        public string foto { get; set; }
        [Required]
        public int cantidadVotosNulos { get; set; }
        [Required]
        public int cantidadVotosBlancos { get; set; }
        [Required]
        public int cantidadVotosImpugnados { get; set; }
        [Required]
        public int cantidadVotosFavor { get; set; }
        [Required]
        public int cantidadVotosContra { get; set; }
        [ForeignKey("idMesa")]
        public Mesa mesa { get; set; }
        [ForeignKey("idTipoEleccion")]
        public TipoEleccion tipoEleccion { get; set; }

    }
}
