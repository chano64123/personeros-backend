using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specification {
    public class InstitucionConDistrito : SpecificationBase<Institucion> {
        public InstitucionConDistrito() {
            AgregarInclude(x => x.distrito);
        }

        public InstitucionConDistrito(int id) : base(x => x.idInstitucion == id) {
            AgregarInclude(x => x.distrito);
        }
    }
}
