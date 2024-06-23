using UnityEngine;

public class PlayerMove : MoveState{
    public override void Enter(){
        Debug.Log("Enter Player Move State");
    }

    public override void LogicUpdate(){
        Player.HandleMovement();
        if(Player.Input.Move.x == 0 && Player.Input.Move.y == 0){
            Player.ChangeState(Player.Idle);
        }
        if(!Player.Movement.IsGrounded()){
            Player.ChangeState(Player.Jump);
        }
    }

    public override void Exit(){
        
    }
}