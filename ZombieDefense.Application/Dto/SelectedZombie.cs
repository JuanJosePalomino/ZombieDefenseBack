using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieDefense.Domain.Entities;

namespace ZombieDefense.Application.Dto {
    public class SelectedZombie {
        public int ZombieId { get; set; }

        public TipoZombie TipoZombie { get; set; } = null!;

        public int PuntajeObtenido { get; set; }
    }
}
