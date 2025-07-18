using UnityEngine;
using UnityEngine.InputSystem;

namespace JamSpace
{
    public sealed class PlayerMove : MonoBehaviour
    {
        private static readonly int Running = Animator.StringToHash("Running");

        [SerializeField]
        private InputAction moveAction;
        [SerializeField]
        private float speed;
        [SerializeField]
        private CharacterController characterController;
        [SerializeField]
        private Animator animator;

        private void Start() { moveAction.Enable(); }

        private void FixedUpdate() { transform.position = characterController.transform.position; }

        private void Update()
        {
            var input = Vector3.zero;
            if (moveAction.IsPressed())
            {
                var v = moveAction.ReadValue<Vector2>();
                input = transform.right * v.x + transform.forward * v.y;
                input = (speed * Time.deltaTime) * input.normalized;
            }

            animator.SetBool(Running, input.sqrMagnitude > 0.1f);

            characterController.SimpleMove(input.WithY(0));
            transform.position = characterController.transform.position;
        }
    }
}