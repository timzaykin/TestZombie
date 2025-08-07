using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DamageSystem
{
    [RequireComponent(typeof(ScoreSystem))]
    [RequireComponent(typeof(DamageVFXHandler))]
    [RequireComponent(typeof(DamageSFXHandler))]
    public class DamageHandler : MonoBehaviour
    {
        [SerializeField] private TimeSlowdown _timeSloowdown;
        [SerializeField] private ToothSpawner _toothSpawner;
    
        [Header("Physics Settings")]
        [SerializeField] private float forceMultiplier = 5f;
        [SerializeField] private float maxForce = 100f;
        [SerializeField] private Character.Character _character;
        private ScoreSystem _scoreSystem;
        private DamageVFXHandler _vfxHandler;
        private DamageSFXHandler _sfxHandler;

        private void Awake()
        {
            _scoreSystem = GetComponent<ScoreSystem>();
            _vfxHandler = GetComponent<DamageVFXHandler>();
            _sfxHandler = GetComponent<DamageSFXHandler>();
        }

        public void HandleDamage(float damage, Transform damager, ContactPoint contact, Rigidbody rb, bool headPart)
        {
            // Добавляем очки
            _scoreSystem.AddDamageScore(damage);

            // Получаем тип урона
            var damageType = _scoreSystem.GetDamageType(damage);
        
            //Реагируем на удар
            SetCharacterReaction(damageType);
        
            // Визуальные эффекты
            _vfxHandler.PlayVFX(damageType, contact, rb.transform);

            // Звуковые эффекты
            _sfxHandler.PlaySound(damageType);
        
            if(damageType == ScoreSystem.DamageType.Normal)
                ApplyForce(damager, contact,damage,rb);
            if (damageType == ScoreSystem.DamageType.Critical)
            {
                ApplyForce(damager, contact,damage,rb);
                _timeSloowdown.ApplySlowdown().Forget();
                if(headPart)_toothSpawner.SpawnRandomTooth(contact,damage);
            }
        }

        private void SetCharacterReaction(ScoreSystem.DamageType damageType)
        {
            switch (damageType)
            {
                case ScoreSystem.DamageType.Minor:
                    _character.TryToProcessInput("[Тебя ударили, но очень слабо, вырази своё мнение с максимальной токсичностью]");
                    break;
                case ScoreSystem.DamageType.Normal:
                    _character.TryToProcessInput("[Тебя ударили весьма ощутим,но ты и не такое терпел,вырази своё мнение об этом]");
                    break;
                case ScoreSystem.DamageType.Critical:
                    _character.TryToProcessInput("[Тебя очень-очень большо ударили, пожалуйся на жизнь, и попроси так больше не делать]");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(damageType), damageType, null);
            }
        }
    
        private void ApplyForce(Transform damager, ContactPoint contact, float damage, Rigidbody rb)
        {
            if (rb == null) return;

            Vector3 direction = (transform.position - damager.position).normalized;
            float force = Mathf.Min(damage * forceMultiplier, maxForce);
        
            rb.AddForceAtPosition(direction * force, contact.point, ForceMode.Impulse);
        }
    
        private void ApplyVibration(ScoreSystem.DamageType damageType)
        {
 
        }
    }
}
