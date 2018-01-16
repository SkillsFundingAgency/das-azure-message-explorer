using System;

namespace SFA.DAS.Azure.MessageExploreConsole.AzureQueue
{
    public class AzureQueueClientFactory
    {
        public IAzureQueueClient Create(Type messageType, string connectionString)
        {
            var genericTypeTemplate = typeof(AzureQueueClient<>);
            var typeArgs = new[]{ messageType };

            var genericType = genericTypeTemplate.MakeGenericType(typeArgs);

            return genericType.GetConstructor(new[] { typeof(string)})
                              ?.Invoke(new object[]{ connectionString }) as IAzureQueueClient;
        }
    }
}
