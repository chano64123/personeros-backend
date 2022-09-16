﻿using Core.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Data {
    public class ApplicationDbContext : DbContext {
        public ApplicationDbContext(DbContextOptions options) : base(options) {
        }

        // aca van los dbset que se crearan como tabla en la base de datos
        public DbSet<Distrito> Distrito { get; set; }
        public DbSet<TipoUsuario> TipoUsuario { get; set; }
        public DbSet<Institucion> Institucion { get; set; }
        public DbSet<Persona> Persona { get; set; }
        public DbSet<TipoEleccion> TipoEleccion { get; set; }
        public DbSet<Mesa> Mesa { get; set; }
        public DbSet<Acta> Acta { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<DesignacionMesa> DesignacionMesa { get; set; }
        public DbSet<DesignacionInstitucion> DesignacionInstitucion { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
