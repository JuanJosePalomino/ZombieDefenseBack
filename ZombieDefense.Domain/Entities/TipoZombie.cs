using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieDefense.Domain.Entities {
    public class TipoZombie {

        public int TipoZombieId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int NivelAmenaza { get; set; }
        public int TiempoDisparoSegundos { get; set; }
        public int BalasRequeridas { get; set; }

        public int PuntajeOtorgado { get; set; }
    }
}
