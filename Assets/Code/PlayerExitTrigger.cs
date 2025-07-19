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
                FinishAsync().Forget();
        }

        private async UniTask FinishAsync()
        {
            player.enabled = false;
            player.characterController.enabled = false;

            await UniTask.NextFrame();

            var currPos = player.characterController.transform.position;
            var pos = currPos.WithX(-currPos.x).WithZ(-currPos.z) + offset;
            var equalsCount = 0;
            // strange problems require strange solutions  ☝️🤓
            while (equalsCount < 3)
            {
                player.characterController.transform.position = pos;
                await UniTask.NextFrame();

                equalsCount += Vector3.Distance(player.characterController.transform.position, pos) < 0.1f
                    ? +1
                    : -equalsCount;
            }

            player.characterController.enabled = true;
            player.enabled = true;

            levelPipeline.Restart();
        }
    }
}