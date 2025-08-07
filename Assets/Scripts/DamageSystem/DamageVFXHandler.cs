using UnityEngine;

namespace DamageSystem
{
    public class DamageVFXHandler : MonoBehaviour
    {
        [Header("VFX References")]
        [SerializeField] private GameObject _minorVFX;
        [SerializeField] private GameObject _normalVFX;
        [SerializeField] private GameObject _criticalVFX;

        [Header("Settings")]
        [SerializeField] private float _decalLifeTime = 30f;
        [SerializeField] private float _vfxLifeTime = 3f;
    
        [SerializeField] private Material _decalMaterial;
        [SerializeField] private LayerMask _decalLayer;

        public void PlayVFX(ScoreSystem.DamageType type, ContactPoint contact, Transform parent)
        {
            GameObject vfxToPlay = type switch
            {
                ScoreSystem.DamageType.Minor => _minorVFX,
                ScoreSystem.DamageType.Normal => _normalVFX,
                ScoreSystem.DamageType.Critical => _criticalVFX,
                _ => null
            };

            if (vfxToPlay != null)
            {
                var instance = Instantiate(vfxToPlay, contact.point, Quaternion.LookRotation(contact.normal), parent);
                Destroy(instance.gameObject, _vfxLifeTime);
            }

            if(type == ScoreSystem.DamageType.Critical) SpawnDecal(contact, type, parent);
        }

        private void SpawnDecal(ContactPoint contact, ScoreSystem.DamageType type, Transform parent)
        {
            float sizeMultiplier = type switch
            {
                ScoreSystem.DamageType.Critical => 5f,
                ScoreSystem.DamageType.Normal => 0.2f,
                _ => 0.1f
            };

            GameObject decal = new GameObject("Decal");
            decal.transform.position = contact.point + contact.normal * 0.01f;
            decal.transform.rotation = Quaternion.LookRotation(-contact.normal);
            decal.transform.parent = parent; 

            Projector projector = decal.AddComponent<Projector>();
            projector.material = _decalMaterial;
            projector.ignoreLayers = ~_decalLayer; 
            projector.farClipPlane = 0.2f;
            projector.orthographic = true;
            projector.orthographicSize = sizeMultiplier;

            Destroy(decal, _decalLifeTime);
        }
    }
}