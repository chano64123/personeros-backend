using Core.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.DTO {
    public class MesaDTO {
        public int idMesa { get; set; }
        public string aula { get; set; }
        public string numero { get; set; }
        public int cantidadElectores { get; set; }
        public InstitucionDTO institucion { get; set; }
    }
}
