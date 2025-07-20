using System;
using UnityEngine;

namespace JamSpace
{
    public sealed class PlayerShootAim : MonoBehaviour
    {
        [SerializeField]
        private PlayerState player;
        [SerializeField]
        private Transform origin;
        [SerializeField]
        private Transform aim;

        private void Update()
        {
            aim.gameObject.SetActive(player.stateValues.shootAimEnable);
            if (player.stateValues.shootAimEnable)
            {
                var forward = player.transform.forward;
                if (Physics.Raycast(origin.position, forward, out var hit, 100f))
                {
                    aim.transform.position = hit.point;
                    aim.transform.LookAt(hit.point + forward);
                }
            }
        }
    }
}