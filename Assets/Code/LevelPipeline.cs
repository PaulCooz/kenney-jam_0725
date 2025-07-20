using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace JamSpace
{
    public sealed class LevelPipeline : MonoBehaviour
    {
        [SerializeField]
        private Enemy[] enemies;

        [SerializeField]
        private PlayerState player;
        [SerializeField]
        private PowerChooser powerChooser;
        [SerializeField]
        private MessagePopup messagePopup;

        [SerializeField]
        private Vector3 playerStartPos;
        [SerializeField]
        private Rect enemySpawnZone;

        [SerializeField]
        private Transform doorBeginPoint, doorEndPoint;
        [SerializeField]
        private Collider doorBeginCollider, doorEndCollider;

        public int levelNumber
        {
            get => PlayerPrefs.GetInt("LevelNumber", 1);
            private set => PlayerPrefs.SetInt("LevelNumber", value);
        }

        private List<Enemy> _spawnedEnemies;

        private const float OpenDoorAngle = 100f;

        private void Awake() { Cursor.lockState = CursorLockMode.Locked; }

        private void Start()
        {
            player.enabled = false;
            player.characterController.enabled = false;
            player.characterController.transform.position = playerStartPos;

            StartLevelAsync().Forget();
        }

        private async UniTask StartLevelAsync()
        {
            await UniTask.DelayFrame(2);

            doorBeginCollider.enabled = doorEndCollider.enabled = true;
            doorBeginPoint.localRotation = doorEndPoint.localRotation = Quaternion.identity;

            player.enabled = true;
            player.characterController.enabled = true;

            var ln = levelNumber;
            var enemiesState = new Enemy.Values
            {
                moveSpeedScale = 1f + (ln - 1) / 5f,
                meleeDamageAdd = ln - 1,
                meleeIntervalScale = 1f - (ln - 1) / 10f,
                healthAdd = ln / 2,
            };
            _spawnedEnemies = new();
            for (var i = 0; i < ln; i++)
            {
                await UniTask.NextFrame();

                var enemy = Instantiate(enemies.GetRand());
                enemy.runToPlayer = false;
                enemy.player = player;
                enemy.transform.position = new(
                    Mathf.Lerp(enemySpawnZone.xMin, enemySpawnZone.xMax, Random.value),
                    0.5f,
                    Mathf.Lerp(enemySpawnZone.yMin, enemySpawnZone.yMax, Random.value)
                );
                enemy.Setup(enemiesState);
                enemy.OnDie += OnEnemyDie;
                _spawnedEnemies.Add(enemy);
            }

            await UniTask.WaitForSeconds(1f);

            foreach (var e in _spawnedEnemies)
                e.runToPlayer = true;

            await doorBeginPoint
                .DOLocalRotate(new(0, OpenDoorAngle, 0), 0.5f)
                .SetEase(Ease.OutBack);

            doorBeginCollider.enabled = false;
        }

        private void OnEnemyDie(Enemy enemy)
        {
            _spawnedEnemies.Remove(enemy);
            if (_spawnedEnemies.Count is 0)
                OpenExit();
        }

        private void OpenExit()
        {
            DOTween.Sequence()
                .Append(doorEndPoint
                    .DOLocalRotate(new(0, OpenDoorAngle, 0), 0.5f)
                    .SetEase(Ease.OutBack)
                )
                .AppendCallback(() => doorEndCollider.enabled = false);
        }

        public async UniTask FinishAsync(bool isWin = true)
        {
            player.enabled = false;
            player.characterController.enabled = false;

            Cursor.lockState = CursorLockMode.None;
            if (isWin)
            {
                levelNumber++;

                await powerChooser.ChooseAsync();
            }
            else
            {
                foreach (var e in _spawnedEnemies)
                    Destroy(e.gameObject);
                await messagePopup.Push("you died", "restart");
            }
            Cursor.lockState = CursorLockMode.Locked;

            var checkSet = 0;
            while (checkSet < 3)
            {
                player.characterController.transform.position = playerStartPos;
                await UniTask.NextFrame();

                checkSet += Vector3.Distance(player.characterController.transform.position, playerStartPos) < 0.1f
                    ? +1
                    : -checkSet;
            }

            player.characterController.enabled = true;
            player.enabled = true;

            player.Awake();

            StartLevelAsync().Forget();
        }
    }
}