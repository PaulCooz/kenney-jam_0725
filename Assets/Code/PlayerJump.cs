using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JamSpace
{
    public sealed class PlayerJump : MonoBehaviour
    {
        private static readonly int StartJump = Animator.StringToHash("StartJump");
        private static readonly int Grounded = Animator.StringToHash("Grounded");

        [SerializeField]
        private InputAction jumpAction;
        [SerializeField]
        private PlayerState player;

        [SerializeField]
        private float jumpHeight = 3f;
        [SerializeField]
        private float gravityScale = 0.1f;

        [SerializeField]
        private float cooldown = 1f;
        private float _lastJumpTime;

        private Vector3 _velocity;

        private void Start() { jumpAction.Enable(); }

        private void Update()
        {
            const float groundDist = 0.2f;
            var isGrounded = Physics.Raycast(player.characterController.bounds.min, Vector3.down, out var hit, 10f) &&
                             hit.distance < groundDist;
            if (isGrounded && _velocity.y < 0)
                _velocity.y = -2f;

            var gravity = Physics.gravity.y * gravityScale;
            var time = Time.time;
            if (jumpAction.IsPressed() && isGrounded && time - _lastJumpTime > cooldown)
            {
                _lastJumpTime = time;
                player.animator.SetTrigger(StartJump);
                _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            _velocity.y += gravity * Time.deltaTime;

            // can go underground :/
            if (_velocity.y < 0f && -_velocity.y * Time.deltaTime > hit.distance)
                _velocity.y = -hit.distance;

            player.movement += _velocity * Time.deltaTime;

            player.animator.SetBool(Grounded, isGrounded);
        }
    }
}