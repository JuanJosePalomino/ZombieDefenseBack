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
