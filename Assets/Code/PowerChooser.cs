using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JamSpace
{
    public sealed class PowerChooser : MonoBehaviour
    {
        [SerializeField]
        private InputAction getLeft, getRight;
        [SerializeField]
        private CanvasGroup group;

        private UniTaskCompletionSource _chooseTcs;

        private void Awake()
        {
            group.alpha = 0f;
            group.blocksRaycasts = false;
        }

        private void Start()
        {
            getLeft.Enable();
            getRight.Enable();
        }

        private void Update()
        {
            if (group.blocksRaycasts)
            {
                if (getLeft.WasPressedThisFrame())
                    ClickChoose(false);
                else if (getRight.WasReleasedThisFrame())
                    ClickChoose(true);
            }
        }

        public void ClickChoose(bool isRight)
        {
            Debug.Log($"choose {isRight}");
            _chooseTcs.TrySetResult();
        }

        public async UniTask ChooseAsync()
        {
            _chooseTcs = new();

            await group
                .DOFade(1f, 0.5f)
                .OnComplete(() => group.blocksRaycasts = true);

            await _chooseTcs.Task;

            group.blocksRaycasts = false;
            await group.DOFade(0f, 0.5f);

            _chooseTcs = null;
        }
    }
}