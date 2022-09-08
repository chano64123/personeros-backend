using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specification {
    public class UsuariosPorTipoDeUsuarioConTodo : SpecificationBase<Usuario> {
        public UsuariosPorTipoDeUsuarioConTodo(int idTipoUsuario) : base(x => x.tipoUsuario.idTipoUsuario == idTipoUsuario) {
            AgregarInclude(x => x.tipoUsuario);
            AgregarInclude(x => x.persona.distritoResidencia);
            AgregarInclude(x => x.persona.institucionVotacion.distrito);
        }
    }

    public class UsuarioPorNombreUsuarioLogin : SpecificationBase<Usuario> {
        public UsuarioPorNombreUsuarioLogin(string nombreUsuario) : base(x => x.nombreUsuario.Equals(nombreUsuario)) {
            AgregarInclude(x => x.tipoUsuario);
            AgregarInclude(x => x.persona.distritoResidencia);
            AgregarInclude(x => x.persona.institucionVotacion.distrito);
        }
    }

    public class UsuarioPorNombreUsuario : SpecificationBase<Usuario> {
        public UsuarioPorNombreUsuario(string nombreUsuario, int idUsuario) : base(x => x.nombreUsuario.Equals(nombreUsuario) && x.idUsuario != idUsuario) {
        }

        public UsuarioPorNombreUsuario(string nombreUsuario) : base(x => x.nombreUsuario.Equals(nombreUsuario)) {
        }
    }

    public class UsuarioMismaPersonaTipoUsuario : SpecificationBase<Usuario> {
        public UsuarioMismaPersonaTipoUsuario(int idPersona, int idTipoUsuario) : base(x => x.idTipoUsuario == idTipoUsuario && x.idPersona == idPersona) {
        }

        public UsuarioMismaPersonaTipoUsuario(int idPersona, int idTipoUsuario, int idUsuario) : base(x => x.idTipoUsuario == idTipoUsuario && x.idPersona == idPersona && x.idUsuario != idUsuario) {
        }
    }

    public class UsuarioConTodoTipoUsuarioTodoPersona : SpecificationBase<Usuario> {
        public UsuarioConTodoTipoUsuarioTodoPersona() {
            AgregarInclude(x => x.tipoUsuario);
            AgregarInclude(x => x.persona.distritoResidencia);
            AgregarInclude(x => x.persona.institucionVotacion.distrito);
        }

        public UsuarioConTodoTipoUsuarioTodoPersona(int id) : base(x => x.idUsuario == id) {
            AgregarInclude(x => x.tipoUsuario);
            AgregarInclude(x => x.persona.distritoResidencia);
            AgregarInclude(x => x.persona.institucionVotacion.distrito);
        }
    }

    public class ActaConTodoMesaTodoTipoEleccion : SpecificationBase<Acta> {
        public ActaConTodoMesaTodoTipoEleccion() {
            AgregarInclude(x => x.tipoEleccion.personaCandidato.distritoResidencia);
            AgregarInclude(x => x.tipoEleccion.personaCandidato.institucionVotacion.distrito);
            AgregarInclude(x => x.mesa.institucion.distrito);
        }

        public ActaConTodoMesaTodoTipoEleccion(int id) : base(x => x.idActa == id) {
            AgregarInclude(x => x.tipoEleccion.personaCandidato.distritoResidencia);
            AgregarInclude(x => x.tipoEleccion.personaCandidato.institucionVotacion.distrito);
            AgregarInclude(x => x.mesa.institucion.distrito);
        }
    }

    public class MesaConTodoInstitucion : SpecificationBase<Mesa> {
        public MesaConTodoInstitucion() {
            AgregarInclude(x => x.institucion.distrito);
        }

        public MesaConTodoInstitucion(int id) : base(x => x.idMesa == id) {
            AgregarInclude(x => x.institucion.distrito);
        }
    }

    public class TipoEleccionConTodoPersona : SpecificationBase<TipoEleccion> {
        public TipoEleccionConTodoPersona() {
            AgregarInclude(x => x.personaCandidato.distritoResidencia);
            AgregarInclude(x => x.personaCandidato.institucionVotacion.distrito);
        }

        public TipoEleccionConTodoPersona(int id) : base(x => x.idTipoEleccion == id) {
            AgregarInclude(x => x.personaCandidato.distritoResidencia);
            AgregarInclude(x => x.personaCandidato.institucionVotacion.distrito);
        }
    }

    public class PersonaPorDni : SpecificationBase<Persona> {
        public PersonaPorDni(string dni, int idPersona) : base(x => x.dni.Equals(dni) && x.idPersona != idPersona) {
        }

        public PersonaPorDni(string dni) : base(x => x.dni.Equals(dni)) {
        }
    }

    public class PersonaPorCelular : SpecificationBase<Persona> {
        public PersonaPorCelular(string celular, int idPersona) : base(x => (x.celular.Contains(celular) || celular.Contains(x.celular)) && x.idPersona != idPersona) {
        }
        public PersonaPorCelular(string celular) : base(x => x.celular.Contains(celular) || celular.Contains(x.celular)) {
        }
    }

    public class PersonaConTodoDistritoTodoInstitucion : SpecificationBase<Persona> {
        public PersonaConTodoDistritoTodoInstitucion() {
            AgregarInclude(x => x.distritoResidencia);
            AgregarInclude(x => x.institucionVotacion.distrito);
        }

        public PersonaConTodoDistritoTodoInstitucion(int id) : base(x => x.idPersona == id) {
            AgregarInclude(x => x.distritoResidencia);
            AgregarInclude(x => x.institucionVotacion.distrito);
        }
    }

    public class InstitucionesDeDistrito : SpecificationBase<Institucion> {
        public InstitucionesDeDistrito(int id) : base(x => x.idDistrito == id) {
            AgregarInclude(x => x.distrito);
        }
    }

    public class InstitucionConTodoDistrito : SpecificationBase<Institucion> {
        public InstitucionConTodoDistrito() {
            AgregarInclude(x => x.distrito);
        }

        public InstitucionConTodoDistrito(int id) : base(x => x.idInstitucion == id) {
            AgregarInclude(x => x.distrito);
        }
    }
}
