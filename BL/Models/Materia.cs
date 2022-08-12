using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BL.Models
{
    public partial class Materia
    {
        public Materia()
        {
            Alumnos = new HashSet<Alumno>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter name"), MaxLength(30)]
        public string? MateriaName { get; set; }
        public string? MateriaProfesor { get; set; }

        public virtual ICollection<Alumno>? Alumnos { get; set; }
    }
}
