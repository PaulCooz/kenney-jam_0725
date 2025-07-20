using System;
using Newtonsoft.Json;
using UnityEngine;

namespace JamSpace
{
    public sealed class PlayerState : MonoBehaviour
    {
        private static readonly int Die = Animator.StringToHash("Die");
        private static readonly int Hit = Animator.StringToHash("Hit");

        [SerializeField]
        public CharacterController characterController;
        [SerializeField]
        public Animator animator;
        [SerializeField]
        private int maxHealth = 5;

        public Values stateValues
        {
            get
            {
                var json = PlayerPrefs.GetString("StateValues");
                return string.IsNullOrEmpty(json) ? Values.Default : JsonConvert.DeserializeObject<Values>(json);
            }
            set => PlayerPrefs.SetString("StateValues", JsonConvert.SerializeObject(value));
        }

        public Vector3 movement { get; set; }

        private int _health;
        public int health
        {
            get => _health;
            set
            {
                value = Mathf.Max(value, 0);

                if (value is 0)
                    animator.SetTrigger(Die);
                else if (_health > value)
                    animator.SetTrigger(Hit);

                _health = value;
            }
        }

        private void Awake()
        {
            _health = maxHealth;
            
        }

        [Serializable]
        public struct Values
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