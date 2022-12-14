
using AplicacionE.Helpers;
using AplicacionE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using BL.Models;
namespace AplicacionE.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class MateriasController : ControllerBase
    {
        private readonly EjemploContext _db;
        private readonly ILogger<MateriasController> _logger;
     

        public MateriasController(EjemploContext db, ILogger<MateriasController> logger)
        {
            _db = db;
            _logger = logger;
        }



        /// <summary>
        /// GetMaterias() obtiene una lista completa de la tabla materias
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(Materia))]
        [ProducesResponseType(400)] // Bad Request
        public async Task<ActionResult<Paginacion<Materia>>> GetMaterias(
                                                                                 int pagina = 1,
                                                                                 int registros_por_pagina = 10)
        {

            _logger.LogInformation("Obteniendo Datos Materias");
            List<Materia> _Materias;
            Paginacion<Materia> _PaginadorMaterias;
            _Materias = _db.Materias.ToList();
            ///////////////////////////
            // SISTEMA DE PAGINACIÓN //
            ///////////////////////////
            int _TotalRegistros = 0;
            int _TotalPaginas = 0;
            // Número total de registros de la tabla Customers
            _TotalRegistros = _Materias.Count();
            // Obtenemos la 'página de registros' de la tabla Customers
            _Materias = _Materias.Skip((pagina - 1) * registros_por_pagina)
                                             .Take(registros_por_pagina)
                                             .ToList();
            // Número total de páginas de la tabla Customers
            _TotalPaginas = (int)Math.Ceiling((double)_TotalRegistros / registros_por_pagina);

            

            // Instanciamos la 'Clase de paginación' y asignamos los nuevos valores
            _PaginadorMaterias = new Paginacion<Materia>()
            {
                RegistrosPorPagina = registros_por_pagina,
                TotalRegistros = _TotalRegistros,
                TotalPaginas = _TotalPaginas,
                PaginaActual = pagina,
                Resultado = _Materias
            };

            return _PaginadorMaterias;

            //var lista = await _db.Materias.OrderBy(c => c.MateriaName).ToListAsync();
            //return Ok(lista);
        }

        /// <summary>
        /// GetMaterias(Id) obtiene un objeto de la tabla Materias.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Objeto</returns>
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        [ProducesResponseType (200, Type = typeof(Materia))]
        [ProducesResponseType (400)] // Bad Request
        [ProducesResponseType (404)]// Not Foud
        public async Task<IActionResult> GetMaterias(int id)
        {
            var obj = await _db.Materias.FirstOrDefaultAsync(c => c.Id == id);
            if (obj == null)
            {
                _logger.LogWarning($"La materia {id} no ha sido encontrada");
                return NotFound();
            }
            _logger.LogInformation($"Materia {id} encontrada");
            return Ok(obj);
        }

        /// <summary>
        /// PostMateria([FromBody] Materia materias) Permite ingresar una nueva materia la tabla de Materias
        /// </summary>
        /// <param name="materias">Datos necesarias para ingresar</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(201)] // Created
        [ProducesResponseType(400)] // Bad Request
        [ProducesResponseType(500)] // Error Interno

        public async Task<IActionResult> PostMateria([FromBody] Materia materias)
        {
            if (materias == null)
            {
                _logger.LogError("Error al ingresar materia");
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("Error al ingresar materia");
                return BadRequest(ModelState);
            }
            if (!MateriaExists(materias.MateriaName))
            {
                _logger.LogInformation("Materia ingresada");
                await _db.AddAsync(materias);
                await _db.SaveChangesAsync();
                return CreatedAtRoute("GetMaterias", new { id = materias.Id }, materias);
            }
            else
            {
                _logger.LogError("Error al ingresar materia");
                return BadRequest(ModelState);
            }
            
        }

        private bool MateriaExists(string id)
        {
            return _db.Materias.Any(e => e.MateriaName == id);
        }

    }
}
