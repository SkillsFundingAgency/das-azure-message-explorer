using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SFA.DAS.Azure.MessageExploreConsole.Messages;
using SFA.DAS.Commitments.Events;
using SFA.DAS.EmployerAccounts.Events.Messages;
using SFA.DAS.Messaging.Attributes;

namespace SFA.DAS.Azure.MessageExploreConsole
{
    public class MessageTypeExtractor
    {
        public static ICollection<MessageLookUp> GetManageApprenticeshipMessages()
        {
            var assembly = Assembly.GetAssembly(typeof(AccountMessageBase));

            return assembly.GetTypes().Where(t => t.GetCustomAttributes<MessageGroupAttribute>().Any()).OrderBy(x => x.Name).Select(x => new MessageLookUp(x.Name, x)).ToList();
        }

        public static ICollection<MessageLookUp> GetCommitmentMessages()
        {
            var assembly = Assembly.GetAssembly(typeof(CohortCreated));

            return assembly.GetTypes().Where(t => t.GetCustomAttributes<MessageGroupAttribute>().Any()).OrderBy(x => x.Name).Select(x => new MessageLookUp(x.Name, x)).ToList();
        }
    }
}
