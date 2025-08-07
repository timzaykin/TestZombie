using UnityEngine;

namespace DamageSystem
{
    public class DamagablePart: MonoBehaviour, ICanBeDamaged
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private DamageHandler _handler;
        [SerializeField] private bool _headPart;

        private void OnValidate()
        {
            if (_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();
            _handler = GetComponentInParent<DamageHandler>();
        }
    
        public Rigidbody GetRigidbody() => _rigidbody;

        public void TakeDamage(float damage, Transform owner, ContactPoint point)
        {
            _handler.HandleDamage(damage,owner, point, _rigidbody, _headPart);
        }
    }
}