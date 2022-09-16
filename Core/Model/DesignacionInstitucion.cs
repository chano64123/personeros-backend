using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model {
    public class DesignacionInstitucion {
        [Key]
        public int idDesignacionInstitucion { get; set; }
        [Required]
        public int idUsuario { get; set; }
        [Required]
        public int idInstitucion { get; set; }
        [ForeignKey("idUsuario")]
        public Usuario usuario { get; set; }
        [ForeignKey("idInstitucion")]
        public Institucion institucion { get; set; }
    }
}
