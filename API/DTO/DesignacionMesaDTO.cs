using Core.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.DTO {
    public class DesignacionMesaDTO {
        public int idDesignacionMesa { get; set; }
        public UsuarioDTO usuario { get; set; }
        public MesaDTO mesa { get; set; }
    }
}
