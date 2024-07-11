using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "VisualsEventHandlerSO", menuName = "FPS/EventHandlers/Visuals", order = 2)]
public class VisualsEventHandlerSO : ScriptableObject {
    public List<Texture2D> CrossesTextures;
    public VisualHelper BulletImpactPrefab;
    public ObjectPool<VisualHelper> BulletImpactVFXPool;
    public UnityEvent<Bullet, Material> OnBulletImpact;
    public UnityEvent<float> OnFadeFromBlack;
    public UnityEvent<float> OnFadeToBlack;

    private void OnEnable() {
        BulletImpactVFXPool ??= CreateEffectPool(BulletImpactPrefab);

        OnBulletImpact ??= new UnityEvent<Bullet, Material>();
        OnFadeFromBlack ??= new UnityEvent<float>();
        OnFadeToBlack ??= new UnityEvent<float>();
    }

    private ObjectPool<VisualHelper> CreateEffectPool(VisualHelper prefab){
        var VFXPool = new ObjectPool<VisualHelper>(()=>{
            return Instantiate(prefab);
        }, newEffect =>{
            newEffect.gameObject.SetActive(true);
        }, newEffect =>{
            newEffect.gameObject.SetActive(false);
        }, newEffect =>{
            Destroy(newEffect);
        }, false, 50, 70);

        return VFXPool;
    }

    public void ReleaseFromPool(ObjectPool<VisualHelper> objectPool, VisualHelper VFX){
        objectPool.Release(VFX);
    }

    public void BulletImpactEffect(Bullet bullet, Material material){
        // OnBulletImpact?.Invoke(bullet, material);
        var bulletImpact = BulletImpactVFXPool.Get();
        bulletImpact.transform.SetPositionAndRotation(bullet.transform.position, Quaternion.identity);
        bulletImpact.Play(material);
        GameController.Instance.StartCoroutine(EffectReleaseRoutine(BulletImpactVFXPool, bulletImpact));
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