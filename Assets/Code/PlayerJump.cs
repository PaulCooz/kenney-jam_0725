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
        private CharacterController characterController;
        [SerializeField]
        private Animator animator;

        [SerializeField]
        private float jumpHeight = 3f;

        private Vector3 _velocity;

        private void Start() { jumpAction.Enable(); }

        private void Update()
        {
            const float groundDist = 0.2f;
            var isGrounded = Physics.Raycast(characterController.bounds.min, Vector3.down, out var hit, 10f) &&
                             hit.distance < groundDist;
            if (isGrounded && _velocity.y < 0)
                _velocity.y = -2f;

            if (jumpAction.IsPressed() && isGrounded)
            {
                animator.SetTrigger(StartJump);
                _velocity.y = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
            }

            _velocity.y += Physics.gravity.y * Time.deltaTime;

            // can go underground :/
            if (_velocity.y < 0f && -_velocity.y * Time.deltaTime > hit.distance)
                _velocity.y = -hit.distance;

            characterController.Move(_velocity * Time.deltaTime);

            animator.SetBool(Grounded, isGrounded);
        }
    }
}