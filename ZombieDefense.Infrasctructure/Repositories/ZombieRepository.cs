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
    public class ZombieRepository : IZombieRepository {

        private readonly AppDbContext _context;

        public ZombieRepository(AppDbContext context) {
            _context = context;
        }

        public async Task CreateAsync(Zombie zombie) {
            _context.Zombies.Add(zombie);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id) {
            var zombie = await _context.Zombies.FindAsync(id);
            if (zombie != null) {
                _context.Zombies.Remove(zombie);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Zombie>> GetAvailableZombiesAsync() {
            return await _context.Zombies.Include(z => z.TipoZombie).ToListAsync();
        }

        public async Task<Zombie?> GetByIdAsync(int id) {
            return await _context.Zombies.Include(z => z.TipoZombie).FirstOrDefaultAsync(z => z.ZombieId == id);
        }

    }
}
