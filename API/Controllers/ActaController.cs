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
    public class ActaController : ControllerBase {
        private readonly IRepository<Acta> repoActa;
        private IMapper mapper;
        protected ResponseDTO response;

        public ActaController(IRepository<Acta> repoActa, IMapper mapper) {
            response = new ResponseDTO();
            this.repoActa = repoActa;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ActaDTO>>> obtenerActaes() {
            IReadOnlyCollection<Acta> actas;
            int code;
            try {
                var espec = new ActaConTodoMesaTodoTipoEleccion();
                actas = await repoActa.obtenerTodosEspecificacionAsync(espec);
                response.success = true;
                response.displayMessage = actas.Count == 0 ? "No se encontraron actas" : "Lista de Actas (" + actas.Count + ")";
                response.result = mapper.Map<IReadOnlyCollection<Acta>, IReadOnlyCollection<ActaDTO>>(actas).OrderBy(x => x.mesa.institucion.distrito.nombre).ThenBy(x => x.mesa.institucion.nombre).ThenBy(x => x.mesa.numero).ThenBy(x => x.tipoEleccion.nombre).ToList();
                code = actas.Count == 0 ? 404 : 200;
            } catch(Exception ex) {
                response.success = false;
                response.displayMessage = "Error con el servidor";
                response.errorMessage = new List<string> { ex.ToString() };
                code = 500;
            }
            return StatusCode(code, response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ActaDTO>> obtenerActa(int id) {
            Acta acta = new();
            int code;
            try {
                var espec = new ActaConTodoMesaTodoTipoEleccion(id);
                acta = await repoActa.obtenerPorIdEspecificoAsync(espec);
                response.success = true;
                response.displayMessage = acta == null ? "No se encontro la acta buscada" : "Acta buscada (" + acta.mesa.institucion.nombre + " - " + acta.mesa.numero + " - " + acta.tipoEleccion.nombre + ")";
                response.result = mapper.Map<Acta, ActaDTO>(acta);
                code = acta == null ? 404 : 200;
            } catch(Exception ex) {
                response.success = false;
                response.displayMessage = "Error con el servidor";
                response.errorMessage = new List<string> { ex.ToString() };
                code = 500;
            }
            return StatusCode(code, response);
        }

        [HttpPost]
        public async Task<ActionResult<ActaDTO>> crearActa(Acta acta) {
            int code;
            try {
                acta = await repoActa.crearAsync(acta);
                var espec = new ActaConTodoMesaTodoTipoEleccion(acta.idActa);
                acta = await repoActa.obtenerPorIdEspecificoAsync(espec);
                response.result = mapper.Map<Acta, ActaDTO>(acta);
                response.success = true;
                response.displayMessage = "Acta creada correctamente";
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
        public async Task<ActionResult<ActaDTO>> actualizarActa(Acta acta) {
            int code;
            try {
                acta = await repoActa.actualizarAsync(acta);
                var espec = new ActaConTodoMesaTodoTipoEleccion(acta.idActa);
                acta = await repoActa.obtenerPorIdEspecificoAsync(espec);
                response.result = mapper.Map<Acta, ActaDTO>(acta);
                response.success = true;
                response.displayMessage = "Acta actualizada correctamente";
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
        public async Task<ActionResult<bool>> eliminarActa(int id) {
            int code;
            try {
                bool actaEliminado = await repoActa.eliminarPorIdAsync(id);
                response.success = actaEliminado;
                response.displayMessage = actaEliminado ? "Acta eliminada correctamente" : "No se pudo eliminar el Acta, primero elimine los datos relacionados al acta";
                code = actaEliminado ? 301 : 400;
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
