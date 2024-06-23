using UnityEngine;

public class PlayerMove : MoveState{
    public override void Enter(){

    }

    public override void LogicUpdate(){
        Player.HandleMovement();
        Player.HandleJump();
        if(Player.Input.Move.x == 0 && Player.Input.Move.y == 0){
            Player.ChangeState(Player.Idle);
        }
        if(!Player.IsGrounded()){
            Player.ChangeState(Player.Jump);
        }
    }

    public override void Exit(){
        
    }
}