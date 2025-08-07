using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DamageSystem
{
    public class Weapon: MonoBehaviour
    {
        [SerializeField]private Rigidbody _rigidbody;
        private float _cooldownTime = 0.5f;
        private bool _isOnCooldown;

        public void OnValidate()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (_isOnCooldown) return;
        
            var damageTarget = other.gameObject.GetComponent<ICanBeDamaged>();

            if (damageTarget != null)
            {
                StartCooldown().Forget();
                Vector3 enemyVelocity = damageTarget.GetRigidbody().velocity;
                Vector3 relativeVelocity = _rigidbody.velocity - enemyVelocity;
                float relativeSpeed = relativeVelocity.magnitude;
                Debug.Log($"Относительная скорость: {relativeSpeed}");
                var contactPoint = other.GetContact(0);
                damageTarget.TakeDamage(relativeSpeed*5f, transform, contactPoint);
            }
        }

        private async UniTask StartCooldown()
        {
            _isOnCooldown = true;
            await UniTask.Delay(TimeSpan.FromSeconds(_cooldownTime), ignoreTimeScale: false);
            _isOnCooldown = false;
        }
    }
}
