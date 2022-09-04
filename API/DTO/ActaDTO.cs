using Core.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.DTO {
    public class ActaDTO {
        public int idActa { get; set; }
        public string foto { get; set; }
        public int cantidadVotosNulos { get; set; }
        public int cantidadVotosBlancos { get; set; }
        public int cantidadVotosImpugnados { get; set; }
        public int cantidadVotosFavor { get; set; }
        public int cantidadVotosContra { get; set; }
        public MesaDTO mesa { get; set; }
        public TipoEleccionDTO tipoEleccion { get; set; }
    }
}
