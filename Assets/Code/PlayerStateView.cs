using System;
using TMPro;
using UnityEngine;

namespace JamSpace
{
    public sealed class PlayerStateView : MonoBehaviour
    {
        [SerializeField]
        private LevelPipeline pipeline;
        [SerializeField]
        private PlayerState player;

        [SerializeField]
        private TMP_Text levelTMP, healthTMP;

        private void Update()
        {
            if (Time.frameCount % 20 == 0)
            {
                levelTMP.text = $"level: {pipeline.levelNumber}";
                healthTMP.text = $"health: {player.health}/{player.maxHealth}";
            }
        }
    }
}