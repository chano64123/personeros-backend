using Core.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.DTO {
    public class UsuarioDTO {
        public int idUsuario { get; set; }
        public string nombreUsuario { get; set; }
        public int cantidadMaximaMesas { get; set; }
        public int cantidadMaximaInstituciones { get; set; }
        public TipoUsuarioDTO tipoUsuario { get; set; }
        public PersonaDTO persona { get; set; }
    }
}
