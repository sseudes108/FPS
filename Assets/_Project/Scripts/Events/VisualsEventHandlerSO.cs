using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "VisualsEventHandlerSO", menuName = "FPS/EventHandlers/Visuals", order = 2)]
public class VisualsEventHandlerSO : ScriptableObject {
    public ObjectPoolSO ObjectPool;
    public ObjectPool<VisualHelper> BulletImpactVFXPool;
    public VisualHelper BulletImpactPrefab;

    public List<Texture2D> CrossesTextures;

    public UnityEvent<float> OnFadeFromBlack;
    public UnityEvent<float> OnFadeToBlack;

    private void OnEnable() {
        BulletImpactVFXPool ??= ObjectPool.CreatePool(BulletImpactPrefab, Vector3.zero);

        OnFadeFromBlack ??= new UnityEvent<float>();
        OnFadeToBlack ??= new UnityEvent<float>();
    }

    public void ReleaseFromPool(ObjectPool<VisualHelper> objectPool, VisualHelper VFX){
        objectPool.Release(VFX);
    }

    public void BulletImpactEffect(Bullet bullet, Material material){
        VisualHelper bulletImpact;

        do{
            bulletImpact = BulletImpactVFXPool.Get();
        }while(bulletImpact == null);

        bulletImpact.transform.SetPositionAndRotation(bullet.transform.position, Quaternion.identity);
        bulletImpact.Play(material);

        if(bullet.gameObject.activeInHierarchy){
            bullet.StartCoroutine(EffectReleaseRoutine(BulletImpactVFXPool, bulletImpact));
        }
    }

    public IEnumerator EffectReleaseRoutine(ObjectPool<VisualHelper> objectPool, VisualHelper VFX){
        yield return new WaitForSeconds(0.5f);
        ReleaseFromPool(objectPool, VFX);
        yield return null;
    }

    public void FadeToBlack(float duration){
        OnFadeToBlack?.Invoke(duration);
    }

    public void FadeFromBlack(float duration){
        OnFadeFromBlack?.Invoke(duration);
    }
}