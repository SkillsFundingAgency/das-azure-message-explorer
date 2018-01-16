using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using SFA.DAS.Messaging.Helpers;

namespace SFA.DAS.Azure.MessageExploreConsole.AzureQueue
{
    public class AzureQueueClient<T> : IAzureQueueClient
    {
        private readonly string _connectionString;

        public AzureQueueClient(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<string> GetNextMessage(string subscriptionName)
        {
            var client = GetClient(subscriptionName);

            var brokeredMessage = await client.PeekAsync();

            return ConvertToMessageData(brokeredMessage);
        }
       
        public async Task<ICollection<string>> GetBatchMessages(string subscriptionName, int messageCount)
        {
            var client = GetClient(subscriptionName);

            var brokeredMessages = await client.PeekBatchAsync(messageCount);

            return ConvertToMessageData(brokeredMessages);
        }

        public async Task<ICollection<string>> GetBatchMessages(string subscriptionName, long startIndex, int messageCount)
        {
            var client = GetClient(subscriptionName);

            var brokeredMessages = await client.PeekBatchAsync(startIndex, messageCount);

            return ConvertToMessageData(brokeredMessages);
        }
        private SubscriptionClient GetClient(string subscriptionName)
        {
            var topicName = MessageGroupHelper.GetMessageGroupName<T>();

            return SubscriptionClient.CreateFromConnectionString(_connectionString, topicName, subscriptionName);
        }

        private static ICollection<string> ConvertToMessageData(IEnumerable<BrokeredMessage> brokeredMessages)
        {
            if(brokeredMessages == null || !brokeredMessages.Any())
                return new List<string>{"No messages"};

            return brokeredMessages.Select(ConvertToMessageData).ToList();
        }

        private static string ConvertToMessageData(BrokeredMessage brokeredMessage)
        {
            if (brokeredMessage == null)
                return "No message";

            var messageDataBuilder = new StringBuilder();

            messageDataBuilder.AppendLine($"Message ID: {brokeredMessage.MessageId}");
            messageDataBuilder.AppendLine($"Message Seq Num: {brokeredMessage.SequenceNumber}");
            messageDataBuilder.AppendLine($"Message Session ID: {brokeredMessage.SessionId}");
            messageDataBuilder.AppendLine($"Message label: {brokeredMessage.Label}");
            messageDataBuilder.AppendLine($"Message content Type: {brokeredMessage.ContentType}");
            messageDataBuilder.AppendLine($"Message enqueued: {brokeredMessage.EnqueuedTimeUtc:F}");
            messageDataBuilder.AppendLine($"Message expires: {brokeredMessage.ExpiresAtUtc:F}");
            messageDataBuilder.AppendLine($"Message delivery count: {brokeredMessage.DeliveryCount}");
            messageDataBuilder.AppendLine($"Message state: {Enum.GetName(typeof(MessageState),brokeredMessage.State)}");
           
            var messageContent = brokeredMessage.GetBody<T>();
            messageDataBuilder.AppendLine("Message Content: " + JsonConvert.SerializeObject(messageContent, Formatting.Indented));

            return messageDataBuilder.ToString();
        }
    }
}
