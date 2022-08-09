using System;
using System.Collections.Generic;

namespace AplicacionE.Models
{
    public partial class Alumno
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int? MateriaId { get; set; }
        public int Curso { get; set; }

        public virtual Materia? Materia { get; set; }
    }
}
