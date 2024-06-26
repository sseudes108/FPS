using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class VFXManager : MonoBehaviour{
    public VFXHelper _bulletImpact;
    private ObjectPool<VFXHelper> _bulletImpactVFXPool;

    private void OnEnable() {
        Bullet.OnBulletImpact += BulletImpactEffect;
    }

    private void OnDisable() {
        Bullet.OnBulletImpact -= BulletImpactEffect;
    }

    public void Awake(){
        _bulletImpactVFXPool = CreateEffectPool(_bulletImpact);
    }

    private void BulletImpactEffect(Bullet bullet, Material material){
        var bulletImpact = _bulletImpactVFXPool.Get();

        //Makes sure that the effect is instantiated outside the other object
        Vector3 impactPositionOffset = bullet.transform.position - (bullet.transform.forward * 1f);
        bulletImpact.transform.SetPositionAndRotation(impactPositionOffset, Quaternion.identity);

        bulletImpact.Play(material);
        StartCoroutine(EffectRoutine(_bulletImpactVFXPool, bulletImpact));
    }

    //Since the effects are configured to 'none' at end of execution this routine releases it (disable object) from the pool
    private IEnumerator EffectRoutine(ObjectPool<VFXHelper> objectPool, VFXHelper VFX){
        yield return new WaitForSeconds(1f);
        ReleaseFromPool(objectPool, VFX);
        yield return null;
    }

    private ObjectPool<VFXHelper> CreateEffectPool(VFXHelper prefab){
        var VFXPool = new ObjectPool<VFXHelper>(()=>{
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

    public void ReleaseFromPool(ObjectPool<VFXHelper> objectPool, VFXHelper VFX){
        objectPool.Release(VFX);
    }
}
