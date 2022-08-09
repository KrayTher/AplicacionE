
using AplicacionE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AplicacionE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlumnoController : ControllerBase
    {
        private readonly EjemploContext _db;

        public AlumnoController(EjemploContext db)
        {
            _db = db;
        }
        /// <summary>
        /// prueba
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<Paginacion<Alumno>>> GetAlumno(int pagina = 1,
                                                    int registros_por_pagina = 10)
        {
            List<Alumno> _Alumno;
            Paginacion<Alumno> _PaginadorAlumno;
            _Alumno = await _db.Alumnos.OrderBy(p => p.Id).Include(p => p.Materia).ToListAsync();
            ///////////////////////////
            // SISTEMA DE PAGINACIÓN //
            ///////////////////////////
            int _TotalRegistros = 0;
            int _TotalPaginas = 0;
            // Número total de registros de la tabla Customers
            _TotalRegistros = _Alumno.Count();
            // Obtenemos la 'página de registros' de la tabla Customers
            _Alumno = _Alumno.Skip((pagina - 1) * registros_por_pagina)
                                             .Take(registros_por_pagina)
                                             .ToList();
            // Número total de páginas de la tabla Customers
            _TotalPaginas = (int)Math.Ceiling((double)_TotalRegistros / registros_por_pagina);

            // Instanciamos la 'Clase de paginación' y asignamos los nuevos valores
            _PaginadorAlumno = new Paginacion<Alumno>()
            {
                RegistrosPorPagina = registros_por_pagina,
                TotalRegistros = _TotalRegistros,
                TotalPaginas = _TotalPaginas,
                PaginaActual = pagina,
                Resultado = _Alumno
            };

            return _PaginadorAlumno;

            //var lista = await _db.Alumnos.OrderBy(p => p.Id).Include(p => p.Materia).ToListAsync();
            //return Ok(lista);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAlumno(int id)
        {
            var obj = await _db.Alumnos.Include(c => c.Materia).FirstOrDefaultAsync(c => c.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            return Ok(obj);
        }

        [HttpPost]
        public async Task<IActionResult> PostAlumno([FromBody] Alumno alumno)
        {

            
            if (alumno == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _db.AddAsync(alumno);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
