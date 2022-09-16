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
    public class DesignacionInstitucionController : ControllerBase {
        private readonly IRepository<DesignacionInstitucion> repoDesignacionInstitucion;
        private readonly IRepository<Usuario> repoUsuario;
        private readonly IRepository<Institucion> repoInstitucion;
        private IMapper mapper;
        protected ResponseDTO response;

        public DesignacionInstitucionController(IRepository<DesignacionInstitucion> repoDesignacionInstitucion, IMapper mapper, IRepository<Usuario> repoUsuario, IRepository<Institucion> repoInstitucion) {
            this.mapper = mapper;
            this.repoDesignacionInstitucion = repoDesignacionInstitucion;
            this.repoUsuario = repoUsuario;
            this.repoInstitucion = repoInstitucion;
            response = new ResponseDTO();
        }

        [HttpGet]
        public async Task<ActionResult<List<DesignacionInstitucionDTO>>> obtenerDesignacionesInstituciones() {
            IReadOnlyCollection<DesignacionInstitucion> designacionesInstituciones;
            int code;
            try {
                var espec = new DesignacionInstitucionConTodo();
                designacionesInstituciones = await repoDesignacionInstitucion.obtenerTodosEspecificacionAsync(espec);
                response.success = true;
                response.displayMessage = designacionesInstituciones.Count == 0 ? "No se encontraron designaciones de instituciones" : "Lista de designaciones de instituciones (" + designacionesInstituciones.Count + ")";
                response.result = mapper.Map<IReadOnlyCollection<DesignacionInstitucion>, IReadOnlyCollection<DesignacionInstitucionDTO>>(designacionesInstituciones).OrderBy(x => x.institucion.distrito.nombre).ThenBy(x => x.institucion.nombre).ThenBy(x => x.usuario.persona.nombres).ToList();
                code = designacionesInstituciones.Count == 0 ? 404 : 200;
            } catch(Exception ex) {
                response.success = false;
                response.displayMessage = "Error con el servidor";
                response.errorMessage = new List<string> { ex.ToString() };
                code = 500;
            }
            return StatusCode(code, response);
        }

        [HttpGet("usuario/{idUsuario}")]
        public async Task<ActionResult<List<DesignacionInstitucionDTO>>> obtenerDesignacionesInstitucionDeUsuario(int idUsuario) {
            IReadOnlyCollection<DesignacionInstitucion> designacionesInstitucions;
            int code;
            try {
                var especUsuario = new UsuarioConTodoTipoUsuarioTodoPersona(idUsuario);
                var usuario = await repoUsuario.obtenerPorIdEspecificoAsync(especUsuario);

                var espec = new DesignacionInstitucionConTodoDeUsuario(idUsuario);
                designacionesInstitucions = await repoDesignacionInstitucion.obtenerTodosEspecificacionAsync(espec);
                response.success = true;
                response.displayMessage = designacionesInstitucions.Count == 0 ? "No se encontraron instituciones designadas al usuario " + usuario.persona.nombres + " " + usuario.persona.apellidoPaterno + " " + usuario.persona.apellidoMaterno + " (" + usuario.nombreUsuario + ")" : "Lista de instituciones designadas al usuario " + usuario.persona.nombres + " " + usuario.persona.apellidoPaterno + " " + usuario.persona.apellidoMaterno + " (" + usuario.nombreUsuario + ")";
                response.result = mapper.Map<IReadOnlyCollection<DesignacionInstitucion>, IReadOnlyCollection<DesignacionInstitucionDTO>>(designacionesInstitucions).OrderBy(x => x.institucion.distrito.nombre).ThenBy(x => x.institucion.nombre).ThenBy(x => x.usuario.persona.nombres).ToList();
                code = designacionesInstitucions.Count == 0 ? 404 : 200;
            } catch(Exception ex) {
                response.success = false;
                response.displayMessage = "Error con el servidor";
                response.errorMessage = new List<string> { ex.ToString() };
                code = 500;
            }
            return StatusCode(code, response);
        }

        [HttpGet("institucion/{idInstitucion}")]
        public async Task<ActionResult<List<DesignacionInstitucionDTO>>> obtenerDesignacionesInstitucionDeInstitucion(int idInstitucion) {
            IReadOnlyCollection<DesignacionInstitucion> designacionesInstitucions;
            int code;
            try {
                var institucion = await repoInstitucion.obtenerPorIdAsync(idInstitucion);

                var espec = new DesignacionInstitucionConTodoDeInstitucion(idInstitucion);
                designacionesInstitucions = await repoDesignacionInstitucion.obtenerTodosEspecificacionAsync(espec);
                response.success = true;
                response.displayMessage = designacionesInstitucions.Count == 0 ? "No se encontraron designaciones de la institución " + institucion.nombre : "Lista de designaciones de la institución " + institucion.nombre + " (" + designacionesInstitucions.Count + ")";
                response.result = mapper.Map<IReadOnlyCollection<DesignacionInstitucion>, IReadOnlyCollection<DesignacionInstitucionDTO>>(designacionesInstitucions).OrderBy(x => x.institucion.distrito.nombre).ThenBy(x => x.institucion.nombre).ThenBy(x => x.usuario.persona.nombres).ToList();
                code = designacionesInstitucions.Count == 0 ? 404 : 200;
            } catch(Exception ex) {
                response.success = false;
                response.displayMessage = "Error con el servidor";
                response.errorMessage = new List<string> { ex.ToString() };
                code = 500;
            }
            return StatusCode(code, response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DesignacionInstitucionDTO>> obtenerDesignacionInstitucion(int id) {
            DesignacionInstitucion designacionInstitucion = new();
            int code;
            try {
                var espec = new DesignacionInstitucionConTodo(id);
                designacionInstitucion = await repoDesignacionInstitucion.obtenerPorIdEspecificoAsync(espec);
                response.success = true;
                response.displayMessage = designacionInstitucion == null ? "No se encontró la designación buscada" : "Designación buscada (" + designacionInstitucion.institucion.nombre + " - " + designacionInstitucion.usuario.persona.nombres.Split(' ')[0] + " " + designacionInstitucion.usuario.persona.apellidoPaterno + ")";
                response.result = mapper.Map<DesignacionInstitucion, DesignacionInstitucionDTO>(designacionInstitucion);
                code = designacionInstitucion == null ? 404 : 200;
            } catch(Exception ex) {
                response.success = false;
                response.displayMessage = "Error con el servidor";
                response.errorMessage = new List<string> { ex.ToString() };
                code = 500;
            }
            return StatusCode(code, response);
        }

        [HttpPost]
        public async Task<ActionResult<List<DesignacionInstitucionDTO>>> crearAsignacion(List<DesignacionInstitucion> designacionesInstitucion) {
            int code;
            List<DesignacionInstitucion> designacionesResult = new List<DesignacionInstitucion>();
            try {
                foreach(var designacionInstitucion in designacionesInstitucion) {
                    //actualiza
                    var nuevaDesignacion = await repoDesignacionInstitucion.crearAsync(designacionInstitucion);
                    //arma la especificacion
                    var espec = new DesignacionInstitucionConTodo(nuevaDesignacion.idDesignacionInstitucion);
                    //obtiene la designación actualizada
                    designacionesResult.Add(await repoDesignacionInstitucion.obtenerPorIdEspecificoAsync(espec));
                }

                response.result = mapper.Map<IReadOnlyCollection<DesignacionInstitucion>, IReadOnlyCollection<DesignacionInstitucionDTO>>(designacionesResult);
                response.success = true;
                response.displayMessage = "Asignacion creada correctamente";
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
        public async Task<ActionResult<List<DesignacionInstitucionDTO>>> actualizarAsignacion(List<DesignacionInstitucion> designacionesInstitucion) {
            int code;
            List<DesignacionInstitucion> designacionesResult = new List<DesignacionInstitucion>();
            try {
                foreach(var designacionInstitucion in designacionesInstitucion) {
                    //actualiza
                    await repoDesignacionInstitucion.actualizarAsync(designacionInstitucion);
                    //arma la especificacion
                    var espec = new DesignacionInstitucionConTodo(designacionInstitucion.idDesignacionInstitucion);
                    //obtiene la designación actualizada
                    designacionesResult.Add(await repoDesignacionInstitucion.obtenerPorIdEspecificoAsync(espec));

                }
                response.result = mapper.Map<IReadOnlyCollection<DesignacionInstitucion>, IReadOnlyCollection<DesignacionInstitucionDTO>>(designacionesResult);
                response.success = true;
                response.displayMessage = "Designacion(es) actualizada(s) correctamente";
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
        public async Task<ActionResult<bool>> eliminarDesignacionInstitucion(int id) {
            int code;
            try {
                bool designacionInstitucionEliminado = await repoDesignacionInstitucion.eliminarPorIdAsync(id);
                response.success = designacionInstitucionEliminado;
                response.displayMessage = designacionInstitucionEliminado ? "Designacion eliminada correctamente" : "No se pudo eliminar la designación, primero elimine los datos relacionados a la designación";
                code = designacionInstitucionEliminado ? 301 : 400;
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
