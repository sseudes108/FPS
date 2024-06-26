using UnityEngine;

public class EnemyPatrol : AbstractState {
    public override void Enter(){
    }
    public override void LogicUpdate(){
        Enemy.HandlePlayerDetection();

        if(Vector3.Distance(Enemy.transform.position, Enemy.InitialPosition) > 1f){
            Enemy.NavmeshAgent.destination = Enemy.InitialPosition;
        }
    }
    public override void Exit(){}
}