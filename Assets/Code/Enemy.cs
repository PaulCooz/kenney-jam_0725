using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace JamSpace
{
    public sealed class Enemy : MonoBehaviour
    {
        private static readonly int Running = Animator.StringToHash("Running");
        private static readonly int MeleeAttack1 = Animator.StringToHash("MeleeAttack1");
        private static readonly int MeleeAttack2 = Animator.StringToHash("MeleeAttack2");
        private static readonly int Die = Animator.StringToHash("Die");
        private static readonly int Hit = Animator.StringToHash("Hit");

        [SerializeField]
        private float attackRange = 1, attackDelay = 1f;
        [SerializeField]
        private int damage = 1;
        [SerializeField]
        private float damageDelay = 0.5f;
        [SerializeField]
        private float speed = 10;
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private CharacterController characterController;
        [SerializeField]
        private GameObject[] weapons;

        [SerializeField]
        private int maxHealth = 5;
        private int _health;

        [SerializeField]
        public PlayerState player;

        private bool _attacking;

        public bool runToPlayer;

        [SerializeField]
        private Values values;

        public event Action<Enemy> OnDie;

        public void Setup(Values v)
        {
            values = v;

            var weapon = Random.Range(0, weapons.Length);
            for (var i = 0; i < weapons.Length; i++)
                weapons[i].SetActive(weapon == i);

            _health = maxHealth + values.healthAdd;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Bullet>(out var bullet))
                TakeDamage(bullet.damage);
        }

        private void TakeDamage(int d)
        {
            if (_health is 0)
                return;

            _health -= d;
            _health = Mathf.Max(_health, 0);
            animator.SetTrigger(_health is 0 ? Die : Hit);

            if (_health is 0)
            {
                enabled = false;
                DOTween.Sequence()
                    .AppendInterval(1.5f)
                    .Append(transform.DOScale(0, 0.4f).SetEase(Ease.InBack))
                    .AppendCallback(() =>
                    {
                        OnDie?.Invoke(this);
                        OnDie = null;
                        Destroy(gameObject);
                    }).WithCancellation(this.GetCancellationTokenOnDestroy());
            }
        }

        private void Update()
        {
            if (_health is 0)
                return;

            var pos = characterController.transform.position;
            var playerPos = player.characterController.transform.position;

            var direction = Time.deltaTime * speed * values.moveSpeedScale * (playerPos - pos).WithY(0).normalized;
            transform.rotation = Quaternion.LookRotation(direction);

            if (_attacking || !runToPlayer)
                return;

            if (Vector3.Distance(pos, playerPos) <= attackRange)
            {
                _attacking = true;
                animator.SetBool(Running, false);
                animator.SetTrigger(Time.frameCount % 2 is 0 ? MeleeAttack1 : MeleeAttack2);
                UniTask.WaitForSeconds(damageDelay)
                    .AttachExternalCancellation(this.GetCancellationTokenOnDestroy())
                    .ContinueWith(() =>
                    {
                        var nPos = characterController.transform.position;
                        var nPlayerPos = player.characterController.transform.position;
                        if (Vector3.Distance(nPos, nPlayerPos) <= attackRange)
                            player.health -= damage + values.meleeDamageAdd;
                    }).Forget();
                UniTask.WaitForSeconds(attackDelay * values.meleeIntervalScale)
                    .AttachExternalCancellation(this.GetCancellationTokenOnDestroy())
                    .ContinueWith(() => _attacking = false).Forget();
            }
            else
            {
                direction += Time.deltaTime * Physics.gravity;
                animator.SetBool(Running, direction.WithY(0f).sqrMagnitude > 0.00001f);
                characterController.Move(direction);
            }
        }

        [Serializable]
        public struct Values
        {
            public int healthAdd;
            public float moveSpeedScale;
            public int meleeDamageAdd;
            public float meleeIntervalScale;
        }
    }
}