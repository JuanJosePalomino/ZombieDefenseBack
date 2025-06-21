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
    public class DefenseStrategyService : IDefenseStrategyService {

        private readonly IZombieRepository _zombieRepository;
        public DefenseStrategyService(IZombieRepository zombieRepository) {
            _zombieRepository = zombieRepository;
        }

        public async Task<List<SelectedZombie>> CalculateOptimalStrategyAsync(int availableBullets, int availableSeconds) {

            var zombies = await _zombieRepository.GetAvailableZombiesAsync();

            var orderedZombies = zombies.OrderByDescending(z => (double)z.TipoZombie.PuntajeOtorgado / (z.TipoZombie.BalasRequeridas + z.TipoZombie.TiempoDisparoSegundos));

            var result = new List<SelectedZombie>();
            int remainingBullets = availableBullets;
            int remainingTime = availableSeconds;

            foreach (var zombie in orderedZombies) {
                var zombieType = zombie.TipoZombie;
                if (remainingBullets >= zombieType.BalasRequeridas && remainingTime >= zombieType.TiempoDisparoSegundos) {
                    result.Add(new SelectedZombie {
                        ZombieId = zombie.ZombieId,
                        TipoZombie = zombieType,
                        PuntajeObtenido = zombieType.PuntajeOtorgado
                    });

                    remainingBullets -= zombieType.BalasRequeridas;
                    remainingTime -= zombieType.TiempoDisparoSegundos;

                }
            }

            return result.OrderByDescending(z => z.TipoZombie.NivelAmenaza).ToList();
        }

    }
}
