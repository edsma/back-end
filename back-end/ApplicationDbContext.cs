using back_end.Entidades;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PeliculasActores>()
                .HasKey(x => new { x.ActorId, x.PeliculaId });
            modelBuilder.Entity<PeliculaGenero>()
                .HasKey(x => new { x.PeliculaId, x.GeneroId});
            modelBuilder.Entity<PeliculaCines>()
                .HasKey(x => new { x.PeliculaId, x.CineId});
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Genero> Genero { get; set; }

        public DbSet<Actor> Actor { get; set; }
        public DbSet<Cine> Cines { get; set; }

        public DbSet<Peliculas> Peliculas { get; set; }
        public DbSet<PeliculasActores> PeliculasActores { get; set; }
        public DbSet<PeliculaGenero> PeliculaGenero { get; set; }
        public DbSet<PeliculaCines> PeliculaCines { get; set; }

        public DbSet<Rating> Ratings { get; set; }

        public DbSet<Polizas> Polizas { get; set; }

    }
}
