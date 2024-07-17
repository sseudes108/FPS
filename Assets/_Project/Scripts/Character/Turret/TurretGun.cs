using System.Collections;
using UnityEngine;

public class TurretGun : MonoBehaviour {
    public ObjectPoolManagerSO _poolManager;
    public Transform[] FirePoints;
    public Material _bulletMaterial;
    public TurretBullet _bulletPrefab;
    public int _damage;

    public void Shoot(){
        StartCoroutine(ShootRoutine());
    }

    public IEnumerator ShootRoutine(){
        foreach(var point in FirePoints){
            var target = GetComponent<Turret>().Target;
            if(target == null) { break; }
            point.LookAt(target.transform.position + new Vector3(0f, 1.2f, 0f));
            var newBullet = Instantiate(_bulletPrefab, point.position, point.rotation);
            newBullet.SetDamage(_damage);
            newBullet.SetMaterial(_bulletMaterial);
            newBullet.SetDirection(point);
            yield return new WaitForSeconds(0.05f);
        }
    }
}