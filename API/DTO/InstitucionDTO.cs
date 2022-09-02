namespace API.DTO {
    public class InstitucionDTO {
        public int idInstitucion { get; set; }
        public string nombre { get; set; }
        public string direccion { get; set; }
        public int cantidadMesas { get; set; }
        public int cantidadElectores { get; set; }
        public DistritoDTO distrito { get; set; }
    }
}
