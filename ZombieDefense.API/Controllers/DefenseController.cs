using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZombieDefense.Application.Dto;
using ZombieDefense.Application.Interfaces;

namespace ZombieDefense.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class DefenseController : ControllerBase {

        private readonly IDefenseStrategyService _defenseStrategyService;
        private readonly ISimulacionService _simulacionService;
        public DefenseController(IDefenseStrategyService defenseStrategyService,
            ISimulacionService simulacionService) {
            _defenseStrategyService = defenseStrategyService;
            _simulacionService = simulacionService;
        }

        /// <summary>
        /// Calcula la estrategia óptima de defensa
        /// </summary>
        /// <param name="bullets">Balas disponibles</param>
        /// <param name="secondsAvailable">Segundos disponibles</param>
        /// <returns>Lista de zombies seleccionados para eliminar</returns>
        [HttpGet("optimal-strategy")]
        public async Task<IActionResult> GetStrategy([FromQuery] int bullets, [FromQuery] int secondsAvailable) {
            if (bullets <= 0 || secondsAvailable <= 0) {
                return BadRequest("Debes enviar balas y segundos disponibles válidos");
            }

            var strategy = await _defenseStrategyService.CalculateOptimalStrategyAsync(bullets, secondsAvailable);
            return Ok(strategy);
        }

        [HttpPost("register-simulation")]
        public async Task<IActionResult> RegistrarSimulacion([FromBody] SimulacionRequest request) {
            if (request.Bullets <= 0 || request.SecondsAvailable <= 0)
                return BadRequest("Valores inválidos.");

            try {
                var simulacionId = await _simulacionService.RegisterSimulacionAsync(request);
                return Ok(new { SimulacionId = simulacionId });
            } catch (Exception ex) {
                return BadRequest(new { error = ex.Message });
            }
        }

    }
}
