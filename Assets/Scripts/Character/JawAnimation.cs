using UnityEngine;

namespace Character
{
    public class JawAnimation : MonoBehaviour
    {
        public AudioSource _audioSource;
        public Transform _jawBone; 
        public float _sensitivity = 0.1f; 
        public float _smoothTime = 0.1f; 
        public float _maxJawOffset = 0.05f; 
    
        private float[] _samples = new float[1024];
        private float _currentVolume;
        private float _jawVelocity;
        private Vector3 _jawRestPosition; 

        void Start()
        {
            _jawRestPosition = _jawBone.localPosition;
        }

        void Update()
        {
            if(!_audioSource.isPlaying) return;
            // Анализ громкости
            _audioSource.GetOutputData(_samples, 0);
            float sum = 0;
            for (int i = 0; i < _samples.Length; i++)
            {
                sum += Mathf.Abs(_samples[i]);
            }

            _currentVolume = sum / _samples.Length * _sensitivity;

            // Плавное смещение челюсти по Z
            float targetOffset = Mathf.Clamp(_currentVolume, 0, _maxJawOffset);
            float smoothOffset = Mathf.SmoothDamp(
                _jawBone.localPosition.z,
                _jawRestPosition.z + targetOffset,
                ref _jawVelocity,
                _smoothTime
            );

            _jawBone.localPosition = new Vector3(
                _jawRestPosition.x,
                _jawRestPosition.y,
                smoothOffset
            );
        }
    }
}
