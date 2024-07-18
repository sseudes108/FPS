using System.Collections;
using UnityEngine;

public class TurretAttack : AbstractState{
    private bool _shoot;
    private IEnumerator _shootRoutine;
    public override void Enter(){
        _shoot = true;
        _shootRoutine = ShootRoutine();
        if(Turret != null){
            Turret.StartCoroutine(_shootRoutine);
        }
    }

    public override void Exit(){
        _shoot = false;
    }

    public override void LogicUpdate(){
        if(Turret.Target == null) { return; }
        Vector3 targetDirection = Turret.Target.transform.position - Turret.TurretWeapon.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        targetRotation *= Quaternion.Euler(0, 180, 0);
        Turret.TurretWeapon.transform.rotation = Quaternion.Slerp(Turret.TurretWeapon.transform.rotation, targetRotation, Time.deltaTime * Turret.RotateSpeed * 5);
    }

    public IEnumerator ShootRoutine(){
        do{
            yield return new WaitForSeconds(1f);
            Turret.TurretGun.Shoot();
        }while(_shoot);
        yield return null;
    }

}