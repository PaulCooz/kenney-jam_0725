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
        private PlayerState player;

        private void Start() { moveAction.Enable(); }

        private void FixedUpdate() { transform.position = player.characterController.transform.position; }

        private void Update()
        {
            var input = Vector3.zero;
            if (moveAction.IsPressed())
            {
                var v = moveAction.ReadValue<Vector2>();
                input = transform.right * v.x + transform.forward * v.y;
                input = (speed * Time.deltaTime) * input.WithY(0).normalized;
            }

            player.movement += input;
            player.animator.SetBool(Running, player.movement.WithY(0f).sqrMagnitude > 0.00001f);

            player.characterController.Move(player.movement);
            player.movement *= Mathf.Clamp01(1f - 10f * Time.deltaTime);

            transform.position = player.characterController.transform.position;
        }
    }
}