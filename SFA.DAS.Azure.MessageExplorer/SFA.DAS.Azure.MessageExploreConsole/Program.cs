using System;
using System.Collections.Generic;
using System.Text;
using SFA.DAS.Azure.MessageExploreConsole.AzureQueue;
using SFA.DAS.Azure.MessageExploreConsole.Messages;

namespace SFA.DAS.Azure.MessageExploreConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var messageTypeSelector = new MessageTypeSelector();
            var queueManagerFactory = new AzureQueueClientFactory();

            var connectionString = GetConnectionString();

            var messageLookUp = messageTypeSelector.SelectMessageType();

            if (messageLookUp == null) return;

            var queueManager = queueManagerFactory.Create(messageLookUp.Type, connectionString);

            var subscriptionName = GetSubscriptionName();

            var messages = DisplayMessages(queueManager, subscriptionName);

            var usePrimaryColour = true;

            foreach (var message in messages)
            {
                Console.ForegroundColor = usePrimaryColour ? ConsoleColor.Gray : ConsoleColor.Cyan;
                usePrimaryColour = !usePrimaryColour;

                Console.WriteLine(message);
            }

            Console.ReadLine();
        }

        private static IEnumerable<string> DisplayMessages(IAzureQueueClient queueManager, string subscriptionName)
        {
            var messages = new List<string>();

            var option = GetMessageCollectionOption();

            switch (option)
            {
                case null:
                    return messages;

                case 1:
                {
                    var result = queueManager.GetNextMessage(subscriptionName).Result;

                    messages.Add(result);
                    break;
                }
                case 2:
                {
                    var messageCount = GetMessageCount();
                    var result = queueManager.GetBatchMessages(subscriptionName, messageCount).Result;

                    messages.AddRange(result);
                    break;
                }
                case 3:
                {
                    var messageCount = GetMessageCount();
                    var startIndex = GetCollectionStartIndex();

                    var result = queueManager.GetBatchMessages(subscriptionName, startIndex, messageCount).Result;

                    messages.AddRange(result);
                    break;
                }
            }

            return messages;
        }

        private static long GetCollectionStartIndex()
        {
            var count = -1;

            do
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("How many messages would you like to collect?: ");

                if (!int.TryParse(Console.ReadLine(), out count))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid selection you must enter a number of zero or above.");
                }

            } while (count < 0);

            return count;
        }

        private static int GetMessageCount()
        {
            var count = 0;

            do
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("How many messages would you like to collect?: ");

                if (!int.TryParse(Console.ReadLine(), out count))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid selection you must enter a number above zero.");
                }

            } while (count < 1);

            return count;
        }

        private static int? GetMessageCollectionOption()
        {
            var menuText = GetMessageCollectionMenuText();

            bool selectionSuccessful;
            var selection = 0;

            do
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(menuText);

                selectionSuccessful = int.TryParse(Console.ReadLine(), out selection);

                if (!selectionSuccessful)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid selection choice please enter a number of the require choice.");
                }

            } while (!selectionSuccessful);

            return selection == 0 ? (int?) null : selection;
        }

        private static string GetMessageCollectionMenuText()
        {
            var menuBulider = new StringBuilder();

            menuBulider.AppendLine("1. Get next available message");
            menuBulider.AppendLine("2. Get messages");
            menuBulider.AppendLine("3. Get message fron given point");
            menuBulider.AppendLine("0. Quit");
            menuBulider.AppendLine(Environment.NewLine + "Please select a collection option(0-3): ");

            return menuBulider.ToString();
        }

        private static string GetConnectionString()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Enter Azure queue connection string: ");
            var connectionString = Console.ReadLine();

            Console.WriteLine();
            return connectionString;
        }

        private static string GetSubscriptionName()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(Environment.NewLine + "Enter Azure queue subscription name: ");

            var subscriptionName = Console.ReadLine();
            return subscriptionName;
        }
    }
}
