using MessagePipe;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;
using VContainer.Unity;

public class QuestInstaller : LifetimeScope
{
    [FormerlySerializedAs("gameSetting")] public QuestConfiguration questConfiguration;
    protected override void Configure(IContainerBuilder builder)
    {
        var options = builder.RegisterMessagePipe();
        builder.RegisterMessageBroker<ProgressiveQuestTopic>(options);
        //
        builder.Register<IDailyQuestRepository, DailyQuestRepository>(Lifetime.Singleton);
        builder.Register<DailyQuestManager>(Lifetime.Singleton);
        builder.Register<QuestEventPublisher>(Lifetime.Singleton);
        

        builder.RegisterInstance<QuestConfiguration>(questConfiguration);
        
        builder.RegisterEntryPoint<QuestConfigurationLoader>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<DailyQuestUI>(Lifetime.Singleton).AsSelf();
    }

    private void Start()
    {
        Debug.Log("QuestInstaller Start in scene: " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        Debug.Log("QuestInstaller Start in scene: " + questConfiguration.localQuestData[0].name + "requiredProgress: " + questConfiguration.localQuestData[0].requiredProgress);
        questConfiguration.localQuestData[0].requiredProgress = UnityEngine.Random.Range(1, 10);
    }
}