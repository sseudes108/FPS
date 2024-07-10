using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class VisualEffects : MonoBehaviour{
    public FadeScreenShaderController FadeScreen { get; private set; }	
    public VisualHelper _bulletImpact;
    private ObjectPool<VisualHelper> _bulletImpactVFXPool;

    private void OnEnable() {
        Bullet.OnBulletImpact += Bullet_OnBulletImpact;
    }

    private void OnDisable() {
        Bullet.OnBulletImpact -= Bullet_OnBulletImpact;
    }

    public void Awake(){
        SetComponents();
        CreatePools();
    }

    private void Bullet_OnBulletImpact(Bullet bullet, Material material){
        var bulletImpact = _bulletImpactVFXPool.Get();

        bulletImpact.transform.SetPositionAndRotation(bullet.transform.position, Quaternion.identity);

        bulletImpact.Play(material);
        StartCoroutine(EffectReleaseRoutine(_bulletImpactVFXPool, bulletImpact));
    }

    //Since the effects are configured to 'none' at end of execution this routine releases it (disable object) from the pool
    private IEnumerator EffectReleaseRoutine(ObjectPool<VisualHelper> objectPool, VisualHelper VFX){
        yield return new WaitForSeconds(0.5f);
        ReleaseFromPool(objectPool, VFX);
        yield return null;
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

    public void CreatePools(){
        _bulletImpactVFXPool = CreateEffectPool(_bulletImpact);
    }

    public void SetComponents(){
        FadeScreen = GetComponent<FadeScreenShaderController>();
    }
}
