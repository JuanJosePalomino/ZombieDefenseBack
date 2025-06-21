using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieDefense.Application.Dto;

namespace ZombieDefense.Application.Interfaces {
    public interface ISimulacionService {
        Task<int> RegisterSimulacionAsync(SimulacionRequest request);
    }
}
