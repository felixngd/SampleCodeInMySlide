using MessagePipe;
using VContainer;
using NUnit.Framework;
using SampleCode.Quest;

public class QuestTest
{
   
    private IScopedObjectResolver _lifetimeScope;

    [SetUp]
    public void SetUp()
    {
        // Create a container builder
        var builder = new ContainerBuilder();

        // Register dependencies
        
        var options = builder.RegisterMessagePipe();
        builder.RegisterMessageBroker<ProgressiveQuestTopic>(options);
        builder.Register<QuestEventPublisher>(Lifetime.Singleton);

        // Build the container
        var container = builder.Build();

        // Create a lifetime scope
        _lifetimeScope = container.CreateScope();
    }

    [Test]
    public void QuestCompletionTest()
    {
        // Resolve the QuestEventPublisher
        var questEventPublisher = _lifetimeScope.Resolve<QuestEventPublisher>();
        // Arrange
        var q = new ProgressiveDailyQuest("01", "02", 3, questEventPublisher);
        //q.IncrementProgress();
        q.Complete();

        // Act
        string questName = q.Name;
        string questDescription = q.Description;
        int requiredProgress = q.RequiredProgress;
        
        // Assert
        Assert.AreEqual("01", questName);
        Assert.AreEqual("02", questDescription);
        Assert.AreEqual(3, requiredProgress);
        Assert.AreEqual(1, q.CurrentProgress);

    }
}
