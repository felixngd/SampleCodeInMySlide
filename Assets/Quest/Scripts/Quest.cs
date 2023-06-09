using System;
using System.Collections.Generic;
using MessagePipe;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace SampleCode.Quest
{
    //Master data: 
    public interface IDailyQuest
    {
        string Name { get; }
        string Description { get; }
        void Complete();
    }

    public class DailyQuest : IDailyQuest
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        public DailyQuest(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public virtual void Complete()
        {
            // Code to complete the quest
            Debug.Log($"Quest {Name} completed!");
        }
    }

    public interface IDailyQuestRepository
    {
        void Add(IDailyQuest quest);
        void Remove(IDailyQuest quest);
        IEnumerable<IDailyQuest> GetAll();
    }

    public class DailyQuestRepository : IDailyQuestRepository
    {
        private List<IDailyQuest> _quests = new List<IDailyQuest>();

        public void Add(IDailyQuest quest)
        {
            _quests.Add(quest);
        }
        
        public void Remove(IDailyQuest quest)
        {
            _quests.Remove(quest);
        }

        public IEnumerable<IDailyQuest> GetAll()
        {
            return _quests;
        }
    }

    public class TimedDailyQuest : DailyQuest
    {
        public DateTime Expiration { get; private set; }

        public TimedDailyQuest(string name, string description, DateTime expiration)
            : base(name, description)
        {
            Expiration = expiration;
        }

        public override void Complete()
        {
            if (DateTime.Now <= Expiration)
            {
                base.Complete();
            }
            else
            {
                // Code to handle expired quest
            }
        }
    }

    public class DailyQuestManager
    {
        private IDailyQuestRepository _repository;

        public int Value { get; set; }
        
        public DailyQuestManager(IDailyQuestRepository repository)
        {
            _repository = repository;
        }

        public void AddQuest(IDailyQuest quest)
        {
            _repository.Add(quest);
        }

        public void RemoveQuest(IDailyQuest quest)
        {
            _repository.Remove(quest);
        }

        public IEnumerable<IDailyQuest> GetAllQuests()
        {
            return _repository.GetAll();
        }
    }

//when sometime I want to add a new type of quest, I can just create a new class that inherits from DailyQuest and implement the Complete method.
//for example: RewardDailyQuest
    public class RewardDailyQuest : DailyQuest
    {
        public string Reward { get; private set; }

        public RewardDailyQuest(string name, string description, string reward)
            : base(name, description)
        {
            Reward = reward;
        }

        public override void Complete()
        {
            base.Complete();
            // Code to give the player the reward
        }
    }

//A quest that both has time limit, progress and reward
    public class ProgressiveTimedRewardDailyQuest : TimedDailyQuest
    {
        public int CurrentProgress { get; private set; }
        public int RequiredProgress { get; private set; }
        public string Reward { get; private set; }

        public ProgressiveTimedRewardDailyQuest(string name, string description, DateTime expiration,
            int requiredProgress, string reward)
            : base(name, description, expiration)
        {
            RequiredProgress = requiredProgress;
            Reward = reward;
        }

        public override void Complete()
        {
            if (CurrentProgress >= RequiredProgress)
            {
                base.Complete();
                // Code to give the player the reward
            }
            else
            {
                // Code to handle incomplete quest
            }
        }
    }


/*TODO:
+ Add a module to serialize/deserialize the quest data of the player
+ Add a module to save/load the quest data of the player
+ Add VContainer to inject the quest system into the game
*/

//+ Using MessagePipe to trigger and notify the quest system


    public struct ProgressiveQuestTopic
    {
        string Name { get; }

        public ProgressiveQuestTopic(string name)
        {
            Name = name;
        }
    }

    public class QuestEventPublisher
    {
        private readonly IPublisher<ProgressiveQuestTopic> _publisher;

        public QuestEventPublisher(IPublisher<ProgressiveQuestTopic> publisher)
        {
            _publisher = publisher;
        }

        public void PublishQuestCompleted(IDailyQuest quest)
        {
            _publisher.Publish(new ProgressiveQuestTopic(quest.Name));
        }
    }


//Add another type of quest: ProgressiveDailyQuest
    public class ProgressiveDailyQuest : DailyQuest
    {
        public int CurrentProgress { get; private set; }
        public int RequiredProgress { get; private set; }

        private readonly QuestEventPublisher _eventPublisher;

        public ProgressiveDailyQuest(string name, string description, int requiredProgress,
            QuestEventPublisher questEventPublisher)
            : base(name, description)
        {
            RequiredProgress = requiredProgress;
            _eventPublisher = questEventPublisher;
        }

        public override void Complete()
        {
            if (CurrentProgress >= RequiredProgress)
            {
                base.Complete();
                _eventPublisher.PublishQuestCompleted(this);
            }
            else
            {
                // Code to handle incomplete quest
                CurrentProgress++;
                Debug.Log($"Quest {Name} progress: {CurrentProgress}/{RequiredProgress}");
            }
        }

        public void IncrementProgress()
        {
            CurrentProgress++;
        }
    }
}