using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BL.Models
{
    public partial class Alumno
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter name"), MaxLength(30)]
        public string Name { get; set; } = null!;
        public int? MateriaId { get; set; }
        public int Curso { get; set; }

        public virtual Materia? Materia { get; set; }
    }
}
