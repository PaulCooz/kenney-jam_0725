using System;
using UnityEngine;

namespace JamSpace
{
    public sealed class PlayerExitTrigger : MonoBehaviour
    {
        [SerializeField]
        private CharacterController characterController;
        [SerializeField]
        private Vector3 offset;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Finish"))
            {
                characterController.enabled = false;
                var pos = characterController.transform.position;
                characterController.transform.position = pos.WithX(-pos.x).WithZ(-pos.z) + offset;
                characterController.enabled = true;
            }
        }
    }
}