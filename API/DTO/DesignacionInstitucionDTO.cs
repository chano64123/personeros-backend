using Core.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.DTO {
    public class DesignacionInstitucionDTO {
        public int idDesignacionInstitucion { get; set; }
        public UsuarioDTO usuario { get; set; }
        public InstitucionDTO institucion { get; set; }
    }
}
