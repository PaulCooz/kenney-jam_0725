using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JamSpace
{
    public sealed class PlayerShooting : MonoBehaviour
    {
        private static readonly int Shoot = Animator.StringToHash("Shoot");

        [SerializeField]
        private InputAction shootAction;
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private float cooldown = 2;

        private float _lastShootTime;

        private void Start() { shootAction.Enable(); }

        private void Update()
        {
            var time = Time.unscaledTime;
            if ((time - _lastShootTime) >= cooldown && shootAction.IsPressed())
            {
                animator.SetTrigger(Shoot);
                _lastShootTime = time;
            }
        }
    }
}