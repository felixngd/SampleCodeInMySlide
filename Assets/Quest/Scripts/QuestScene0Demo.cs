using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace SampleCode.Quest
{
    public class QuestScene0Demo : MonoBehaviour
    {
        public Button button;
        [Inject] public QuestEventPublisher _questEventPublisher;
        [Inject] public DailyQuestManager _dailyQuestManager;

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
            //load scene Scene_1_Quest
            if (GUILayout.Button("Load Scene_1_Quest"))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Scene_1_Quest");
            }
        }
    }
}
