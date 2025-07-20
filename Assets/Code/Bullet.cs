using System;
using UnityEngine;

namespace JamSpace
{
    public sealed class Bullet : MonoBehaviour
    {
        [SerializeField]
        private float speed = 10;
        [SerializeField]
        private float maxLifeTime = 5f;
        public int damage { get; private set; } = 1;

        private float _lifeTime;
        private Vector3 _flyDirection;

        public void ShootTo(Vector3 forward, int additionalDamage)
        {
            _flyDirection = forward;
            damage += additionalDamage;
        }

        private void Update()
        {
            var dt = Time.deltaTime;
            _lifeTime += dt;
            if (_lifeTime > maxLifeTime)
                Destroy(gameObject);
            transform.Translate(_flyDirection * (dt * speed));
        }

        private void OnTriggerEnter(Collider other) => Destroy(gameObject);
    }
}