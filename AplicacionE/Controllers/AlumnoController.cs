
using AplicacionE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BL.Models;

namespace AplicacionE.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AlumnoController : ControllerBase
    {
        private readonly EjemploContext _db;
        private readonly ILogger<AlumnoController> _logger;

        public AlumnoController(EjemploContext db, ILogger<AlumnoController> logger)
        {
            _db = db;
            _logger = logger;
        }
        /// <summary>
        /// prueba
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<Paginacion<Alumno>>> GetAlumno(int pagina = 1,
                                                    int registros_por_pagina = 10)
        {
            _logger.LogInformation("Obteniendo Datos Alumnos");
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
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAlumno(int id)
        {
            var obj = await _db.Alumnos.Include(c => c.Materia).FirstOrDefaultAsync(c => c.Id == id);
            if (obj == null)
            {
                _logger.LogWarning($"Alumno {id} no encontrado");
                return NotFound();
            }
            _logger.LogInformation($"Alumno {id} encontrado");
            return Ok(obj);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostAlumno([FromBody] Alumno alumno)
        {

            
            if (alumno == null)
            {
                _logger.LogError("Error al ingresar Alumno");
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("Error al ingresar Alumno");
                return BadRequest(ModelState);
            }
            if (!AlumnoExists(alumno.Name))
            {
                _logger.LogInformation("Alumno ingresado");

                 await _db.AddAsync(alumno);
                 await _db.SaveChangesAsync();
                 return Ok();
            }
            else
            {
                _logger.LogError("Error al ingresar Alumno");
                return BadRequest(ModelState);
            }
            
        }

        private bool AlumnoExists(string id)
        {
            return _db.Alumnos.Any(e => e.Name == id);
        }

    }
}
