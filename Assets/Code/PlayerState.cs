using System;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace JamSpace
{
    public sealed class PlayerState : MonoBehaviour
    {
        private static readonly int Die = Animator.StringToHash("Die");
        private static readonly int Hit = Animator.StringToHash("Hit");

        [SerializeField]
        private LevelPipeline levelPipeline;
        [SerializeField]
        public CharacterController characterController;
        [SerializeField]
        public Animator animator;
        [SerializeField]
        public int maxHealth = 5;

        private static Values stateValuesPref
        {
            get
            {
                var json = PlayerPrefs.GetString("StateValues");
                return string.IsNullOrEmpty(json) ? Values.Default : JsonConvert.DeserializeObject<Values>(json);
            }
            set => PlayerPrefs.SetString("StateValues", JsonConvert.SerializeObject(value));
        }

        public Values stateValues;

        public Vector3 movement { get; set; }

        private int _health;
        public int health
        {
            get => _health;
            set
            {
                if (_health is 0)
                    return;

                value = Mathf.Max(value, 0);

                if (value is 0)
                {
                    levelPipeline.FinishAsync(false).Forget();
                    animator.SetTrigger(Die);
                }
                else if (_health > value)
                {
                    animator.SetTrigger(Hit);
                }

                _health = value;
            }
        }

        public void SaveValues() => stateValuesPref = stateValues;

        public void Awake()
        {
            _health = maxHealth;
            stateValues = stateValuesPref;
            animator.Play("Idle");
        }

        [Serializable]
        public class Values
        {
            public static readonly Values Default = new()
            {
                moveSpeedScale = 1f,
                jumpGravityScale = 1f,
                jumpForceScale = 1f,

                shootDamageAdd = 0,
                shootIntervalScale = 1f,
                shootBulletSizeScale = 1f,
                shootAimEnable = false,
            };

            public float moveSpeedScale;
            public float jumpGravityScale;
            public float jumpForceScale;

            public int shootDamageAdd;
            public float shootIntervalScale;
            public float shootBulletSizeScale;
            public bool shootAimEnable;
        }
    }
}