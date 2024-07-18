using System.Collections;
using UnityEngine;

public class TurretGun : MonoBehaviour {
    [SerializeField] private ObjectPoolManagerSO _poolManager;
    [SerializeField] private Transform[] FirePoints;
    [SerializeField] private Material _bulletMaterial;
    [SerializeField] private TurretBullet _bulletPrefab;
    [SerializeField] private int _damage;

    public void Shoot(){
        StartCoroutine(ShootRoutine());
    }

    public IEnumerator ShootRoutine(){
        foreach(var point in FirePoints){
            var target = GetComponent<Turret>().Target;
            if(target == null) { break; }
            point.LookAt(target.Camera.transform.position + new Vector3(0f, 1.2f, 0f));
            var newBullet = Instantiate(_bulletPrefab, point.position, point.rotation); //Used the instantiation because of some strange behaviour in getting the bullets from a pool
            newBullet.SetDamage(_damage);
            newBullet.SetMaterial(_bulletMaterial);
            newBullet.SetDirection(point);
            yield return new WaitForSeconds(0.05f);
        }
    }
}