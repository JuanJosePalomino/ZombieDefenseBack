using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieDefense.Domain.Entities;

namespace ZombieDefense.Domain.Interfaces {
    public interface IZombieRepository {

        Task<List<Zombie>> GetAvailableZombiesAsync();

        Task<Zombie?> GetByIdAsync(int id);

        Task CreateAsync(Zombie zombie);

        Task DeleteAsync(int id);
    }
}
