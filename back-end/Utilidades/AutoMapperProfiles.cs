namespace back_end.Utilidades
{
    using AutoMapper;
    using back_end.Dto;
    using back_end.Entidades;

    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Genero, GeneroDto>().ReverseMap();
            CreateMap<GeneroCreacionDto, Genero>();
            CreateMap<Actor, ActorDto>().ReverseMap();
            CreateMap<ActorCreacionDto, Actor>()
                .ForMember(x => x.Foto,options => options.Ignore());

        }
    }
}
