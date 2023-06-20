using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using MessagePipe;
using Sample.QuestMediator;
using VContainer;
using VContainer.Unity;

namespace Sample.QuestMediator
{
    //this example demonstrates how to use the mediator pattern to decouple the quest-related logic from other game systems.
    
    public enum QuestStatus
    {
        NotStarted,
        InProgress,
        Completed,
        Claimed
    }

    public class Quest
    {
        public int QuestId { get; set; }
        public string QuestName { get; set; }
        public List<Objective> Objectives { get; set; }

        public QuestStatus Status { get; set; }
        // Other quest properties
    }

    public class Objective
    {
        public int ObjectiveId { get; set; }
        public string ObjectiveDescription { get; set; }

        public bool IsCompleted { get; set; }
        // Other objective properties
    }

    public struct StartQuest
    {
        public int QuestId { get; set; }
    }

    public struct ObjectiveCompleted
    {
        public int QuestId { get; set; }
        public int ObjectiveId { get; set; }
    }

    public struct ClaimQuestReward
    {
        public int QuestId { get; set; }
    }

    public interface IQuestRepository
    {
        Quest GetQuest(int questId);
        void UpdateQuest(Quest quest);
    }

    public class QuestRepository : IQuestRepository
    {
        private readonly Dictionary<int, Quest> _quests = new Dictionary<int, Quest>();

        public Quest GetQuest(int questId)
        {
            return _quests[questId];
        }

        public void UpdateQuest(Quest quest)
        {
            _quests[quest.QuestId] = quest;
        }
    }

    public class StartQuestHandler : IRequestHandler<StartQuest, bool>
    {
        private readonly IQuestRepository _questRepository;

        public StartQuestHandler(IQuestRepository questRepository)
        {
            _questRepository = questRepository;
        }

        public bool Invoke(StartQuest request)
        {
            var quest = _questRepository.GetQuest(request.QuestId);
            quest.Status = QuestStatus.InProgress;
            _questRepository.UpdateQuest(quest);
            return true;
        }
    }

    public class ObjectiveCompletedHandler : IRequestHandler<ObjectiveCompleted, bool>
    {
        private readonly IQuestRepository _questRepository;

        public ObjectiveCompletedHandler(IQuestRepository questRepository)
        {
            _questRepository = questRepository;
        }

        public bool Invoke(ObjectiveCompleted request)
        {
            var quest = _questRepository.GetQuest(request.QuestId);
            Objective objective = null;
            foreach (var obj in quest.Objectives)
            {
                if (obj.ObjectiveId == request.ObjectiveId)
                {
                    objective = obj;
                    break;
                }
            }

            if (objective != null)
            {
                objective.IsCompleted = true;
                _questRepository.UpdateQuest(quest);
            }

            if (quest.Objectives.All(o => o.IsCompleted))
            {
                quest.Status = QuestStatus.Completed;
                _questRepository.UpdateQuest(quest);
            }

            return objective != null;
        }
    }

    public class ClaimQuestRewardHandler : IRequestHandler<ClaimQuestReward, bool>
    {
        private readonly IQuestRepository _questRepository;

        public ClaimQuestRewardHandler(IQuestRepository questRepository)
        {
            _questRepository = questRepository;
        }

        public bool Invoke(ClaimQuestReward request)
        {
            var quest = _questRepository.GetQuest(request.QuestId);
            quest.Status = QuestStatus.Claimed;
            _questRepository.UpdateQuest(quest);
            return true;
        }
    }
    //The request handlers can be independently used in other systems without wrapping them in QuestServices.
    public class QuestServices
    {
        private readonly IRequestHandler<StartQuest, bool> _startQuestHandler;
        private readonly IRequestHandler<ObjectiveCompleted, bool> _objectiveCompletedHandler;
        private readonly IRequestHandler<ClaimQuestReward, bool> _claimQuestRewardHandler;

        public QuestServices(IRequestHandler<StartQuest, bool> startQuestHandler,
            IRequestHandler<ObjectiveCompleted, bool> objectiveCompletedHandler,
            IRequestHandler<ClaimQuestReward, bool> claimQuestRewardHandler)
        {
            _startQuestHandler = startQuestHandler;
            _objectiveCompletedHandler = objectiveCompletedHandler;
            _claimQuestRewardHandler = claimQuestRewardHandler;
        }

        public bool StartQuest(int questId)
        {
            return _startQuestHandler.Invoke(new StartQuest {QuestId = questId});
        }

        public bool CompleteObjective(int questId, int objectiveId)
        {
            return _objectiveCompletedHandler.Invoke(new ObjectiveCompleted
                {QuestId = questId, ObjectiveId = objectiveId});
        }

        public bool ClaimReward(int questId)
        {
            return _claimQuestRewardHandler.Invoke(new ClaimQuestReward {QuestId = questId});
        }
    }

    public class QuestSomeWhere : IStartable
    {
        private readonly QuestServices _questServices;
        private readonly QuestRepository _questRepository;

        public QuestSomeWhere(QuestServices questServices, QuestRepository questRepository)
        {
            _questServices = questServices;
            _questRepository = questRepository;
        }

        public void Start()
        {
            //add some quests
            _questRepository.UpdateQuest(new Quest
            {
                QuestId = 1,
                QuestName = "Quest 1",
                Objectives = new List<Objective>
                {
                    new Objective {ObjectiveId = 1, ObjectiveDescription = "Objective 1"},
                    new Objective {ObjectiveId = 2, ObjectiveDescription = "Objective 2"},
                    new Objective {ObjectiveId = 3, ObjectiveDescription = "Objective 3"},
                }
            });

            _questRepository.UpdateQuest(new Quest
            {
                QuestId = 2,
                QuestName = "Quest 2",
                Objectives = new List<Objective>
                {
                    new Objective {ObjectiveId = 1, ObjectiveDescription = "Objective 1"},
                    new Objective {ObjectiveId = 2, ObjectiveDescription = "Objective 2"},
                    new Objective {ObjectiveId = 3, ObjectiveDescription = "Objective 3"},
                }
            });

            _questRepository.UpdateQuest(new Quest
            {
                QuestId = 3,
                QuestName = "Quest 3",
                Objectives = new List<Objective>
                {
                    new Objective {ObjectiveId = 1, ObjectiveDescription = "Objective 1"},
                    new Objective {ObjectiveId = 2, ObjectiveDescription = "Objective 2"},
                    new Objective {ObjectiveId = 3, ObjectiveDescription = "Objective 3"},
                }
            });
        }

        //simulate quest doing
        public async UniTask SimulateQuestDoing()
        {
            // Start Quest 1
            _questServices.StartQuest(1);
            await UniTask.Delay(1000);

            // Complete Objective 1 of Quest 1
            _questServices.CompleteObjective(1, 1);
            await UniTask.Delay(1000);

            // Complete Objective 2 of Quest 1
            _questServices.CompleteObjective(1, 2);
            await UniTask.Delay(1000);

            // Complete Objective 3 of Quest 1
            _questServices.CompleteObjective(1, 3);
            await UniTask.Delay(1000);

            // Claim Reward of Quest 1
            _questServices.ClaimReward(1);
            await UniTask.Delay(1000);
            
            // Start Quest 2
            _questServices.StartQuest(2);
            await UniTask.Delay(1000);
            
        }
    }


    public class QuestCompositionRoot : LifetimeScope
    {
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<IQuestRepository, QuestRepository>(Lifetime.Singleton);
            builder.Register<QuestServices>(Lifetime.Singleton);

            var messagePipeOptions = builder.RegisterMessagePipe();
            builder.RegisterRequestHandler<StartQuest, bool, StartQuestHandler>(messagePipeOptions);
            builder.RegisterRequestHandler<ObjectiveCompleted, bool, ObjectiveCompletedHandler>(messagePipeOptions);
            builder.RegisterRequestHandler<ClaimQuestReward, bool, ClaimQuestRewardHandler>(messagePipeOptions);
        }
    }
}