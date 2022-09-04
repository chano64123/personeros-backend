using API.DTO;
using AutoMapper;
using Core.Model;

namespace API.Helpers {
    public class MappingProfiles : Profile{
        public MappingProfiles() {
            CreateMap<Distrito, DistritoDTO>();
            CreateMap<TipoUsuario, TipoEleccionDTO>();
            CreateMap<Institucion, InstitucionDTO>();
            CreateMap<Persona, PersonaDTO>();
        }
    }
}
