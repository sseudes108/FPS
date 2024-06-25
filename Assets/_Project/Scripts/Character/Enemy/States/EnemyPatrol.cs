using UnityEngine;

public class EnemyPatrol : AbstractState {
    public override void Enter(){
        Debug.Log("Enemy Patrol State");
    }
    public override void LogicUpdate(){
        if(Enemy.PlayerDetected()){
            Enemy.ChangeState(Enemy.Move);
        }
    }
    public override void Exit(){}
}