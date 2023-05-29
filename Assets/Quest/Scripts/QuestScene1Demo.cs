using System;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class QuestScene1Demo : MonoBehaviour
{
    public Button button;
    
    [Inject] public QuestEventPublisher _questEventPublisher;
    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            var q = new ProgressiveDailyQuest("01", "02", 3, _questEventPublisher);
            q.IncrementProgress();
            q.Complete();
        });
    }

    private void OnGUI()
    {
        //load scene Scene_2_Quest
        if (GUILayout.Button("Load Scene_2_Quest"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Scene_2_Quest");
        }
    }
}