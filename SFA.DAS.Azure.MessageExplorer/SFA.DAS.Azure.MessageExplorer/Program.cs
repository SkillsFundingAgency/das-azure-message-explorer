using System;

namespace SFA.DAS.Azure.MessageExplorer
{
    class Program
    {
        private const string connectionString = "Endpoint=sb://das-at-sbq-ns.servicebus.windows.net/;SharedAccessKeyName=Robin;SharedAccessKey=2mvlzjtkpuvnRBIRP0sMiNcViFPy/NN3Z2mjDoyxUQY=";
        private const string topicName = "agreement_created";
        private const string subscriptionName = "Activity_AgreementCreatedMessageProcessor";

        static void Main(string[] args)
        {
            var queueManager = new AzureQueueManager();

            queueManager.SetupConnection(connectionString, topicName, subscriptionName);

            Console.WriteLine(queueManager.GetNextMessage().Result);

            Console.ReadLine();
        }
    }
}
