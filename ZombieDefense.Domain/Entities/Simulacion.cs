using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieDefense.Domain.Entities {
    public class Simulacion {
        public int SimulacionId { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public int TiempoDisponibleSegundos { get; set; }

        public int BalasDisponibles { get; set; }

        public ICollection<ZombieEliminado> ZombiesEliminados { get; set; } = new List<ZombieEliminado>();
    }
}
