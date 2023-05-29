using UnityEngine;
using VContainer;

public class QuestConfigurationLoader : MonoBehaviour
{
    [Inject] public QuestConfiguration _questConfiguration;
    [Inject] public DailyQuestManager _dailyQuestManager;
    [Inject] public QuestEventPublisher _questEventPublisher;

    private void Start()
    {
        if (_questConfiguration.isRemote)
        {
            LoadRemoteQuests();
        }
        else
        {
            LoadLocalQuests();
        }
    }

    private void LoadLocalQuests()
    {
        foreach (var questData in _questConfiguration.localQuestData)
        {
            IDailyQuest quest = null;

            switch (questData.type)
            {
                case "DailyQuest":
                    quest = new DailyQuest(questData.name, questData.description);
                    break;
                case "TimedDailyQuest":
                    quest = new TimedDailyQuest(questData.name, questData.description, questData.expiration);
                    break;
                case "RewardDailyQuest":
                    quest = new RewardDailyQuest(questData.name, questData.description, questData.reward);
                    break;
                case "ProgressiveDailyQuest":
                    quest = new ProgressiveDailyQuest(questData.name, questData.description, questData.requiredProgress,
                        _questEventPublisher);
                    break;
                case "ProgressiveTimedRewardDailyQuest":
                    quest = new ProgressiveTimedRewardDailyQuest(questData.name, questData.description,
                        questData.expiration, questData.requiredProgress, questData.reward);
                    break;
            }

            if (quest != null)
            {
                _dailyQuestManager.AddQuest(quest);
            }
        }
    }

    private void LoadRemoteQuests()
    {
        // Implement remote quest loading logic here
    }
}