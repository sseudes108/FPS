// using System.Collections;
// using UnityEngine;

// //Move - Chase
// public class EnemyMove : MoveState {
//     private Vector3 _lastKnownTargetPosition;
//     float wait;
//     IEnumerator shotRoutine;
//     bool isShooting;

//     public override void Enter(){
//         Enemy.ChangeAnimation(Enemy.RUN);
//     }

//     public override void LogicUpdate(){
//         //Make the enemy do not rotate in the Y axis when the player jumps
//         Vector3 targetPoint = new (Enemy.Target.transform.position.x, Enemy.transform.position.y, Enemy.Target.transform.position.z);
//         Enemy.NavmeshAgent.destination = targetPoint;
//         Enemy.transform.LookAt(targetPoint);

//         /*
//             Se fora do range de aggro o destino var ser o _lastKnown e quando chegar na posição muda o state para idle. 
//                 Caso contrario atualiza a cada frame a _lasknown para a posição atual do target
//         */
//         if(Vector3.Distance(Enemy.transform.position, Enemy.Target.transform.position) > Enemy.DeAggroRadius){
//             Enemy.NavmeshAgent.destination = _lastKnownTargetPosition;
//             if(Vector3.Distance(Enemy.transform.position, _lastKnownTargetPosition) < 0.1f){
//                 Enemy.ChangeState(Enemy.Idle);
//             }
//         }else{
//             if(wait > 0){
//                 wait -= Time.deltaTime;
//             }else{
//                 if(shotRoutine == null){
//                     shotRoutine = Shoot();
//                     Enemy.StartCoroutine(shotRoutine);
//                 }
//             }

//             //Para o enemy quando estiver atirando
//             if(isShooting){
//                 Enemy.NavmeshAgent.destination = Enemy.transform.position;
//             }

//             // Se a distancia for de 1/3 do aggro range o enemy pára na sua posição atual
//             if(Vector3.Distance(Enemy.transform.position, Enemy.Target.transform.position) <= Enemy.AggroRadius / 3){
//                 Enemy.NavmeshAgent.destination = Enemy.transform.position;
//                 Enemy.ChangeAnimation(Enemy.SHOOT);
//             }else{
//                 if(!isShooting){
//                     Enemy.ChangeAnimation(Enemy.RUN);
//                 }
//             }
//             _lastKnownTargetPosition = Enemy.Target.transform.position;
//         }
//     }
     
//     public override void Exit(){ }

//     private IEnumerator Shoot(){
//         //6 Shoots ins a row
//         Enemy.NavmeshAgent.destination = Enemy.transform.position;
//         isShooting = true;
//         for(int i = 0; i < 6; i++){
//             Enemy.HandleShot();
//             Enemy.ChangeAnimation(Enemy.SHOOT);
//             yield return new WaitForSeconds(Enemy.Gun.GunData.Firerate);
//         }
//         shotRoutine = null;
//         isShooting = false;
//         wait = 1f;
//         yield return null;
//     }
// }