using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieDefense.Domain.Entities {
    public class ZombieEliminado {
        public int ZombieId { get; set; }
        public int SimulacionId { get; set; }
        public int PuntosObtenidos { get; set; }
        public DateTime FechaEliminacion { get; set; } = DateTime.Now;
        public Zombie Zombie { get; set; } = null!;
        public Simulacion Simulacion { get; set; } = null!;
    }
}
