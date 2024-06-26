using System.Collections;
using UnityEngine;

public class EnemyIdle : IdleState {
    public override void Enter(){
        Enemy.StartCoroutine(WaitRoutine());
    }

    public override void LogicUpdate(){
        Enemy.HandlePlayerDetection();
    }

    public override void Exit(){}

    private IEnumerator WaitRoutine(){
        yield return new WaitForSeconds(Random.Range(2f, 3f));
        Enemy.ChangeState(Enemy.PatrolState);
        yield return null;
    }
}