//a module to handle the quest configuration

using System;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestConfiguration", menuName = "Quests/Quest Configuration", order = 1)]
public class QuestConfiguration : ScriptableObject
{
    public bool isRemote;
    public string remoteUrl;
    public LocalQuestData[] localQuestData;

    [Serializable]
    public class LocalQuestData
    {
        public string name;
        public string description;
        public string type;
        public DateTime expiration;
        public int requiredProgress;
        public string reward;
    }
}