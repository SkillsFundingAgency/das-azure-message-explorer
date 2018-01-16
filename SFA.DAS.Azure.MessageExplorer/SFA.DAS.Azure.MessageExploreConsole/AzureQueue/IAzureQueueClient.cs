using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.Azure.MessageExploreConsole.AzureQueue
{
    public interface IAzureQueueClient
    {
        Task<string> GetNextMessage(string subscriptionName);

        Task<ICollection<string>> GetBatchMessages(string subscriptionName, int messageCount );

        Task<ICollection<string>> GetBatchMessages(string subscriptionName, long startIndex, int messageCount);

    }
}