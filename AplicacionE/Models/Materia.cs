using System;
using System.Collections.Generic;

namespace AplicacionE.Models
{
    public partial class Materia
    {
        public Materia()
        {
            Alumnos = new HashSet<Alumno>();
        }

        public int Id { get; set; }
        public string? MateriaName { get; set; }
        public string? MateriaProfesor { get; set; }

        public virtual ICollection<Alumno> Alumnos { get; set; }
    }
}
