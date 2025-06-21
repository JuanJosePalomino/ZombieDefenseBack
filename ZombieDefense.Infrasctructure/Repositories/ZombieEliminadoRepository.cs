using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieDefense.Domain.Entities;
using ZombieDefense.Domain.Interfaces;
using ZombieDefense.Infrasctructure.Data;

namespace ZombieDefense.Infrasctructure.Repositories {
    public class ZombieEliminadoRepository : IZombieEliminadoRepository {


        private readonly AppDbContext _context;

        public ZombieEliminadoRepository(AppDbContext context) {
            _context = context;
        }
        public async Task CreateRangeAsync(IEnumerable<ZombieEliminado> eliminaciones) {
            _context.ZombiesEliminados.AddRange(eliminaciones);
            await _context.SaveChangesAsync();
        }
    }
}
