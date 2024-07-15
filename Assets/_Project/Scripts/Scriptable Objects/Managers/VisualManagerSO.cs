using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "VisualsManagerSO", menuName = "FPS/Managers/Visuals", order = 2)]
public class VisualManagerSO : ScriptableObject {
    
    [SerializeField] private ObjectPoolManagerSO _objectPool;
    [SerializeField] private ObjectPool<VisualHelper> _bulletImpactVFXPool;
    [SerializeField] private VisualHelper _bulletImpactPrefab;

    public List<Texture2D> CrossesTextures;

    [HideInInspector] public UnityEvent<float> OnFadeFromBlack, OnFadeToBlack;

    private void OnEnable() {
        _bulletImpactVFXPool ??= _objectPool.CreatePool(_bulletImpactPrefab, Vector3.zero);

        OnFadeFromBlack ??= new UnityEvent<float>();
        OnFadeToBlack ??= new UnityEvent<float>();
    }

    public void FadeToBlack(float duration) { OnFadeToBlack?.Invoke(duration); }
    public void FadeFromBlack(float duration) { OnFadeFromBlack?.Invoke(duration); }

    public void BulletImpactEffect(MonoBehaviour caller, Vector3 position, Material material){
        VisualHelper bulletImpact;

        do{
            bulletImpact = _bulletImpactVFXPool.Get();
        }while(bulletImpact == null);

        bulletImpact.transform.position = position;
        bulletImpact.Play(material);
        caller.StartCoroutine(EffectReleaseRoutine(_bulletImpactVFXPool, bulletImpact));
    }

    public IEnumerator EffectReleaseRoutine(ObjectPool<VisualHelper> objectPool, VisualHelper VFX){
        yield return new WaitForSeconds(0.5f);
        ReleaseFromPool(objectPool, VFX);
        yield return null;
    }

    public void ReleaseFromPool(ObjectPool<VisualHelper> objectPool, VisualHelper VFX){
        objectPool.Release(VFX);
    }
}