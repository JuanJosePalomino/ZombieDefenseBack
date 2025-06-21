using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieDefense.Domain.Entities;

namespace ZombieDefense.Domain.Interfaces {
    public interface IZombieEliminadoRepository {
        Task CreateRangeAsync(IEnumerable<ZombieEliminado> eliminaciones);
    }
}
