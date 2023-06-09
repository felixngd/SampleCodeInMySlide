using System.Collections.Generic;
using UnityEngine;
/*
    This is a sample code for the Skill system. It is not a complete implementation.
    This sample covers the execution of skills and the implementation of skill commands.
*/
namespace SampleCode.Skill
{
    /*
     MASTER DATA
     */
    public abstract class ASkillData : ScriptableObject
    {
        public int id;
    }

    public class FireballSkillData : ASkillData
    {
        public float damage;
        public float radius;
        public float speed;
        public GameObject vfxPrefab;
        public AudioClip soundClip;
    }

    public class HealSkillData : ASkillData
    {
        public float healAmount;
        public float radius;
        public float speed;

        public GameObject vfxPrefab;
        public AudioClip soundClip;
    }

    /*
     RUNTIME DATA
     */
    public class RuntimeSkillData
    {
        public GameObject executor;
        public List<GameObject> targets;
    }

    public interface ISkill
    {
        void Execute(RuntimeSkillData runtimeData);
    }

    public class FireballSkill : ISkill
    {
        private FireballSkillData _data;

        public FireballSkill(FireballSkillData data)
        {
            _data = data;
        }

        public void Execute(RuntimeSkillData runtimeData)
        {
            // Implement the fireball skill logic here
            PlayVFX(runtimeData.executor);
            PlaySound(runtimeData.executor);
        }

        private void PlayVFX(GameObject executor)
        {
            // Instantiate the VFX prefab at the executor's position
            GameObject vfx = GameObject.Instantiate(_data.vfxPrefab, executor.transform.position, Quaternion.identity);
        }

        private void PlaySound(GameObject executor)
        {
            // Play the sound clip at the executor's position
            AudioSource.PlayClipAtPoint(_data.soundClip, executor.transform.position);
        }
    }

    public class HealSkill : ISkill
    {
        private HealSkillData _data;

        public HealSkill(HealSkillData data)
        {
            _data = data;
        }

        public void Execute(RuntimeSkillData runtimeData)
        {
            // Implement the heal skill logic here
            PlayVFX(runtimeData.executor);
            PlaySound(runtimeData.executor);
        }

        private void PlayVFX(GameObject executor)
        {
            GameObject vfx = GameObject.Instantiate(_data.vfxPrefab, executor.transform.position, Quaternion.identity);
        }

        private void PlaySound(GameObject executor)
        {
            AudioSource.PlayClipAtPoint(_data.soundClip, executor.transform.position);
        }
    }

    public interface ISkillCommand
    {
        void Execute();
        bool Validate();
    }

    public class SkillCommand : ISkillCommand
    {
        public ISkill Skill { get; private set; }
        public RuntimeSkillData RuntimeData { get; private set; }
        public string Debug { get; private set; }

        public SkillCommand(ISkill skill, RuntimeSkillData runtimeData, string debug = "")
        {
            Skill = skill;
            RuntimeData = runtimeData;
            Debug = debug;
        }

        public void Execute()
        {
            if (Validate())
            {
                Skill.Execute(RuntimeData);

                if (!string.IsNullOrEmpty(Debug))
                {
                    // Implement debug logic here
                }
            }
        }

        public bool Validate()
        {
            // Implement validation logic here
            return true;
        }
    }

    public class SkillCommandQueue
    {
        private Queue<ISkillCommand> _queue;

        public SkillCommandQueue()
        {
            _queue = new Queue<ISkillCommand>();
        }

        public void Enqueue(ISkillCommand command)
        {
            _queue.Enqueue(command);
        }

        public void ExecuteNext()
        {
            if (_queue.Count > 0)
            {
                ISkillCommand command = _queue.Dequeue();
                command.Execute();
            }
        }
    }


    /*
     SKILL EXECUTOR
     */
    public class Dummy : MonoBehaviour
    {
        private SkillCommandQueue _skillCommandQueue;
        private FireballSkillData _fireballSkillData;
        private HealSkillData _healSkillData;
        
        //The skills this executor can use
        private List<ISkill> _skills = new List<ISkill>();
        private void Start()
        {
            _skillCommandQueue = new SkillCommandQueue();

            //Configure the skills this executor can use
            FireballSkill fireballSkill = new FireballSkill(_fireballSkillData);
            HealSkill healSkill = new HealSkill(_healSkillData);
            _skills.Add(fireballSkill);
            _skills.Add(healSkill);
        }

        public void ExecuteSkill(List<GameObject> targets)
        {
            RuntimeSkillData runtimeData = new RuntimeSkillData
            {
                executor = this.gameObject,
                targets = targets
            }; 
            //randomly select a skill from the list of skills
            ISkill skill = _skills[Random.Range(0, _skills.Count)];

            SkillCommand command = new SkillCommand(skill, runtimeData);
            _skillCommandQueue.Enqueue(command);
            _skillCommandQueue.ExecuteNext();

            // You can also enqueue multiple commands here
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ExecuteSkill(new List<GameObject>());
            }
        }
    }
}
//Pure Dependency Injection