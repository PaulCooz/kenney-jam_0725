using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace JamSpace
{
    public sealed class SettingsPopup : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup group;
        [SerializeField]
        private InputAction showAction;

        [SerializeField]
        private TMP_Text playerTMP;
        [SerializeField]
        private PlayerState player;

        [SerializeField]
        private Slider camX, camY;
        [SerializeField]
        private Toggle soundToggle;

        public static bool EnabledSounds
        {
            get => PlayerPrefs.GetInt("EnabledSounds", 1) == 1;
            set => PlayerPrefs.SetInt("EnabledSounds", value ? 1 : 0);
        }

        public static float CamXSen
        {
            get => PlayerPrefs.GetFloat("CamXSen", 1f);
            set => PlayerPrefs.SetFloat("CamXSen", value);
        }
        public static float CamYSen
        {
            get => PlayerPrefs.GetFloat("CamYSen", 1f);
            set => PlayerPrefs.SetFloat("CamYSen", value);
        }

        private bool _inProgress;
        private bool _showing;

        private void Awake()
        {
            group.alpha = 0f;
            group.blocksRaycasts = false;
            showAction.Enable();

            AudioListener.volume = EnabledSounds ? 1 : 0;
            soundToggle.onValueChanged.AddListener(v =>
            {
                EnabledSounds = v;
                AudioListener.volume = EnabledSounds ? 1 : 0;
            });

            camX.onValueChanged.AddListener(v => CamXSen = v);
            camY.onValueChanged.AddListener(v => CamYSen = v);
        }

        private void Update()
        {
            if (_inProgress)
                return;

            if (showAction.WasReleasedThisFrame())
            {
                if (_showing)
                {
                    _showing = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    Time.timeScale = 1f;
                    _inProgress = true;
                    group.blocksRaycasts = false;

                    soundToggle.isOn = EnabledSounds;
                    camX.value = CamXSen;
                    camY.value = CamYSen;

                    group.DOFade(0f, 0.3f).OnComplete(() => _inProgress = false).SetUpdate(true);
                }
                else
                {
                    _showing = true;
                    Cursor.lockState = CursorLockMode.None;
                    Time.timeScale = 0.1f;
                    _inProgress = true;
                    playerTMP.text = player.stateValues.ToString();
                    group.DOFade(1f, 0.3f).OnComplete(() =>
                    {
                        group.blocksRaycasts = true;
                        _inProgress = false;
                    }).SetUpdate(true);
                }
            }
        }
    }
}