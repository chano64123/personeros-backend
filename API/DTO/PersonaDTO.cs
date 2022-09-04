using Core.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.DTO {
    public class PersonaDTO {
        public int idPersona { get; set; }
        public string dni { get; set; }
        public string nombres { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public string celular { get; set; }
        public string direccion { get; set; }
        public DistritoDTO distritoResidencia { get; set; }
        public InstitucionDTO institucionVotacion { get; set; }
    }
}
