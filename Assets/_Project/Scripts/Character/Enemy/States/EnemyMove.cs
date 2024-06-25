using UnityEngine;

public class EnemyMove : MoveState {
    public override void Enter(){
        Debug.Log("Enemy Move/Chase State");
    }

    public override void LogicUpdate(){
        //Make the enemy do not rotate in the Y axis when the player jumps
        Vector3 targetPoint = new (Enemy.Target.transform.position.x, Enemy.transform.position.y, Enemy.Target.transform.position.z);
        // Enemy.transform.LookAt(targetPoint);
        // Enemy.Movement.SetCharacterDirection(Enemy.transform.forward);
        Enemy.NavmeshAgent.destination = targetPoint;

        if(Vector3.Distance(Enemy.transform.position, Enemy.Target.transform.position) > Enemy.DeAggroRadius){
            Enemy.ChangeState(Enemy.Idle);
        }
    }
     
    public override void Exit(){
        // Enemy.Movement.SetCharacterDirection(Vector3.zero);
    }
}