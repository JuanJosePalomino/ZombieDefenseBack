using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieDefense.Application.Dto {
    public class SimulacionRequest {
        public int Bullets { get; set; }
        public int SecondsAvailable { get; set; }
        public List<SelectedZombieDto> ZombiesEliminados { get; set; } = new();
    }
}
