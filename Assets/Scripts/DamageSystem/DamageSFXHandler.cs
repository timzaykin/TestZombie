using UnityEngine;

namespace DamageSystem
{
    public class DamageSFXHandler : MonoBehaviour
    {
        [Header("SFX References")]
        [SerializeField] private AudioClip[] _minorSounds;
        [SerializeField] private AudioClip[] _normalSounds;
        [SerializeField] private AudioClip[] _criticalSounds;

        [Header("Settings")]
        [SerializeField] private float _volume = 0.8f;
        [SerializeField] private float _spatialBlend = 0.9f;

        [SerializeField] private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.spatialBlend = _spatialBlend;
        }

        public void PlaySound(ScoreSystem.DamageType type)
        {
            AudioClip[] clips = type switch
            {
                ScoreSystem.DamageType.Minor => _minorSounds,
                ScoreSystem.DamageType.Normal => _normalSounds,
                ScoreSystem.DamageType.Critical => _criticalSounds,
                _ => null
            };

            if (clips != null && clips.Length > 0)
            {
                AudioClip clip = clips[Random.Range(0, clips.Length)];
                _audioSource.PlayOneShot(clip, _volume);
            }
        }
    }
}