using UnityEngine;

public class EnemyPatrol : AbstractState {
    public override void Enter(){
        Enemy.ChangeAnimation(Enemy.RUN);
    }

    public override void LogicUpdate(){
        Enemy.HandlePlayerDetection();

        if(Vector3.Distance(Enemy.transform.position, Enemy.InitialPosition) > 1f){
            Enemy.NavmeshAgent.destination = Enemy.InitialPosition;
        }else{
            if(Enemy.Anim.CurrentAnimation == Enemy.IDLE){
                return;
            }
            Enemy.ChangeAnimation(Enemy.IDLE);
        }
    }

    public override void Exit(){}
}