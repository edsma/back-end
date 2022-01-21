﻿namespace back_end.Utilidades
{
    using AutoMapper;
    using back_end.Dto;
    using back_end.Entidades;
    using NetTopologySuite.Geometries;
    using System.Collections.Generic;

    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles(GeometryFactory geometryFactory)
        {
            CreateMap<Genero, GeneroDto>().ReverseMap();
            CreateMap<GeneroCreacionDto, Genero>();
            CreateMap<Actor, ActorDto>().ReverseMap();
            CreateMap<ActorCreacionDto, Actor>()
                .ForMember(x => x.Foto,options => options.Ignore());
            CreateMap<CineCreacionDto, Cine>()
                .ForMember(x => x.Ubicacion, x => x.MapFrom(dto =>
                     geometryFactory.CreatePoint(new Coordinate(dto.Longitud, dto.Latitud))));
            CreateMap<Cine, CineDto>()
                .ForMember(x => x.Latitud, dto => dto.MapFrom(campo => campo.Ubicacion.Y))
                .ForMember(x => x.Longitud, dto => dto.MapFrom(campo => campo.Ubicacion.X));
            CreateMap<PeliculaCreacionDto, Peliculas>()
                .ForMember(x => x.Poster, opciones => opciones.Ignore())
                .ForMember(x => x.PeliculaGenero, opciones => opciones.MapFrom(MapearPeliculasGenero))
                .ForMember(x => x.PeliculaCine, opciones => opciones.MapFrom(MapearPeliculasCine))
                .ForMember(x => x.PeliculasActores, opciones => opciones.MapFrom(MapearPeliculasActor));

        }

        private List<PeliculasActores> MapearPeliculasActor(PeliculaCreacionDto peliculaCreacionDto, Peliculas pelicula)
        {
            List<PeliculasActores> resultado = new List<PeliculasActores>();
            if (peliculaCreacionDto.actorCreacionPeliculasDto == null)
            {
                return resultado;
            }

            foreach (ActorPeliculasCreacionDto actor in peliculaCreacionDto.actorCreacionPeliculasDto)
            {
                resultado.Add(new PeliculasActores() { ActorId =  actor.Id, Personaje = actor.Personaje});
            }

            return resultado;
        }

        private List<PeliculaCines> MapearPeliculasCine(PeliculaCreacionDto peliculaCreacionDto, Peliculas pelicula)
        {
            List<PeliculaCines> resultado = new List<PeliculaCines>();
            if (peliculaCreacionDto.CinesIds == null)
            {
                return resultado;
            }

            foreach (int id in peliculaCreacionDto.CinesIds)
            {
                resultado.Add(new PeliculaCines() {CineId  = id });
            }

            return resultado;
        }

        private List<PeliculaGenero> MapearPeliculasGenero(PeliculaCreacionDto peliculaCreacionDto, Peliculas pelicula)
        {
            List<PeliculaGenero> resultado = new List<PeliculaGenero>();
            if (peliculaCreacionDto.GenerosIds == null)
            {
                return resultado;
            }

            foreach (int id in peliculaCreacionDto.GenerosIds)
            {
                resultado.Add(new PeliculaGenero() {GeneroId = id});
            }

            return resultado;
        }
    }
}
