using MessagePipe;
using VContainer;
using VContainer.Unity;

namespace SampleCode.Quest
{
    public class MessagePipeInstaller : IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            var options = builder.RegisterMessagePipe();
            builder.RegisterMessageBroker<ProgressiveQuestTopic>(options);
        }
    }
}