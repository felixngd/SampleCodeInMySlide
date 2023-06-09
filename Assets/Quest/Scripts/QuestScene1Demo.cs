using System;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace SampleCode.Quest
{
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

                AnimatorController runtimeAnimatorController = new AnimatorController();
                runtimeAnimatorController.name = "RuntimeAnimatorController";

                AnimatorState newState = new AnimatorState();
                newState.name = "NewState";
                runtimeAnimatorController.AddEffectiveStateMachineBehaviour(typeof(AnimatorState), newState, 0);

                AnimationClip newAnimation = new AnimationClip();
                newState.motion = newAnimation;
                
                //save the controller as an asset
                AssetDatabase.CreateAsset(runtimeAnimatorController, "Assets/AnimatorController.controller");
            }
        }
    }
}