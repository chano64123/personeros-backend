using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model {
    public class Mesa {
        [Key]
        public int idMesa { get; set; }
        [Required]
        public int idInstitucion { get; set; }
        [Required]
        public string aula { get; set; }
        [Required]
        public string numero { get; set; }
        public int cantidadElectores { get; set; }
        [ForeignKey("idInstitucion")]
        public Institucion institucion { get; set; }
    }
}
