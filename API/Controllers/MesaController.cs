using API.DTO;
using AutoMapper;
using Core.Interface;
using Core.Model;
using Core.Specification;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class MesaController : ControllerBase {
        private readonly IRepository<Mesa> repoMesa;
        private IMapper mapper;
        protected ResponseDTO response;

        public MesaController(IRepository<Mesa> repoMesa, IMapper mapper) {
            response = new ResponseDTO();
            this.repoMesa = repoMesa;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<MesaDTO>>> obtenerMesaes() {
            IReadOnlyCollection<Mesa> mesas;
            int code;
            try {
                var espec = new MesaConTodoInstitucion();
                mesas = await repoMesa.obtenerTodosEspecificacionAsync(espec);
                response.success = true;
                response.displayMessage = mesas.Count == 0 ? "No se encontraron mesas" : "Lista de Mesas (" + mesas.Count + ")";
                response.result = mapper.Map<IReadOnlyCollection<Mesa>, IReadOnlyCollection<MesaDTO>>(mesas).OrderBy(x => x.institucion.distrito.nombre).ThenBy(x => x.institucion.nombre).ThenBy(x => x.numero).ToList();
                code = mesas.Count == 0 ? 404 : 200;
            } catch(Exception ex) {
                response.success = false;
                response.displayMessage = "Error con el servidor";
                response.errorMessage = new List<string> { ex.ToString() };
                code = 500;
            }
            return StatusCode(code, response);
        }

        [HttpGet("Institucion/{idInstitucion}")]
        public async Task<ActionResult<List<MesaDTO>>> obtenerMesaes(int idInstitucion) {
            IReadOnlyCollection<Mesa> mesas;
            int code;
            try {
                var espec = new MesaDeInstitucionConTodoInstitucion(idInstitucion);
                mesas = await repoMesa.obtenerTodosEspecificacionAsync(espec);
                response.success = true;
                response.displayMessage = mesas.Count == 0 ? "No se encontraron mesas" : "Lista de Mesas (" + mesas.Count + ")";
                response.result = mapper.Map<IReadOnlyCollection<Mesa>, IReadOnlyCollection<MesaDTO>>(mesas).OrderBy(x => x.institucion.distrito.nombre).ThenBy(x => x.institucion.nombre).ThenBy(x => x.numero).ToList();
                code = mesas.Count == 0 ? 404 : 200;
            } catch(Exception ex) {
                response.success = false;
                response.displayMessage = "Error con el servidor";
                response.errorMessage = new List<string> { ex.ToString() };
                code = 500;
            }
            return StatusCode(code, response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MesaDTO>> obtenerMesa(int id) {
            Mesa mesa = new();
            int code;
            try {
                var espec = new MesaConTodoInstitucion(id);
                mesa = await repoMesa.obtenerPorIdEspecificoAsync(espec);
                response.success = true;
                response.displayMessage = mesa == null ? "No se encontró la mesa buscada" : "Mesa buscada (" + mesa.numero + " - " + mesa.institucion.nombre + ")";
                response.result = mapper.Map<Mesa, MesaDTO>(mesa);
                code = mesa == null ? 404 : 200;
            } catch(Exception ex) {
                response.success = false;
                response.displayMessage = "Error con el servidor";
                response.errorMessage = new List<string> { ex.ToString() };
                code = 500;
            }
            return StatusCode(code, response);
        }

        [HttpPost]
        public async Task<ActionResult<MesaDTO>> crearMesa(Mesa mesa) {
            int code;
            try {
                mesa = await repoMesa.crearAsync(mesa);
                var espec = new MesaConTodoInstitucion(mesa.idMesa);
                mesa = await repoMesa.obtenerPorIdEspecificoAsync(espec);
                response.result = mapper.Map<Mesa, MesaDTO>(mesa);
                response.success = true;
                response.displayMessage = "Mesa creada correctamente";
                code = 200;
            } catch(Exception ex) {
                response.success = false;
                response.displayMessage = "Error con el servidor";
                response.errorMessage = new List<string> { ex.ToString() };
                code = 500;
            }
            return StatusCode(code, response);
        }

        [HttpPut]
        public async Task<ActionResult<MesaDTO>> actualizarMesa(Mesa mesa) {
            int code;
            try {
                mesa = await repoMesa.actualizarAsync(mesa);
                var espec = new MesaConTodoInstitucion(mesa.idMesa);
                mesa = await repoMesa.obtenerPorIdEspecificoAsync(espec);
                response.result = mapper.Map<Mesa, MesaDTO>(mesa);
                response.success = true;
                response.displayMessage = "Mesa actualizada correctamente";
                code = 200;
            } catch(Exception ex) {
                response.success = false;
                response.displayMessage = "Error con el servidor";
                response.errorMessage = new List<string> { ex.ToString() };
                code = 304;
            }
            return StatusCode(code, response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> eliminarMesa(int id) {
            int code;
            try {
                bool mesaEliminado = await repoMesa.eliminarPorIdAsync(id);
                response.success = mesaEliminado;
                response.displayMessage = mesaEliminado ? "Mesa eliminada correctamente" : "No se pudo eliminar la Mesa, primero elimine los datos relacionados a la mesa";
                code = mesaEliminado ? 301 : 400;
            } catch(Exception ex) {
                response.success = false;
                response.displayMessage = "Error con el servidor";
                response.errorMessage = new List<string> { ex.ToString() };
                code = 500;
            }
            return StatusCode(code, response);
        }
    }
}
