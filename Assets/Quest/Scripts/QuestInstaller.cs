using System;
using MessagePipe;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;
using VContainer.Unity;

namespace SampleCode.Quest
{
    public class QuestInstaller : LifetimeScope
    {
        [FormerlySerializedAs("gameSetting")] public QuestConfiguration questConfiguration;
        protected override void Configure(IContainerBuilder builder)
        {
            // var options = builder.RegisterMessagePipe();
            // builder.RegisterMessageBroker<ProgressiveQuestTopic>(options);
            //
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
            DontDestroyOnLoad(this);
        }
    }
}