using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace JamSpace
{
    public sealed class PlayerExitTrigger : MonoBehaviour
    {
        [SerializeField]
        private PlayerState player;
        [SerializeField]
        private LevelPipeline levelPipeline;
        [SerializeField]
        private Vector3 offset;

        private void OnTriggerEnter(Collider other)
        {
            if (!player.isActiveAndEnabled)
                return;

            if (other.CompareTag("Finish"))
                levelPipeline.FinishAsync(offset).Forget();
        }
    }
}