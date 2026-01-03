using System.Globalization;
using KnowU.Domain.Knowledge.Contract;
using KnowU.Domain.Storage.Contract;

namespace KnowU.Domain.Knowledge;

public class AgentRunner
{
    public AgentRunner(IStorage _storage, IList<IAgent> agents)
    {
        Task.Run(async () =>
        {
            await Task.Delay(1000);
            
            foreach (var document in _storage.Documents)
            {
                
            }
        });
    }
}
