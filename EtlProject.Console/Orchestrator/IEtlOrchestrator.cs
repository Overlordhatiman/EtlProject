using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtlProject.Console.Orchestrator
{
    public interface IEtlOrchestrator
    {
        Task ExecuteAsync(string csvFilePath);
    }
}
