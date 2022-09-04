using Core.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.DTO {
    public class UsuarioDTO {
        public int idUsuario { get; set; }
        public string nombreUsuario { get; set; }
        public string cantidadMaximaMesas { get; set; }
        public string cantidadMaximaInstituciones { get; set; }
        public TipoUsuarioDTO tipoUsuario { get; set; }
        public PersonaDTO persona { get; set; }
    }
}
