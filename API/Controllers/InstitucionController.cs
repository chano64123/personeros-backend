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
    public class InstitucionController : ControllerBase {
        private readonly IRepository<Institucion> repoInstitucion;
        private IMapper mapper;
        protected ResponseDTO response;

        public InstitucionController(IRepository<Institucion> repoInstitucion, IMapper mapper) {
            response = new ResponseDTO();
            this.repoInstitucion = repoInstitucion;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<InstitucionDTO>>> obtenerInstituciones() {
            IReadOnlyCollection<Institucion> instituciones;
            int code;
            try {
                var espec = new InstitucionConTodoDistrito();
                instituciones = await repoInstitucion.obtenerTodosEspecificacionAsync(espec);
                response.success = true;
                response.displayMessage = instituciones.Count == 0 ? "No se encontraron instituciones" : "Lista de Instituciones (" + instituciones.Count + ")";
                response.result = mapper.Map<IReadOnlyCollection<Institucion>, IReadOnlyCollection<InstitucionDTO>>(instituciones).OrderBy(x => x.distrito.nombre).ToList();
                code = instituciones.Count == 0 ? 404 : 200;
            } catch (Exception ex) {
                response.success = false;
                response.displayMessage = "Error con el servidor";
                response.errorMessage = new List<string> { ex.ToString() };
                code = 500;
            }
            return StatusCode(code, response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InstitucionDTO>> obtenerInstitucion(int id) {
            Institucion institucion = new();
            int code;
            try {
                var espec = new InstitucionConTodoDistrito(id);
                institucion = await repoInstitucion.obtenerPorIdEspecificoAsync(espec);
                response.success = true;
                response.displayMessage = institucion == null ? "No se encontro la institucion buscado" : "Institución buscada (" + institucion.nombre + ")";
                response.result = mapper.Map<Institucion, InstitucionDTO>(institucion);
                code = institucion == null ? 404 : 200;
            } catch (Exception ex) {
                response.success = false;
                response.displayMessage = "Error con el servidor";
                response.errorMessage = new List<string> { ex.ToString() };
                code = 500;
            }
            return StatusCode(code, response);
        }

        [HttpPost]
        public async Task<ActionResult<InstitucionDTO>> crearInstitucion(Institucion institucion) {
            int code;
            try {
                institucion = await repoInstitucion.crearAsync(institucion);
                var espec = new InstitucionConTodoDistrito(institucion.idInstitucion);
                institucion = await repoInstitucion.obtenerPorIdEspecificoAsync(espec);
                response.result = mapper.Map<Institucion, InstitucionDTO>(institucion);
                response.success = true;
                response.displayMessage = "Institución creada correctamente";
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
        public async Task<ActionResult<InstitucionDTO>> actualizarInstitucion(Institucion institucion) {
            int code;
            try {
                institucion = await repoInstitucion.actualizarAsync(institucion);
                var espec = new InstitucionConTodoDistrito(institucion.idInstitucion);
                institucion = await repoInstitucion.obtenerPorIdEspecificoAsync(espec);
                response.result = mapper.Map<Institucion, InstitucionDTO>(institucion);
                response.success = true;
                response.displayMessage = "Institución actualizada correctamente";
                code = 200;
            } catch (Exception ex) {
                response.success = false;
                response.displayMessage = "Error con el servidor";
                response.errorMessage = new List<string> { ex.ToString() };
                code = 304;
            }
            return StatusCode(code, response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> eliminarInstitucion(int id) {
            int code;
            try {
                bool institucionEliminado = await repoInstitucion.eliminarPorIdAsync(id);
                response.success = institucionEliminado;
                response.displayMessage = institucionEliminado ? "Institución eliminada correctamente" : "No se pudo eliminar la Institución, primero elimine los datos relacionados a la institución";
                code = institucionEliminado ? 301 : 400;
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
