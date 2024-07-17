using System.Collections;
using UnityEngine;

public class TurretIdle : IdleState {
    private bool _rotate;
    private Quaternion _targetRotation;
    public override void Enter(){
        Turret.StartCoroutine(RotationRoutine());
    }

    public override void LogicUpdate(){
        Turret.transform.localRotation = Quaternion.Lerp(Turret.transform.localRotation, _targetRotation, Turret.RotateSpeed * Time.deltaTime);
    }

    public override void Exit(){
        _rotate = false;
    }  

    public IEnumerator RotationRoutine(){
        do{
            _targetRotation = Turret.transform.localRotation * Quaternion.Euler(0, Random.Range(-30, 30), 0);
            _rotate = true;
            yield return new WaitForSeconds(5f);
        }while(_rotate);
        yield return null;
    }

}