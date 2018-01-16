using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;

namespace SFA.DAS.Azure.MessageExplorer
{
    public class AzureQueueManager
    {
        private string _connectionString;
        private string _topicName;
        private string _subscriptionName;
        public void SetupConnection()
        {
            Console.Write("Enter Azure queue connection string: ");
            _connectionString = Console.ReadLine();

            Console.WriteLine();

            Console.Write("Enter Azure queue topic name: ");
            _topicName = Console.ReadLine();

            Console.WriteLine();

            Console.Write("Enter Azure queue subscription name: ");
            _subscriptionName = Console.ReadLine();
        }

        public void SetupConnection(string connectionString, string  topicName, string subscription)
        {
            _connectionString = connectionString;
            _topicName = topicName;
            _subscriptionName = subscription;
        }

        public async Task<string> GetNextMessage()
        {
            var client =
                SubscriptionClient.CreateFromConnectionString(_connectionString, _topicName, _subscriptionName);

            var brokeredMessage = await client.PeekAsync();

            using (var stream = brokeredMessage.GetBody<Stream>())
            {
                using (var reader = new StreamReader(stream))
                {
                    var content = reader.ReadToEnd();
                    reader.Close();

                    return content;
                }

            }
        }
    }
}
