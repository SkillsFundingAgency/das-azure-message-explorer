using System;

namespace SFA.DAS.Azure.MessageExploreConsole.Messages
{
    public class MessageLookUp
    {
        public string Name { get; }
        public Type Type { get; }

        public MessageLookUp(string name, Type type)
        {
            Name = name;
            Type = type;
        }
    }
}
