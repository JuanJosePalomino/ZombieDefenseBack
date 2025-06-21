using Moq;
using ZombieDefense.Application.Services;
using ZombieDefense.Domain.Entities;
using ZombieDefense.Domain.Interfaces;

namespace ZombieDefense.Tests {
    public class DefenseStrategyServiceTests {

        [Fact]
        public async Task PruebaEficiencia() {
            var zombies = new List<Zombie> {
                new Zombie {
                    ZombieId = 1,
                    TipoZombie = new TipoZombie {
                        Nombre = "Tanque",
                        BalasRequeridas = 3,
                        TiempoDisparoSegundos = 4,
                        PuntajeOtorgado = 100,
                        NivelAmenaza = 10
                    }
                },
                new Zombie {
                    ZombieId = 1,
                    TipoZombie = new TipoZombie {
                        Nombre = "Enano",
                        BalasRequeridas = 3,
                        TiempoDisparoSegundos = 5,
                        PuntajeOtorgado = 500,
                        NivelAmenaza = 10
                    }
                }
            };

            var mockRepo = new Mock<IZombieRepository>();
            mockRepo.Setup(r => r.GetAvailableZombiesAsync()).ReturnsAsync(zombies);
            var service = new DefenseStrategyService(mockRepo.Object);

            var result = await service.CalculateOptimalStrategyAsync(3, 5);

            Assert.NotNull(result);
            Assert.Contains(result, z => z.TipoZombie.Nombre == "Enano");
            Assert.All(result, z => {
                Assert.True(z.PuntajeObtenido > 0);
                Assert.True(z.TipoZombie.BalasRequeridas <= 4);
                Assert.True(z.TipoZombie.TiempoDisparoSegundos <= 6);
            });
        }

        [Fact]
        public async Task ZombiesExcedenRecursos() {

            var zombies = new List<Zombie> {
                new Zombie {
                    ZombieId = 1,
                    TipoZombie = new TipoZombie {
                        Nombre = "Tanque",
                        BalasRequeridas = 10,
                        TiempoDisparoSegundos = 15,
                        PuntajeOtorgado = 500,
                        NivelAmenaza = 10
                    }
                }
            };

            var mockRepo = new Mock<IZombieRepository>();
            mockRepo.Setup(r => r.GetAvailableZombiesAsync()).ReturnsAsync(zombies);
            var service = new DefenseStrategyService(mockRepo.Object);

            var result = await service.CalculateOptimalStrategyAsync(5, 5);
            Assert.Empty(result);

        }
    }
}
