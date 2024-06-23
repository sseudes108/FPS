using UnityEngine;

public class PlayerJump : JumpState {
    public override void Enter(){

    }

    public override void LogicUpdate(){
        Player.HandleMovement();
        Player.Movement.ApplyExtraGravity();
        if(Player.IsGrounded()){
            Player.ChangeState(Player.StateMachine.IdleState);
        }
    }

    public override void Exit(){
        Player.Movement.ResetGravityForce();
        Player.Movement.SetDirection(Vector3.zero);
    }
}