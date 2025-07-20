using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace JamSpace
{
    public sealed class PowerChooser : MonoBehaviour
    {
        [SerializeField]
        private InputAction getLeft, getRight;
        [SerializeField]
        private CanvasGroup group;
        [SerializeField]
        private TMP_Text leftTMP, rightTMP;
        [SerializeField]
        private PlayerState player;

        private PowerUps.PowerUp _leftPU, _rightPU;

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
            if (isRight)
                _rightPU.Use(player);
            else
                _leftPU.Use(player);
            _chooseTcs.TrySetResult();
        }

        public async UniTask ChooseAsync()
        {
            _chooseTcs = new();

            await UniTask.NextFrame();

            var powerUps = PowerUps.All.Where(pu => pu.CanChoose(player)).ToArray();
            var sum = powerUps.Sum(pu => pu.Prob);
            var rand = Random.Range(0, sum + 1);
            foreach (var pu in powerUps)
            {
                rand -= pu.Prob;
                if (rand <= 0)
                {
                    leftTMP.text = pu.GetToChoose();
                    _leftPU = pu;
                    break;
                }
            }

            powerUps = powerUps.Where(pu => pu.Name != _leftPU.Name).ToArray();
            sum = powerUps.Sum(pu => pu.Prob);
            rand = Random.Range(0, sum + 1);
            foreach (var pu in powerUps)
            {
                rand -= pu.Prob;
                if (rand <= 0)
                {
                    rightTMP.text = pu.GetToChoose();
                    _rightPU = pu;
                    break;
                }
            }

            await UniTask.NextFrame();

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