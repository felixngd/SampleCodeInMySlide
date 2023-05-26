using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class QuestTriggerDemo : MonoBehaviour
{
    public Button button;
    
    [Inject] private QuestEventPublisher _questEventPublisher;
    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            var q = new ProgressiveDailyQuest("01", "02", 1, _questEventPublisher);
            q.IncrementProgress();
            q.Complete();
        });
    }
}