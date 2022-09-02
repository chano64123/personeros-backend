using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model {
    public class TipoUsuario {
        [Key]
        public int idTipoUsuario { get; set; }
        [Required]
        public string nombre { get; set; }
        [Required]
        public int identificador { get; set; }
        [Required]
        public int nivelAcceso { get; set; }
    }
}
