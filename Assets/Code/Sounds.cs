using System;
using System.Collections.Generic;
using UnityEngine;

namespace JamSpace
{
    public sealed class Sounds : MonoBehaviour
    {
        [SerializeField]
        private AudioSource musicSource;

        [SerializeField]
        private AudioClip[] musicClips;

        [SerializeField]
        private AudioSource doorBeginSource, doorEndSource;

        [SerializeField]
        private PlayerState player;
        [SerializeField]
        private AudioSource shootSource;
        [SerializeField]
        private AudioSource footstepSource;

        [SerializeField]
        private AudioSource uiSource;
        [SerializeField]
        private List<AudioClip> uiClips;

        public void OnLevelStart()
        {
            musicSource.clip = musicClips.GetRand();
            musicSource.Play();
            footstepSource.Play();
        }

        public void OnDoorOpen(bool isBegin)
        {
            if (isBegin)
                doorBeginSource.Play();
            else
                doorEndSource.Play();
        }

        public void OnShoot() { shootSource.Play(); }

        public void PlayUI(string clip)
        {
            uiSource.clip = uiClips.Find(c => c.name == clip);
            uiSource.Play();
        }

        private void Update()
        {
            footstepSource.volume = Mathf.Clamp(
                player.characterController.isGrounded ? player.movement.WithY(0f).sqrMagnitude * 100f : 0f,
                0f, 0.2f
            );
        }
    }
}