using System;
using System.Collections.Generic;
using MessagePipe;
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
        
        builder.Register<IDailyQuestRepository, DailyQuestRepository>(Lifetime.Singleton);
        builder.Register<DailyQuestManager>(Lifetime.Singleton);
        builder.Register<QuestEventPublisher>(Lifetime.Singleton);
        
        builder.RegisterInstance<QuestConfiguration>(questConfiguration);
        
        builder.RegisterEntryPoint<QuestConfigurationLoader>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<DailyQuestUI>(Lifetime.Singleton).AsSelf();
    }
    
    
}