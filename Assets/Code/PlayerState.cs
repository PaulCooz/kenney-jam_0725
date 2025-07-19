using System;
using UnityEngine;

namespace JamSpace
{
    public sealed class PlayerState : MonoBehaviour
    {
        [SerializeField]
        public CharacterController characterController;
        [SerializeField]
        public Animator animator;

        public Vector3 movement { get; set; }
    }
}