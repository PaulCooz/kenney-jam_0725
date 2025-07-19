using System;
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

        private void Awake() { _health = maxHealth; }
    }
}