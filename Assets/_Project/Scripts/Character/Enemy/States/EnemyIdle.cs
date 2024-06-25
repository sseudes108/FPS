using System.Collections;
using UnityEngine;

public class EnemyIdle : IdleState {
    public override void Enter(){
        Debug.Log("Enemy Idle State");
        Enemy.StartCoroutine(WaitRoutine());
    }

    public override void LogicUpdate(){
        if(Enemy.PlayerDetected()){
            Enemy.ChangeState(Enemy.Move);
        }
    }

    public override void Exit(){}

    private IEnumerator WaitRoutine(){
        yield return new WaitForSeconds(Random.Range(0.5f, 2.5f));
        Enemy.ChangeState(Enemy.PatrolState);
        yield return null;
    }
}