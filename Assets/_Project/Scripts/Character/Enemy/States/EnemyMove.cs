using UnityEngine;

public class EnemyMove : MoveState {

    Vector3 _lastKnownTargetPosition;
    public override void Enter(){
        Debug.Log("Enemy Move/Chase State");
    }

    public override void LogicUpdate(){
        //Make the enemy do not rotate in the Y axis when the player jumps
        Vector3 targetPoint = new (Enemy.Target.transform.position.x, Enemy.transform.position.y, Enemy.Target.transform.position.z);
        Enemy.NavmeshAgent.destination = targetPoint;
        Enemy.transform.LookAt(targetPoint);

        //If the player is out of aggro range move enemy to the lastknown position
        if(Vector3.Distance(Enemy.transform.position, Enemy.Target.transform.position) > Enemy.DeAggroRadius){
            Enemy.NavmeshAgent.destination = _lastKnownTargetPosition;

            if(Vector3.Distance(Enemy.transform.position, _lastKnownTargetPosition) < 1f){
                Enemy.ChangeState(Enemy.Idle);
            }
        }else{
            if(Vector3.Distance(Enemy.transform.position, Enemy.Target.transform.position) < 3f){
                Enemy.NavmeshAgent.destination = Enemy.transform.position;
            }
            //Update in each frame the lastknown position to the current position of the target
            _lastKnownTargetPosition = targetPoint;
        }
    }
     
    public override void Exit(){
        
    }
}