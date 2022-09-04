using Core.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.DTO {
    public class TipoEleccionDTO {
        public int idTipoEleccion { get; set; }
        public string nombre { get; set; }
        public PersonaDTO personaCandidato { get; set; }
    }
}

