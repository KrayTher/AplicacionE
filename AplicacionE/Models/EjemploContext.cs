using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using BL.Models;
namespace AplicacionE.Models
{
    public partial class EjemploContext : DbContext
    {
        public EjemploContext()
        {
        }
        /// <summary>
        /// Contructor Context
        /// </summary>
        /// <param name="options"></param>
        public EjemploContext(DbContextOptions<EjemploContext> options)
            : base(options)
        {
        }
        /// <summary>
        /// Referencia a Models
        /// </summary>
        public virtual DbSet<Alumno> Alumnos { get; set; } = null!;
        public virtual DbSet<Materia> Materias { get; set; } = null!;

        public virtual DbSet<UserInfo>? UserInfos { get; set; } = null;
        /// <summary>
        /// Configuracion de coneccion
        /// </summary>
        /// <param name="optionsBuilder"></param>

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost\\MSSQLSERVERLEOAU;Database=Ejemplo;Trusted_Connection=True;");
            }
        }
        /// <summary>
        /// Se crea el modelo relacional entre Alumnos y materias
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Alumno>(entity =>
            {
                entity.ToTable("Alumno");

                entity.HasIndex(e => e.MateriaId, "IX_Alumno_MateriaID");

                entity.Property(e => e.MateriaId).HasColumnName("MateriaID");

                entity.HasOne(d => d.Materia)
                    .WithMany(p => p.Alumnos)
                    .HasForeignKey(d => d.MateriaId);
            });

            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("UserInfo");
                entity.Property(e => e.UserId).HasColumnName("UserId");
                entity.Property(e => e.DisplayName).HasMaxLength(60).IsUnicode(false);
                entity.Property(e => e.UserName).HasMaxLength(30).IsUnicode(false);
                entity.Property(e => e.Email).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.Password).HasMaxLength(20).IsUnicode(false);
                entity.Property(e => e.CreatedDate).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }


        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
