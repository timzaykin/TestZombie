using System.Collections.Generic;
using UnityEngine;

namespace DamageSystem
{
    public class ToothSpawner : MonoBehaviour
    {
        [Header("Tooth Settings")]
        [SerializeField] private GameObject _toothPrefab;
        [SerializeField] private Transform _spawnPoint; 
        [SerializeField] private float _minForce = 3f;
        [SerializeField] private float _maxForce = 10f;
        [SerializeField] private float _torqueForce = 5f;
        [SerializeField] private float _lifeTime = 30f;

        private List<GameObject> _activeTeeth = new List<GameObject>();

        public void SpawnRandomTooth(ContactPoint hitPoint, float damage)
        {
            GameObject tooth = Instantiate(
                _toothPrefab,
                _spawnPoint.position,
                _spawnPoint.rotation
            );
        
            Rigidbody rb = tooth.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 forceDirection = (_spawnPoint.position - hitPoint.point).normalized;
                float force = Mathf.Lerp(_minForce, _maxForce, damage);
            
                rb.AddForce(forceDirection * force, ForceMode.Impulse);
                rb.AddTorque(
                    Random.Range(-_torqueForce, _torqueForce),
                    Random.Range(-_torqueForce, _torqueForce),
                    Random.Range(-_torqueForce, _torqueForce),
                    ForceMode.Impulse
                );
            }
        
            Destroy(tooth, _lifeTime);
            _activeTeeth.Add(tooth);
        }

        public void ClearAllTeeth()
        {
            foreach (var tooth in _activeTeeth)
            {
                if (tooth != null) Destroy(tooth);
            }
            _activeTeeth.Clear();
        }
    }
}