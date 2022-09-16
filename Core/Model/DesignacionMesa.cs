using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model {
    public class DesignacionMesa {
        [Key]
        public int idDesignacionMesa { get; set; }
        [Required]
        public int idUsuario { get; set; }
        [Required]
        public int idMesa { get; set; }
        [ForeignKey("idUsuario")]
        public Usuario usuario { get; set; }
        [ForeignKey("idMesa")]
        public Mesa mesa { get; set; }
    }
}
