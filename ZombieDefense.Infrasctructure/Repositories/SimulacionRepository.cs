using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieDefense.Domain.Entities;
using ZombieDefense.Domain.Interfaces;
using ZombieDefense.Infrasctructure.Data;

namespace ZombieDefense.Infrasctructure.Repositories {
    public class SimulacionRepository : ISimulacionRepository {


        private readonly AppDbContext _context;

        public SimulacionRepository(AppDbContext context) {
            _context = context;
        }

        public async Task CreateAsync(Simulacion simulacion) {
            _context.Simulaciones.Add(simulacion);
            await _context.SaveChangesAsync();
        }
    }
}
