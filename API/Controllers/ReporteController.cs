using API.DTO;
using AutoMapper;
using Core.Interface;
using Core.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ReporteController : ControllerBase {
        private readonly IRepository<Acta> repoActa;
        private readonly IRepository<Distrito> repoDistrito;
        private readonly IRepository<Institucion> repoIsntitucion;
        private readonly IRepository<Mesa> repoMesa;
        private readonly IRepository<Persona> repoPersona;
        private readonly IRepository<TipoEleccion> repoTipoEleccion;
        private readonly IRepository<TipoUsuario> repoTipoUsuario;
        private readonly IRepository<Usuario> repoUsuario;
        private IMapper mapper;
        protected ResponseDTO response;

        public ReporteController(IRepository<Acta> repoActa, IRepository<Distrito> repoDistrito, IRepository<Institucion> repoIsntitucion, IRepository<Mesa> repoMesa, IRepository<Persona> repoPersona, IRepository<TipoEleccion> repoTipoEleccion, IRepository<TipoUsuario> repoTipoUsuario, IRepository<Usuario> repoUsuario, IMapper mapper) {
            this.repoActa = repoActa;
            this.repoDistrito = repoDistrito;
            this.repoIsntitucion = repoIsntitucion;
            this.repoMesa = repoMesa;
            this.repoPersona = repoPersona;
            this.repoTipoEleccion = repoTipoEleccion;
            this.repoTipoUsuario = repoTipoUsuario;
            this.repoUsuario = repoUsuario;
            response = new ResponseDTO();
            this.mapper = mapper;
        }

        [HttpGet("totales/{identificadoUsuario}")]
        public async Task<ActionResult<Reporte>> obtenerTotales(int identificadoUsuario) {
            Reporte reporte = new();
            int code;
            try {
                //obteniendo objetos
                var actas = await repoActa.obtenerTodosAsync();
                var distritos = await repoDistrito.obtenerTodosAsync();
                var instituciones = await repoIsntitucion.obtenerTodosAsync();
                var mesas = await repoMesa.obtenerTodosAsync();
                var personas = await repoPersona.obtenerTodosAsync();
                var tiposElecciones = await repoTipoEleccion.obtenerTodosAsync();
                var tiposUsuarios = await repoTipoUsuario.obtenerTodosAsync();
                var usuarios = await repoUsuario.obtenerTodosAsync();

                //asignando valores
                //solo los modulos que tiene permiso el admin
                if(identificadoUsuario == 5) {
                    reporte.cantidadDistritos = distritos.Count();
                    reporte.cantidadTiposUsuario = tiposUsuarios.Count();
                }
                //el resto de modulos
                reporte.cantidadActas = actas.Count();
                reporte.cantidadInstituciones = instituciones.Count();
                reporte.cantidadMesas = mesas.Count();
                reporte.cantidadPersona = personas.Count();
                reporte.cantidadTiposEleccion = tiposElecciones.Count();
                reporte.cantidadUsuarios = usuarios.Where(x => x.tipoUsuario.identificador <= identificadoUsuario).Count();

                response.success = true;
                response.displayMessage = "Cantidad de Registros";
                response.result = reporte;
                code = 200;
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
