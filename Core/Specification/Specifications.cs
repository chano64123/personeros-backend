using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specification {

    public class TipoEleccionConPersona : SpecificationBase<TipoEleccion> {
        public TipoEleccionConPersona() {
            AgregarInclude(x => x.personaCandidato.distritoResidencia);
            AgregarInclude(x => x.personaCandidato.institucionVotacion.distrito);
        }

        public TipoEleccionConPersona(int id) : base(x => x.idTipoEleccion == id) {
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

    public class PersonaConDistritoInstitucion : SpecificationBase<Persona> {
        public PersonaConDistritoInstitucion() {
            AgregarInclude(x => x.distritoResidencia);
            AgregarInclude(x => x.institucionVotacion.distrito);
        }

        public PersonaConDistritoInstitucion(int id) : base(x => x.idPersona == id) {
            AgregarInclude(x => x.distritoResidencia);
            AgregarInclude(x => x.institucionVotacion.distrito);
        }
    }

    public class InstitucionesDeDistrito : SpecificationBase<Institucion> {
        public InstitucionesDeDistrito(int id) : base(x => x.idDistrito == id) {
            AgregarInclude(x => x.distrito);
        }
    }

    public class InstitucionConDistrito : SpecificationBase<Institucion> {
        public InstitucionConDistrito() {
            AgregarInclude(x => x.distrito);
        }

        public InstitucionConDistrito(int id) : base(x => x.idInstitucion == id) {
            AgregarInclude(x => x.distrito);
        }
    }
}
