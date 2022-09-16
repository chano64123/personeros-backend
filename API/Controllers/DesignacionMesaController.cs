using API.DTO;
using AutoMapper;
using Core.Interface;
using Core.Model;
using Core.Specification;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class DesignacionMesaController : ControllerBase {
        private readonly IRepository<DesignacionMesa> repoDesignacionMesa;
        private readonly IRepository<Usuario> repoUsuario;
        private readonly IRepository<Institucion> repoInstitucion;
        private IMapper mapper;
        protected ResponseDTO response;

        public DesignacionMesaController(IRepository<DesignacionMesa> repoDesignacionMesa, IMapper mapper, IRepository<Usuario> repoUsuario, IRepository<Institucion> repoInstitucion) {
            this.mapper = mapper;
            this.repoDesignacionMesa = repoDesignacionMesa;
            this.repoUsuario = repoUsuario;
            this.repoInstitucion = repoInstitucion;
            response = new ResponseDTO();
        }

        [HttpGet]
        public async Task<ActionResult<List<DesignacionMesaDTO>>> obtenerDesignacionesMesa() {
            IReadOnlyCollection<DesignacionMesa> designacionesMesas;
            int code;
            try {
                var espec = new DesignacionMesaConTodo();
                designacionesMesas = await repoDesignacionMesa.obtenerTodosEspecificacionAsync(espec);
                response.success = true;
                response.displayMessage = designacionesMesas.Count == 0 ? "No se encontraron designaciones de mesas" : "Lista de designaciones de mesas (" + designacionesMesas.Count + ")";
                response.result = mapper.Map<IReadOnlyCollection<DesignacionMesa>, IReadOnlyCollection<DesignacionMesaDTO>>(designacionesMesas).OrderBy(x => x.mesa.institucion.distrito.nombre).ThenBy(x => x.mesa.institucion.nombre).ThenBy(x => x.mesa.numero).ThenBy(x => x.usuario.persona.nombres).ToList();
                code = designacionesMesas.Count == 0 ? 404 : 200;
            } catch(Exception ex) {
                response.success = false;
                response.displayMessage = "Error con el servidor";
                response.errorMessage = new List<string> { ex.ToString() };
                code = 500;
            }
            return StatusCode(code, response);
        }

        [HttpGet("usuario/{idUsuario}")]
        public async Task<ActionResult<List<DesignacionMesaDTO>>> obtenerDesignacionesMesaDeUsuario(int idUsuario) {
            IReadOnlyCollection<DesignacionMesa> designacionesMesas;
            int code;
            try {
                var especUsuario = new UsuarioConTodoTipoUsuarioTodoPersona(idUsuario);
                var usuario = await repoUsuario.obtenerPorIdEspecificoAsync(especUsuario);

                var espec = new DesignacionMesaConTodoDeUsuario(idUsuario);
                designacionesMesas = await repoDesignacionMesa.obtenerTodosEspecificacionAsync(espec);
                response.success = true;
                response.displayMessage = designacionesMesas.Count == 0 ? "No se encontraron mesas designadas al usuario " + usuario.persona.nombres + " " + usuario.persona.apellidoPaterno + " " + usuario.persona.apellidoMaterno + " (" + usuario.nombreUsuario + ")" : "Lista de mesas designadas al usuario " + usuario.persona.nombres + " " + usuario.persona.apellidoPaterno + " " + usuario.persona.apellidoMaterno + " (" + usuario.nombreUsuario + ")";
                response.result = mapper.Map<IReadOnlyCollection<DesignacionMesa>, IReadOnlyCollection<DesignacionMesaDTO>>(designacionesMesas).OrderBy(x => x.mesa.institucion.distrito.nombre).ThenBy(x => x.mesa.institucion.nombre).ThenBy(x => x.mesa.numero).ThenBy(x => x.usuario.persona.nombres).ToList();
                code = designacionesMesas.Count == 0 ? 404 : 200;
            } catch(Exception ex) {
                response.success = false;
                response.displayMessage = "Error con el servidor";
                response.errorMessage = new List<string> { ex.ToString() };
                code = 500;
            }
            return StatusCode(code, response);
        }

        [HttpGet("institucion/{idInstitucion}")]
        public async Task<ActionResult<List<DesignacionMesaDTO>>> obtenerDesignacionesMesaDeInstitucion(int idInstitucion) {
            IReadOnlyCollection<DesignacionMesa> designacionesMesas;
            int code;
            try {
                var institucion = await repoInstitucion.obtenerPorIdAsync(idInstitucion);

                var espec = new DesignacionMesaConTodoDeInstitucion(idInstitucion);
                designacionesMesas = await repoDesignacionMesa.obtenerTodosEspecificacionAsync(espec);
                response.success = true;
                response.displayMessage = designacionesMesas.Count == 0 ? "No se encontraron designaciones de la institución " + institucion.nombre : "Lista de designaciones de la institución " + institucion.nombre + " (" + designacionesMesas.Count + ")";
                response.result = mapper.Map<IReadOnlyCollection<DesignacionMesa>, IReadOnlyCollection<DesignacionMesaDTO>>(designacionesMesas).OrderBy(x => x.mesa.institucion.distrito.nombre).ThenBy(x => x.mesa.institucion.nombre).ThenBy(x => x.mesa.numero).ThenBy(x => x.usuario.persona.nombres).ToList();
                code = designacionesMesas.Count == 0 ? 404 : 200;
            } catch(Exception ex) {
                response.success = false;
                response.displayMessage = "Error con el servidor";
                response.errorMessage = new List<string> { ex.ToString() };
                code = 500;
            }
            return StatusCode(code, response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DesignacionMesaDTO>> obtenerDesignacionMesa(int id) {
            DesignacionMesa designacionMesa = new();
            int code;
            try {
                var espec = new DesignacionMesaConTodo(id);
                designacionMesa = await repoDesignacionMesa.obtenerPorIdEspecificoAsync(espec);
                response.success = true;
                response.displayMessage = designacionMesa == null ? "No se encontró la designacion buscada" : "Designación buscada (" + designacionMesa.mesa.numero+ " - " + designacionMesa.usuario.persona.nombres.Split(' ')[0] + " " + designacionMesa.usuario.persona.apellidoPaterno + ")";
                response.result = mapper.Map<DesignacionMesa, DesignacionMesaDTO>(designacionMesa);
                code = designacionMesa == null ? 404 : 200;
            } catch(Exception ex) {
                response.success = false;
                response.displayMessage = "Error con el servidor";
                response.errorMessage = new List<string> { ex.ToString() };
                code = 500;
            }
            return StatusCode(code, response);
        }

        [HttpPost]
        public async Task<ActionResult<List<DesignacionMesaDTO>>> crearAsignacion(List<DesignacionMesa> designacionesMesa) {
            int code;
            List<DesignacionMesa> designacionesResult = new List<DesignacionMesa>();
            try {
                foreach(var designacionMesa in designacionesMesa) {
                    //actualiza
                    var nuevaDesignacion = await repoDesignacionMesa.crearAsync(designacionMesa);
                    //arma la especificacion
                    var espec = new DesignacionMesaConTodo(nuevaDesignacion.idDesignacionMesa);
                    //obtiene la designacion actualizada
                    designacionesResult.Add(await repoDesignacionMesa.obtenerPorIdEspecificoAsync(espec));
                }

                response.result = mapper.Map<IReadOnlyCollection<DesignacionMesa>, IReadOnlyCollection<DesignacionMesaDTO>>(designacionesResult);
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
        public async Task<ActionResult<List<DesignacionMesaDTO>>> actualizarAsignacion(List<DesignacionMesa> designacionesMesa) {
            int code;
            List<DesignacionMesa> designacionesResult = new List<DesignacionMesa>();
            try {
                foreach(var designacionMesa in designacionesMesa) {
                    //actualiza
                    await repoDesignacionMesa.actualizarAsync(designacionMesa);
                    //arma la especificacion
                    var espec = new DesignacionMesaConTodo(designacionMesa.idDesignacionMesa);
                    //obtiene la designacion actualizada
                    designacionesResult.Add(await repoDesignacionMesa.obtenerPorIdEspecificoAsync(espec));

                }
                response.result = mapper.Map<List<DesignacionMesa>, List<DesignacionMesaDTO>>(designacionesResult);
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
        public async Task<ActionResult<bool>> eliminarDesignacionMesa(int id) {
            int code;
            try {
                bool designacionMesaEliminado = await repoDesignacionMesa.eliminarPorIdAsync(id);
                response.success = designacionMesaEliminado;
                response.displayMessage = designacionMesaEliminado ? "Personero eliminado correctamente" : "No se pudo eliminar el personero, primero elimine los datos relacionados al personero";
                code = designacionMesaEliminado ? 301 : 400;
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
