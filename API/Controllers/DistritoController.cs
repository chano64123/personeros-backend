using API.DTO;
using AutoMapper;
using Core.Interface;
using Core.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class DistritoController : ControllerBase {
        private readonly IRepository<Distrito> repoDistrito;
        private IMapper mapper;
        protected ResponseDTO response;

        public DistritoController(IRepository<Distrito> repoDistrito, IMapper mapper) {
            response = new ResponseDTO();
            this.repoDistrito = repoDistrito;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<DistritoDTO>>> obtenerDistritos() {
            IReadOnlyCollection<Distrito> distritos;
            int code;
            try {
                distritos = await repoDistrito.obtenerTodosAsync();
                response.success = true;
                response.displayMessage = distritos.Count == 0 ? "No se encontraron distritos" : "Lista de Distritos (" + distritos.Count + ")";
                response.result = mapper.Map<IReadOnlyCollection<Distrito>, IReadOnlyCollection<DistritoDTO>>(distritos);
                code = distritos.Count == 0 ? 404 : 200;
            } catch (Exception ex) {
                response.success = false;
                response.displayMessage = "Error con el servidor";
                response.errorMessage = new List<string> { ex.ToString() };
                code = 500;
            }
            return StatusCode(code, response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DistritoDTO>> obtenerDistrito(int id) {
            Distrito distrito = new();
            int code;
            try {
                distrito = await repoDistrito.obtenerPorIdAsync(id);
                response.success = true;
                response.displayMessage = distrito == null ? "No se encontro el distrito buscado" : "Distrito buscado (" + distrito.nombre + ")";
                response.result = mapper.Map<Distrito, DistritoDTO>(distrito);
                code = distrito == null ? 404 : 200;
            } catch (Exception ex) {
                response.success = false;
                response.displayMessage = "Error con el servidor";
                response.errorMessage = new List<string> { ex.ToString() };
                code = 500;
            }
            return StatusCode(code, response);
        }

        [HttpPost]
        public async Task<ActionResult<DistritoDTO>> crearDistrito(Distrito distrito) {
            int code;
            try {
                distrito = await repoDistrito.crearAsync(distrito);
                response.success = true;
                response.displayMessage = "Distrito creado correctamente";
                response.result = mapper.Map<Distrito, DistritoDTO>(distrito);
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
        public async Task<ActionResult<DistritoDTO>> actualizarDistrito(Distrito distrito) {
            int code;
            try {
                distrito = await repoDistrito.actualizarAsync(distrito);
                response.success = true;
                response.displayMessage = "Distrito actualizado correctamente";
                response.result = mapper.Map<Distrito, DistritoDTO>(distrito);
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
        public async Task<ActionResult<bool>> eliminarDistrito(int id) {
            int code;
            try {
                bool distritoEliminado = await repoDistrito.eliminarPorIdAsync(id);
                response.success = distritoEliminado;
                response.displayMessage = distritoEliminado ? "Distrito eliminado correctamente" : "No se pudo eliminar el Distrito";
                code = distritoEliminado ? 301 : 400;
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
