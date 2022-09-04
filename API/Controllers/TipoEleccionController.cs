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
    public class TipoEleccionController : ControllerBase {
        private readonly IRepository<TipoEleccion> repoTipoEleccion;
        private IMapper mapper;
        protected ResponseDTO response;

        public TipoEleccionController(IRepository<TipoEleccion> repoTipoEleccion, IMapper mapper) {
            response = new ResponseDTO();
            this.repoTipoEleccion = repoTipoEleccion;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<TipoEleccionDTO>>> obtenerTiposElecciones() {
            IReadOnlyCollection<TipoEleccion> tiposElecciones;
            int code;
            try {
                var espec = new TipoEleccionConPersona();
                tiposElecciones = await repoTipoEleccion.obtenerTodosEspecificacionAsync(espec);
                response.success = true;
                response.displayMessage = tiposElecciones.Count == 0 ? "No se encontraron tipos de elecciones" : "Lista de tipos de elecciones (" + tiposElecciones.Count + ")";
                response.result = mapper.Map<IReadOnlyCollection<TipoEleccion>, IReadOnlyCollection<TipoEleccionDTO>>(tiposElecciones);
                code = tiposElecciones.Count == 0 ? 404 : 200;
            } catch(Exception ex) {
                response.success = false;
                response.displayMessage = "Error con el servidor";
                response.errorMessage = new List<string> { ex.ToString() };
                code = 500;
            }
            return StatusCode(code, response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TipoEleccionDTO>> obtenerTipoEleccion(int id) {
            TipoEleccion tipoEleccion = new();
            int code;
            try {
                var espec = new TipoEleccionConPersona(id);
                tipoEleccion = await repoTipoEleccion.obtenerPorIdEspecificoAsync(espec);
                response.success = true;
                response.displayMessage = tipoEleccion == null ? "No se encontro el tipo de elección buscada" : "Tipo de Elección buscada (" + tipoEleccion.nombre + ")";
                response.result = mapper.Map<TipoEleccion, TipoEleccionDTO>(tipoEleccion);
                code = tipoEleccion == null ? 404 : 200;
            } catch(Exception ex) {
                response.success = false;
                response.displayMessage = "Error con el servidor";
                response.errorMessage = new List<string> { ex.ToString() };
                code = 500;
            }
            return StatusCode(code, response);
        }

        [HttpPost]
        public async Task<ActionResult<TipoEleccionDTO>> crearTipoEleccion(TipoEleccion tipoEleccion) {
            int code;
            try {
                tipoEleccion = await repoTipoEleccion.crearAsync(tipoEleccion);
                var espec = new TipoEleccionConPersona(tipoEleccion.idTipoEleccion);
                tipoEleccion = await repoTipoEleccion.obtenerPorIdEspecificoAsync(espec);
                response.success = true;
                response.displayMessage = "Tipo de Elección creada correctamente";
                response.result = mapper.Map<TipoEleccion, TipoEleccionDTO>(tipoEleccion);
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
        public async Task<ActionResult<TipoEleccionDTO>> actualizarTipoEleccion(TipoEleccion tipoEleccion) {
            int code;
            try {
                tipoEleccion = await repoTipoEleccion.actualizarAsync(tipoEleccion);
                var espec = new TipoEleccionConPersona(tipoEleccion.idTipoEleccion);
                tipoEleccion = await repoTipoEleccion.obtenerPorIdEspecificoAsync(espec);
                response.success = true;
                response.displayMessage = "Tipo de Elección actualizada correctamente";
                response.result = mapper.Map<TipoEleccion, TipoEleccionDTO>(tipoEleccion);
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
        public async Task<ActionResult<bool>> eliminarTipoEleccion(int id) {
            int code;
            try {
                bool tipoEleccionEliminado = await repoTipoEleccion.eliminarPorIdAsync(id);
                response.success = tipoEleccionEliminado;
                response.displayMessage = tipoEleccionEliminado ? "Tipo de Elección eliminada correctamente" : "No se pudo eliminar el Tipo de Elección, primero elimine los datos relacionados al tipo de elección";
                code = tipoEleccionEliminado ? 301 : 400;
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
