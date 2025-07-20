using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace JamSpace
{
    public sealed class MessagePopup : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup group;
        [SerializeField]
        private TMP_Text titleTMP, okTMP;

        private UniTaskCompletionSource _tcs;

        private void Awake()
        {
            group.alpha = 0f;
            group.blocksRaycasts = false;
        }

        public void Ok() => _tcs.TrySetResult();

        public async UniTask Push(string title, string ok)
        {
            titleTMP.text = title;
            okTMP.text = ok;
            _tcs = new();

            await group
                .DOFade(1f, 0.5f)
                .OnComplete(() => group.blocksRaycasts = true);

            await _tcs.Task;

            group.blocksRaycasts = false;
            await group.DOFade(0f, 0.5f);
        }
    }
}