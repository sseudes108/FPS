using System.Collections;
using UnityEngine;

//Move - Chase
public class EnemyMove : MoveState {
    private Vector3 _lastKnownTargetPosition;
    private IEnumerator _shootRoutine;
    private bool isShooting;

    public override void Enter(){
        _shootRoutine = ShootRoutine();
        Enemy.StartCoroutine(_shootRoutine);
    }

    public override void LogicUpdate(){
        //Make the enemy do not rotate in the Y axis when the player jumps
        Vector3 targetPoint = new (Enemy.Target.transform.position.x, Enemy.transform.position.y, Enemy.Target.transform.position.z);
        Enemy.NavmeshAgent.destination = targetPoint;
        Enemy.transform.LookAt(targetPoint);

        if(isShooting){
            Enemy.NavmeshAgent.destination = Enemy.transform.position;
        }

        //If the player is out of aggro range move enemy to the lastknown position
        if(Vector3.Distance(Enemy.transform.position, Enemy.Target.transform.position) > Enemy.DeAggroRadius){
            Enemy.NavmeshAgent.destination = _lastKnownTargetPosition;
            if(Vector3.Distance(Enemy.transform.position, _lastKnownTargetPosition) < 1f){
                Enemy.ChangeState(Enemy.Idle);
            }
        }else{
            //5f is where the enemy stop moving towards the target
            if(Vector3.Distance(Enemy.transform.position, Enemy.Target.transform.position) < 5f){
                Enemy.NavmeshAgent.destination = Enemy.transform.position;
            }
            //Update in each frame the lastknown position to the current position of the target
            _lastKnownTargetPosition = targetPoint;
        }
    }
     
    public override void Exit(){
        Enemy.StopCoroutine(_shootRoutine);
    }

    private IEnumerator ShootRoutine(){
        //Time before start shooting
        yield return new WaitForSeconds(Enemy.Gun.CoolDown);
        
        //Count of bullet Burst
        for(int i = 0; i < Enemy.Gun.BulletBurst ; i++){
            while(Vector3.Distance(Enemy.transform.position, Enemy.Target.transform.position) < Enemy.DeAggroRadius){
                isShooting = true;
                Enemy.HandleShot();
                yield return new WaitForSeconds(Enemy.Gun.Firerate);

                i++;
                if(i == Enemy.Gun.BulletBurst ){
                    isShooting = false;
                    yield return new WaitForSeconds(Enemy.Gun.CoolDown);
                    i = 0;
                }

                yield return null;
            }
        }
    }
}