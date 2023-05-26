using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class DailyQuestUI : MonoBehaviour
{
    public GameObject questPrefab;
    public Transform questContainer;
    [Inject]
    private DailyQuestManager _dailyQuestManager;

    private void Start()
    {
        // Add some quests for demonstration purposes
        _dailyQuestManager.AddQuest(new DailyQuest("Quest 1", "Complete 5 tasks"));
        _dailyQuestManager.AddQuest(new TimedDailyQuest("Quest 2", "Collect 10 items", DateTime.Now.AddHours(2)));

        // Display the quests in the UI
        DisplayQuests();
    }

    private void DisplayQuests()
    {
        // Clear any existing quests in the UI
        foreach (Transform child in questContainer)
        {
            Destroy(child.gameObject);
        }

        // Get all quests from the DailyQuestManager
        IEnumerable<IDailyQuest> quests = _dailyQuestManager.GetAllQuests();

        // Instantiate and display each quest in the UI
        foreach (IDailyQuest quest in quests)
        {
            GameObject questInstance = Instantiate(questPrefab, questContainer);
            questInstance.transform.Find("QuestName").GetComponent<Text>().text = quest.Name;
            questInstance.transform.Find("QuestDescription").GetComponent<Text>().text = quest.Description;
        }
    }
}