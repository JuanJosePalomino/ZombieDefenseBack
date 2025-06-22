using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieDefense.Application.Dto;
using ZombieDefense.Application.Interfaces;
using ZombieDefense.Domain.Entities;
using ZombieDefense.Domain.Interfaces;

namespace ZombieDefense.Application.Services {
    public class SimulacionService : ISimulacionService{
        private readonly ISimulacionRepository _simulacionRepository;
        private readonly IZombieEliminadoRepository _zombieEliminadoRepository;

        public SimulacionService(ISimulacionRepository simulacionRepository,
            IZombieEliminadoRepository zombieEliminadoRepository) {
            _simulacionRepository = simulacionRepository;
            _zombieEliminadoRepository = zombieEliminadoRepository;
        }

        /// <summary>
        /// Registra una nueva simulación en la base de datos junto con los zombies eliminados durante la misma.
        /// </summary>
        /// <param name="request">
        /// Objeto que contiene la información de la simulación, incluyendo balas disponibles,
        /// tiempo disponible y la lista de zombies eliminados.
        /// </param>
        /// <returns>
        /// El identificador único (<c>SimulacionId</c>) de la simulación registrada.
        /// </returns>
        /// <exception cref="Exception">
        /// Se lanza si no se especifican zombies eliminados en la solicitud.
        /// </exception>
        /// <remarks>
        /// Este método crea primero la entidad <see cref="Simulacion"/> y luego registra las entidades <see cref="ZombieEliminado"/> relacionadas.
        /// </remarks>
        public async Task<int> RegisterSimulacionAsync(SimulacionRequest request) {
            if (request.ZombiesEliminados == null || !request.ZombiesEliminados.Any())
                throw new Exception("No se puede registrar una simulación sin zombies eliminados.");

            var simulacion = new Simulacion {
                TiempoDisponibleSegundos = request.SecondsAvailable,
                BalasDisponibles = request.Bullets,
                FechaCreacion = DateTime.Now
            };

            await _simulacionRepository.CreateAsync(simulacion);

            var eliminaciones = request.ZombiesEliminados.Select(z => new ZombieEliminado {
                SimulacionId = simulacion.SimulacionId,
                ZombieId = z.ZombieId,
                PuntosObtenidos = z.PuntajeObtenido,
                FechaEliminacion = DateTime.Now
            });

            await _zombieEliminadoRepository.CreateRangeAsync(eliminaciones);

            return simulacion.SimulacionId;
        }

    }
}
