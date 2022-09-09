using API.DTO;
using AutoMapper;
using Core.Interface;
using Core.Model;
using Core.Specification;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static Core.Specification.ActaConTodoMesaTodoTipoEleccion;

namespace API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase {
        private readonly IRepository<Usuario> repoUsuario;
        private IMapper mapper;
        protected ResponseDTO response;

        public UsuarioController(IRepository<Usuario> repoUsuario, IMapper mapper) {
            response = new ResponseDTO();
            this.repoUsuario = repoUsuario;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<UsuarioDTO>>> obtenerUsuarios() {
            IReadOnlyCollection<Usuario> usuarios;
            int code;
            try {
                var espec = new UsuarioConTodoTipoUsuarioTodoPersona();
                usuarios = await repoUsuario.obtenerTodosEspecificacionAsync(espec);
                response.success = true;
                response.displayMessage = usuarios.Count == 0 ? "No se encontraron usuarios" : "Lista de Usuarios (" + usuarios.Count + ")";
                response.result = mapper.Map<IReadOnlyCollection<Usuario>, IReadOnlyCollection<UsuarioDTO>>(usuarios).ToList();
                code = usuarios.Count == 0 ? 404 : 200;
            } catch(Exception ex) {
                response.success = false;
                response.displayMessage = "Error con el servidor";
                response.errorMessage = new List<string> { ex.ToString() };
                code = 500;
            }
            return StatusCode(code, response);
        }

        [HttpGet("TipoUsuario/{idTipoUsuario}")]
        public async Task<ActionResult<List<UsuarioDTO>>> obtenerUsuarioPorTipoUsuario(int idTipoUsuario) {
            IReadOnlyCollection<Usuario> usuarios;
            int code;
            try {
                var espec = new UsuariosPorTipoDeUsuarioConTodo(idTipoUsuario);
                usuarios = await repoUsuario.obtenerTodosEspecificacionAsync(espec);
                response.success = true;
                response.displayMessage = usuarios.Count == 0 ? "No se encontraron usuarios del tipo buscado" : "Lista de Usuarios (" + usuarios.Count + ")";
                response.result = mapper.Map<IReadOnlyCollection<Usuario>, IReadOnlyCollection<UsuarioDTO>>(usuarios).ToList();
                code = usuarios.Count == 0 ? 404 : 200;
            } catch(Exception ex) {
                response.success = false;
                response.displayMessage = "Error con el servidor";
                response.errorMessage = new List<string> { ex.ToString() };
                code = 500;
            }
            return StatusCode(code, response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDTO>> obtenerUsuario(int id) {
            Usuario usuario = new();
            int code;
            try {
                var espec = new UsuarioConTodoTipoUsuarioTodoPersona(id);
                usuario = await repoUsuario.obtenerPorIdEspecificoAsync(espec);
                response.success = true;
                response.displayMessage = usuario == null ? "No se encontro el usuario buscado" : "Usuario buscado (" + usuario.nombreUsuario + ")";
                response.result = mapper.Map<Usuario, UsuarioDTO>(usuario);
                code = usuario == null ? 404 : 200;
            } catch(Exception ex) {
                response.success = false;
                response.displayMessage = "Error con el servidor";
                response.errorMessage = new List<string> { ex.ToString() };
                code = 500;
            }
            return StatusCode(code, response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UsuarioDTO>> loginUsuario([FromBody]Login login) {
            Usuario usuario = new();
            int code;
            try {
                //obtener usuario de acuerdo al nombre de usuario
                var espec = new UsuarioPorNombreUsuarioLogin(login.nombreUsuario);
                usuario = await repoUsuario.obtenerPorIdEspecificoAsync(espec);

                //verificar si usario existe
                if(usuario == null) {
                    response.success = false;
                    response.displayMessage = "No exite el usuario ingresado";
                    response.result = null;
                    code = 400;
                    return StatusCode(code, response);
                }

                //compara clave
                if(!BCrypt.Net.BCrypt.Verify(login.clave, usuario.clave)) {
                    response.success = false;
                    response.displayMessage = "Las credenciales ingresadas no coinciden";
                    response.result = null;
                    code = 400;
                    return StatusCode(code, response);
                }

                //verifica nivel de acceso
                if(usuario.tipoUsuario.identificador < 4) {
                    response.success = false;
                    response.displayMessage = "No tienes los permisos para iniciar sesión";
                    response.result = null;
                    code = 400;
                    return StatusCode(code, response);
                }

                //continua normal
                response.success = true;
                response.displayMessage = "Bienvenido " + usuario.persona.nombres.Split(' ')[0] + " " + usuario.persona.apellidoPaterno;
                response.result = mapper.Map<Usuario, UsuarioDTO>(usuario);
                code = 200;
            } catch(Exception ex) {
                response.success = false;
                response.displayMessage = "Error con el servidor";
                response.errorMessage = new List<string> { ex.ToString() };
                code = 500;
            }
            return StatusCode(code, response);
        }

        [HttpPost]
        public async Task<ActionResult<UsuarioDTO>> crearUsuario(Usuario usuario) {
            int code;
            try {
                //verificar nombre de usuario
                bool existeUsuarioName = await existeNombreDeUsuario(usuario.nombreUsuario, usuario.idUsuario);
                if(existeUsuarioName) {
                    response.success = false;
                    response.displayMessage = "El nombre de usuario " + usuario.nombreUsuario + " ya se encuentra registrado";
                    response.result = null;
                    code = 409;
                    return StatusCode(code, response);
                }

                //verificar persona con mismo tipo xe usuario
                bool existePersonaRol = await existePersonaConTipoUsuario(usuario.idPersona, usuario.idTipoUsuario, usuario.idUsuario);
                if(existePersonaRol) {
                    response.success = false;
                    response.displayMessage = "Ya existe existe una cuenta de la persona con el tipo de usuario seleccionado";
                    response.result = null;
                    code = 409;
                    return StatusCode(code, response);
                }

                //formateanbdo datos del usuario
                usuario.clave = BCrypt.Net.BCrypt.HashPassword(usuario.clave);
                
                //continua normal
                usuario = await repoUsuario.crearAsync(usuario);
                var espec = new UsuarioConTodoTipoUsuarioTodoPersona(usuario.idUsuario);
                usuario = await repoUsuario.obtenerPorIdEspecificoAsync(espec);
                response.result = mapper.Map<Usuario, UsuarioDTO>(usuario);
                response.success = true;
                response.displayMessage = "Usuario creado correctamente";
                code = 200;
            } catch(Exception ex) {
                response.success = false;
                response.displayMessage = "Error con el servidor";
                response.errorMessage = new List<string> { ex.ToString() };
                code = 500;
            }
            return StatusCode(code, response);
        }

        private async Task<bool> existeNombreDeUsuario(string nombreUsuario, int idUsuario = 0) {
            var espec = idUsuario != 0 ? new UsuarioPorNombreUsuario(nombreUsuario, idUsuario) : new UsuarioPorNombreUsuario(nombreUsuario);
            var usuario = await repoUsuario.obtenerPorIdEspecificoAsync(espec);
            return !(usuario == null);
        }

        private async Task<bool> existePersonaConTipoUsuario(int idPersona, int idTipoUsuario, int idUsuario = 0) {
            var espec = idUsuario != 0 ? new UsuarioMismaPersonaTipoUsuario(idPersona, idTipoUsuario, idUsuario) : new UsuarioMismaPersonaTipoUsuario(idPersona, idTipoUsuario);
            var usuario = await repoUsuario.obtenerPorIdEspecificoAsync(espec);
            return !(usuario == null);
        }

        [HttpPut]
        public async Task<ActionResult<UsuarioDTO>> actualizarUsuario(Usuario usuario) {
            int code;
            try {
                ////verificar clave
                usuario.clave = await obtenerClaveUsuario(usuario.idUsuario, usuario.clave);

                //verificar nombre de usuario
                bool existeUsuarioName = await existeNombreDeUsuario(usuario.nombreUsuario, usuario.idUsuario);
                if(existeUsuarioName) {
                    response.success = false;
                    response.displayMessage = "El nombre de usuario " + usuario.nombreUsuario + " ya se encuentra registrado";
                    response.result = null;
                    code = 409;
                    return StatusCode(code, response);
                }

                //verificar persona con mismo tipo de usuario
                bool existePersonaRol = await existePersonaConTipoUsuario(usuario.idPersona, usuario.idTipoUsuario, usuario.idUsuario);
                if(existePersonaRol) {
                    response.success = false;
                    response.displayMessage = "Ya existe existe una cuenta de la persona con el tipo de usuario seleccionado";
                    response.result = null;
                    code = 409;
                    return StatusCode(code, response);
                }

                //continua normal
                usuario = await repoUsuario.actualizarAsync(usuario);
                var espec = new UsuarioConTodoTipoUsuarioTodoPersona(usuario.idUsuario);
                usuario = await repoUsuario.obtenerPorIdEspecificoAsync(espec);
                response.result = mapper.Map<Usuario, UsuarioDTO>(usuario);
                response.success = true;
                response.displayMessage = "Usuario actualizado correctamente";
                code = 200;
            } catch(Exception ex) {
                response.success = false;
                response.displayMessage = "Error con el servidor";
                response.errorMessage = new List<string> { ex.ToString() };
                code = 304;
            }
            return StatusCode(code, response);
        }

        private async Task<string> obtenerClaveUsuario(int idUsuario, string clave) {
            if(String.IsNullOrEmpty(clave)) {
                var espec = new UsuarioConTodoTipoUsuarioTodoPersona(idUsuario);
                var usuarioSearch = await repoUsuario.obtenerPorIdEspecificoAsync(espec);
                clave = usuarioSearch.clave;
            } else {
                clave = BCrypt.Net.BCrypt.HashPassword(clave);
            }
            return clave;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> eliminarUsuario(int id) {
            int code;
            try {
                bool usuarioEliminado = await repoUsuario.eliminarPorIdAsync(id);
                response.success = usuarioEliminado;
                response.displayMessage = usuarioEliminado ? "Usuario eliminado correctamente" : "No se pudo eliminar el Usuario, primero elimine los datos relacionados al usuario";
                code = usuarioEliminado ? 301 : 400;
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
