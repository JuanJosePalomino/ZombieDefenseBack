using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieDefense.Domain.Entities {
    public class Zombie {

        public int ZombieId { get; set; }
        public int TipoZombieId { get; set; }
        public bool Eliminado { get; set; }
        public TipoZombie TipoZombie { get; set; } = null!;

    }
}
