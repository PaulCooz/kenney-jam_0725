using UnityEngine;
using UnityEngine.InputSystem;

namespace JamSpace
{
    public sealed class PlayerCamera : MonoBehaviour
    {
        [SerializeField]
        private InputAction lookXAction;
        [SerializeField]
        private InputAction lookYAction;

        [SerializeField]
        private float lookSpeedX = 100f, lookSpeedY = 10f;

        [SerializeField]
        private Vector2 lookAngleX = new(-20, 70);
        [SerializeField]
        private float distance = 5, lookDist = 5;

        [SerializeField]
        private Camera mainCamera;
        [SerializeField]
        private Transform camAim;
        private float _camAngle;

        [SerializeField]
        private CharacterController characterController;

        private void Start()
        {
            lookXAction.Enable();
            lookYAction.Enable();
        }

        private void FixedUpdate() => UpdatePos();

        private void Update()
        {
            var lookX = lookXAction.WasPerformedThisFrame() ? lookXAction.ReadValue<float>() : 0f;
            var deltaTime = lookSpeedX * Time.deltaTime * lookX;
            transform.Rotate(transform.up, deltaTime);

            var lookY = lookYAction.WasPerformedThisFrame() ? lookYAction.ReadValue<float>() : 0f;
            _camAngle -= lookSpeedY * Time.deltaTime * lookY;
            _camAngle = Mathf.Clamp(_camAngle, lookAngleX.x, lookAngleX.y);
            UpdatePos();
        }

        private void UpdatePos()
        {
            mainCamera.transform.position =
                camAim.position -
                camAim.forward * (distance * Mathf.Cos(_camAngle * Mathf.Deg2Rad)) +
                camAim.up * (distance * Mathf.Sin(_camAngle * Mathf.Deg2Rad));
            mainCamera.transform.LookAt(camAim.transform.position + lookDist * camAim.forward);
        }
    }
}