using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZombieDefense.Application.Dto;
using ZombieDefense.Application.Interfaces;
using ZombieDefense.Domain.Entities;
using ZombieDefense.Domain.Interfaces;

namespace ZombieDefense.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class DefenseController : ControllerBase {

        private readonly IDefenseStrategyService _defenseStrategyService;
        private readonly ISimulacionService _simulacionService;
        private readonly IZombieRepository _zombieRepository;
        public DefenseController(IDefenseStrategyService defenseStrategyService,
            ISimulacionService simulacionService,
            IZombieRepository zombieRepository) {
            _defenseStrategyService = defenseStrategyService;
            _simulacionService = simulacionService;
            _zombieRepository = zombieRepository;
        }

        /// <summary>
        /// Calcula la estrategia óptima de defensa contra zombies, maximizando el puntaje
        /// según la cantidad de balas y tiempo disponibles.
        /// </summary>
        /// <param name="bullets">Cantidad de balas disponibles para eliminar zombies.</param>
        /// <param name="secondsAvailable">Tiempo disponible en segundos para ejecutar la estrategia.</param>
        /// <returns>
        /// Una lista de zombies seleccionados que representan la estrategia óptima.
        /// </returns>
        /// <response code="200">Estrategia óptima calculada correctamente.</response>
        /// <response code="400">Parámetros inválidos (balas o segundos no positivos).</response>
        [HttpGet("optimal-strategy")]
        public async Task<IActionResult> GetStrategy([FromQuery] int bullets, [FromQuery] int secondsAvailable) {
            if (bullets <= 0 || secondsAvailable <= 0) {
                return BadRequest("Debes enviar balas y segundos disponibles válidos");
            }

            var strategy = await _defenseStrategyService.CalculateOptimalStrategyAsync(bullets, secondsAvailable);
            return Ok(strategy);
        }

        /// <summary>
        /// Registra una simulación real de eliminación de zombies.
        /// </summary>
        /// <param name="request">Contiene el número de balas, segundos disponibles y la lista de zombies eliminados.</param>
        /// <returns>
        /// Un código 200 OK con el ID de la simulación registrada, o un código 400 Bad Request si los datos son inválidos o si ocurre un error en el proceso.
        /// </returns>
        /// <response code="200">Simulación registrada correctamente.</response>
        /// <response code="400">Error de validación o problema interno al registrar la simulación.</response>
        [HttpPost("register-simulation")]
        public async Task<IActionResult> RegistrarSimulacion([FromBody] SimulacionRequest request) {
            if (request.Bullets <= 0 || request.SecondsAvailable <= 0)
                return BadRequest("La simulación tiene valores inválidos de balas o segundos disponibles.");

            try {
                var simulacionId = await _simulacionService.RegisterSimulacionAsync(request);
                return Ok(new { SimulacionId = simulacionId });
            } catch (Exception ex) {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene la lista completa de zombies disponibles en el sistema.
        /// </summary>
        /// <returns>
        /// Una lista de zombies con sus tipos y características.
        /// </returns>
        /// <response code="200">Lista de zombies obtenida correctamente.</response>
        [HttpGet("zombies")]
        public async Task<ActionResult<IEnumerable<Zombie>>> GetZombies() {
            var zombies = await _zombieRepository.GetAvailableZombiesAsync();
            return Ok(zombies);
        }

    }
}
