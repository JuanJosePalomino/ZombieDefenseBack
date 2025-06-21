using Xunit;
using Microsoft.EntityFrameworkCore;
using ZombieDefense.Domain.Entities;
using ZombieDefense.Infrasctructure.Data;
using ZombieDefense.Application.Dto;
using ZombieDefense.Application.Services;
using Moq;
using ZombieDefense.Domain.Interfaces;
using ZombieDefense.Infrasctructure.Repositories;

namespace ZombieDefense.Tests {
    public class SimulacionServiceTests {

        private AppDbContext CrearDbContextInMemory() {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task RegistrarSimulacionAsync_DeberiaCrearSimulacionYZombiesEliminados() {
            // Arrange
            using var context = CrearDbContextInMemory();

            var zombie1 = new Zombie {
                ZombieId = 1,
                TipoZombie = new TipoZombie {
                    TipoZombieId = 1,
                    Nombre = "Corredor",
                    BalasRequeridas = 1,
                    TiempoDisparoSegundos = 2,
                    PuntajeOtorgado = 100,
                    NivelAmenaza = 5
                },
                TipoZombieId = 1
            };

            context.TiposZombie.Add(zombie1.TipoZombie);
            context.Zombies.Add(zombie1);
            await context.SaveChangesAsync();

            var request = new SimulacionRequest {
                Bullets = 10,
                SecondsAvailable = 20,
                ZombiesEliminados = new List<SelectedZombieDto>
                {
                new SelectedZombieDto { ZombieId = 1, PuntajeObtenido = 100 }
            }
            };
            var simulacionRepo = new SimulacionRepository(context);
            var zombieEliminadoRepo = new ZombieEliminadoRepository(context);

            var service = new SimulacionService(simulacionRepo, zombieEliminadoRepo);

            // Act
            var simulacionId = await service.RegisterSimulacionAsync(request);

            // Assert
            var simulacion = await context.Simulaciones.FindAsync(simulacionId);
            Assert.NotNull(simulacion);
            Assert.Equal(10, simulacion!.BalasDisponibles);
            Assert.Equal(20, simulacion.TiempoDisponibleSegundos);

            var eliminados = context.ZombiesEliminados.Where(z => z.SimulacionId == simulacionId).ToList();
            Assert.Single(eliminados);
            Assert.Equal(100, eliminados [0].PuntosObtenidos);
            Assert.Equal(1, eliminados [0].ZombieId);
        }

    }
}
