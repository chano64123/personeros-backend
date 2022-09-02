using API.DTO;
using AutoMapper;
using Core.Interface;
using Core.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class TipoUsuarioController : ControllerBase {
        private readonly IRepository<TipoUsuario> repoTipoUsuario;
        private IMapper mapper;
        protected ResponseDTO response;

        public TipoUsuarioController(IRepository<TipoUsuario> repoTipoUsuario, IMapper mapper) {
            response = new ResponseDTO();
            this.repoTipoUsuario = repoTipoUsuario;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<TipoEleccionDTO>>> obtenerTiposUsuario() {
            IReadOnlyCollection<TipoUsuario> tiposUsuario;
            int code;
            try {
                tiposUsuario = await repoTipoUsuario.obtenerTodosAsync();
                response.success = true;
                response.displayMessage = tiposUsuario.Count == 0 ? "No se encontraron tipos de usuario" : "Lista de tipos de usuario (" + tiposUsuario.Count + ")";
                response.result = mapper.Map<IReadOnlyCollection<TipoUsuario>, IReadOnlyCollection<TipoEleccionDTO>>(tiposUsuario);
                code = tiposUsuario.Count == 0 ? 404 : 200;
            } catch (Exception ex) {
                response.success = false;
                response.displayMessage = "Error con el servidor";
                response.errorMessage = new List<string> { ex.ToString() };
                code = 500;
            }
            return StatusCode(code, response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TipoEleccionDTO>> obtenerTipoUsuario(int id) {
            TipoUsuario tipoUsuario = new();
            int code;
            try {
                tipoUsuario = await repoTipoUsuario.obtenerPorIdAsync(id);
                response.success = true;
                response.displayMessage = tipoUsuario == null ? "No se encontro el tipo de usuario buscado" : "Tipo de Usuario buscado (" + tipoUsuario.nombre + ")";
                response.result = mapper.Map<TipoUsuario, TipoEleccionDTO>(tipoUsuario);
                code = tipoUsuario == null ? 404 : 200;
            } catch (Exception ex) {
                response.success = false;
                response.displayMessage = "Error con el servidor";
                response.errorMessage = new List<string> { ex.ToString() };
                code = 500;
            }
            return StatusCode(code, response);
        }

        [HttpPost]
        public async Task<ActionResult<TipoEleccionDTO>> crearTipoUsuario(TipoUsuario tipoUsuario) {
            int code;
            try {
                tipoUsuario = await repoTipoUsuario.crearAsync(tipoUsuario);
                response.success = true;
                response.displayMessage = "Tipo de Usuario creado correctamente";
                response.result = mapper.Map<TipoUsuario, TipoEleccionDTO>(tipoUsuario);
                code = 200;
            } catch (Exception ex) {
                response.success = false;
                response.displayMessage = "Error con el servidor";
                response.errorMessage = new List<string> { ex.ToString() };
                code = 500;
            }
            return StatusCode(code, response);
        }

        [HttpPut]
        public async Task<ActionResult<TipoEleccionDTO>> actualizarTipoUsuario(TipoUsuario tipoUsuario) {
            int code;
            try {
                tipoUsuario = await repoTipoUsuario.actualizarAsync(tipoUsuario);
                response.success = true;
                response.displayMessage = "Tipo de Usuario actualizado correctamente";
                response.result = mapper.Map<TipoUsuario, TipoEleccionDTO>(tipoUsuario);
                code = 200;
            } catch (Exception ex) {
                response.success = false;
                response.displayMessage = "Error con el servidor";
                response.errorMessage = new List<string> { ex.ToString() };
                code = 304;
            }
            return StatusCode(code, response);
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> eliminarTipoUsuario(int id) {
            int code;
            try {
                bool tipoUsuarioEliminado = await repoTipoUsuario.eliminarPorIdAsync(id);
                response.success = tipoUsuarioEliminado;
                response.displayMessage = tipoUsuarioEliminado ? "Tipo de Usuario eliminado correctamente" : "No se pudo eliminar el Tipo de Usuario";
                code = tipoUsuarioEliminado ? 301 : 400;
            } catch (Exception ex) {
                response.success = false;
                response.displayMessage = "Error con el servidor";
                response.errorMessage = new List<string> { ex.ToString() };
                code = 500;
            }
            return StatusCode(code, response);
        }
    }
}
