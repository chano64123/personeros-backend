using API.DTO;
using API.Helpers;
using AutoMapper;
using Core.Interface;
using Core.Model;
using Core.Specification;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Net;

namespace API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class PersonaController : ControllerBase {
        private readonly IRepository<Persona> repoPersona;
        private readonly IRepository<Distrito> repoDistrito;
        private readonly IRepository<Institucion> repoInstitucion;
        private IMapper mapper;
        protected ResponseDTO response;

        public PersonaController(IRepository<Distrito> repoDistrito, IMapper mapper, IRepository<Institucion> repoInstitucion, IRepository<Persona> repoPersona) {
            response = new ResponseDTO();
            this.repoDistrito = repoDistrito;
            this.mapper = mapper;
            this.repoInstitucion = repoInstitucion;
            this.repoPersona = repoPersona;
        }

        [HttpGet]
        public async Task<ActionResult<List<PersonaDTO>>> obtenerPersonas() {
            IReadOnlyCollection<Persona> personas;
            int code;
            try {
                var espec = new PersonaConTodoDistritoTodoInstitucion();
                personas = await repoPersona.obtenerTodosEspecificacionAsync(espec);
                response.success = true;
                response.displayMessage = personas.Count == 0 ? "No se encontraron personas" : "Lista de Personas (" + personas.Count + ")";
                response.result = mapper.Map<IReadOnlyCollection<Persona>, IReadOnlyCollection<PersonaDTO>>(personas).OrderBy(x => x.apellidoPaterno).ThenBy(x => x.apellidoMaterno).ThenBy(x => x.nombres).ToList();
                code = personas.Count == 0 ? 404 : 200;
            } catch(Exception ex) {
                response.success = false;
                response.displayMessage = "Error con el servidor";
                response.errorMessage = new List<string> { ex.ToString() };
                code = 500;
            }
            return StatusCode(code, response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PersonaDTO>> obtenerPersona(int id) {
            Persona persona = new();
            int code;
            try {
                var espec = new PersonaConTodoDistritoTodoInstitucion(id);
                persona = await repoPersona.obtenerPorIdEspecificoAsync(espec);
                response.success = true;
                response.displayMessage = persona == null ? "No se encontró la persona buscada" : "Persona buscada (" + persona.nombres.Split()[0] + " " + persona.apellidoPaterno + ")";
                response.result = mapper.Map<Persona, PersonaDTO>(persona);
                code = persona == null ? 404 : 200;
            } catch(Exception ex) {
                response.success = false;
                response.displayMessage = "Error con el servidor";
                response.errorMessage = new List<string> { ex.ToString() };
                code = 500;
            }
            return StatusCode(code, response);
        }

        [HttpPost]
        public async Task<ActionResult<PersonaDTO>> crearPersona(Persona persona) {
            int code;
            try {
                //verificar dni
                bool existeDni = await existeDniPersona(persona.dni);
                if(existeDni) {
                    response.success = false;
                    response.displayMessage = "El DNI " + persona.dni + " ya se encuentra registrado";
                    response.result = null;
                    code = 409;
                    return StatusCode(code, response);
                }

                //verificar celular
                bool existeCelular = await existeCelularPersona(persona.celular);
                if(existeCelular) {
                    response.success = false;
                    response.displayMessage = "El celular/teléfono " + persona.celular + " ya se encuentra registrado";
                    response.result = null;
                    code = 409;
                    return StatusCode(code, response);
                }

                //formateando datos de la persona
                persona.nombres = UtilityHelper.ToUpperFirstLetterOfWord(persona.nombres);
                persona.apellidoMaterno = UtilityHelper.ToUpperFirstLetterOfWord(persona.apellidoMaterno.Trim());
                persona.apellidoPaterno= UtilityHelper.ToUpperFirstLetterOfWord(persona.apellidoPaterno.Trim());

                //continua normal
                persona = await repoPersona.crearAsync(persona);
                var espec = new PersonaConTodoDistritoTodoInstitucion(persona.idPersona);
                persona = await repoPersona.obtenerPorIdEspecificoAsync(espec);
                response.success = true;
                response.displayMessage = "Persona creada correctamente";
                response.result = mapper.Map<Persona, PersonaDTO>(persona);
                code = 200;
            } catch(Exception ex) {
                response.success = false;
                response.displayMessage = "Error con el servidor";
                response.errorMessage = new List<string> { ex.ToString() };
                code = 500;
            }
            return StatusCode(code, response);
        }

        private async Task<bool> existeCelularPersona(string celular, int idPersona = 0) {
            var espec = idPersona != 0 ? new PersonaPorCelular(celular, idPersona) : new PersonaPorCelular(celular);
            var persona = await repoPersona.obtenerPorIdEspecificoAsync(espec);
            return !(persona == null);
        }

        private async Task<bool> existeDniPersona(string dni, int idPersona = 0) {
            var espec = idPersona != 0 ? new PersonaPorDni(dni, idPersona) : new PersonaPorDni(dni);
            var persona = await repoPersona.obtenerPorIdEspecificoAsync(espec);
            return !(persona == null);

        }

        [HttpPut]
        public async Task<ActionResult<PersonaDTO>> actualizarPersona(Persona persona) {
            int code;
            try {
                //verificar dni
                bool existeDni = await existeDniPersona(persona.dni, persona.idPersona);
                if(existeDni) {
                    response.success = false;
                    response.displayMessage = "El DNI ya se encuentra registrado";
                    response.result = null;
                    code = 409;
                    return StatusCode(code, response);
                }

                //verificar celular
                bool existeCelular = await existeCelularPersona(persona.celular, persona.idPersona);
                if(existeCelular) {
                    response.success = false;
                    response.displayMessage = "El celular/teléfono ya se encuentra registrado";
                    response.result = null;
                    code = 409;
                    return StatusCode(code, response);
                }

                //continua normal
                persona = await repoPersona.actualizarAsync(persona);
                var espec = new PersonaConTodoDistritoTodoInstitucion(persona.idPersona);
                persona = await repoPersona.obtenerPorIdEspecificoAsync(espec);
                response.success = true;
                response.displayMessage = "Persona actualizada correctamente";
                response.result = mapper.Map<Persona, PersonaDTO>(persona);
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
        public async Task<ActionResult<bool>> eliminarPersona(int id) {
            int code;
            try {
                bool personaEliminada = await repoPersona.eliminarPorIdAsync(id);
                response.success = personaEliminada;
                response.displayMessage = personaEliminada ? "Persona eliminada correctamente" : "No se pudo eliminar la Persona, primero elimine los datos relacionados a la persona";
                code = personaEliminada ? 301 : 400;
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
