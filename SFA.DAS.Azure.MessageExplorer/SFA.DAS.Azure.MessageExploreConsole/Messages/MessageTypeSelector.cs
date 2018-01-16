using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFA.DAS.Azure.MessageExploreConsole.Messages
{
    public class MessageTypeSelector
    {
        private readonly ICollection<MessageLookUp> _messageTypes;
        private readonly string _consoleMenuText;

        public MessageTypeSelector()
        { 
            var types = new List<MessageLookUp>();

            types.AddRange(MessageTypeExtractor.GetManageApprenticeshipMessages());
            types.AddRange(MessageTypeExtractor.GetCommitmentMessages());

            _messageTypes = types;

            var menuBuilder = new StringBuilder();

            for (var index = 0; index < _messageTypes.Count; index++)
            {
                menuBuilder.AppendLine($"{index + 1}. {_messageTypes.ElementAt(index).Name}");
            }

            menuBuilder.AppendLine("0. Quit");

            menuBuilder.AppendLine(Environment.NewLine + 
                $"Please select a message type to collect (0 - {_messageTypes.Count}): ");

            _consoleMenuText = menuBuilder.ToString();
        }

        public MessageLookUp SelectMessageType()
        {
            bool selectionSuccessful;
            var selection = 0;

            do
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(_consoleMenuText);

                selectionSuccessful = int.TryParse(Console.ReadLine(), out selection);

                if(!selectionSuccessful)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid selection choice please enter a number of the require choice.");
                }

            } while (!selectionSuccessful);

            return selection == 0 ? null : _messageTypes.ElementAt(selection - 1);
        }
    }
}
