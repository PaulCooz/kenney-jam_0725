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
        private PlayerState player;
        [SerializeField]
        private Bullet bullet;
        [SerializeField]
        private Transform origin;
        [SerializeField]
        private float cooldown = 2;

        private float _lastShootTime;

        private void Start() { shootAction.Enable(); }

        private void Update()
        {
            if (!player.isActiveAndEnabled)
                return;

            var time = Time.unscaledTime;
            if ((time - _lastShootTime) >= cooldown && shootAction.IsPressed())
            {
                player.animator.SetTrigger(Shoot);
                var b = Instantiate(bullet);
                b.transform.position = origin.position;
                b.ShootTo(transform.forward);
                _lastShootTime = time;
            }
        }
    }
}