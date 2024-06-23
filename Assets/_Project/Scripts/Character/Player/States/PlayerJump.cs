using UnityEngine;

public class PlayerJump : JumpState {
    public override void Enter(){
        Debug.Log("Enter Player Jump State");
    }

    public override void LogicUpdate(){
        Player.Movement.ApplyExtraGravityForce();
        if(Player.Movement.IsGrounded()){
            Player.ChangeState(Player.StateMachine.IdleState);
        }
    }

    public override void Exit(){
        Player.Movement.ResetGravityForce();
    }
}