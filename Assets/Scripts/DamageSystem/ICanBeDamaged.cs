using UnityEngine;

namespace DamageSystem
{
    public interface ICanBeDamaged
    {
        Rigidbody GetRigidbody();
        void TakeDamage(float damage, Transform owner, ContactPoint contactPoint);
    }
}