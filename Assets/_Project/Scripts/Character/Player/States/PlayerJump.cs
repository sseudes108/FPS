using UnityEngine;

public class PlayerJump : JumpState {
    private bool _canDoubleJump;
    public override void Enter(){
        _canDoubleJump = true;
    }

    public override void LogicUpdate(){
        Player.HandleMovement();
        
        if(Player.Input.Jump && _canDoubleJump){
            Player.Movement.Jump(true);
            _canDoubleJump =  false;
        }

        if(Player.IsGrounded()){
            Player.ChangeState(Player.StateMachine.IdleState);
        }
    }

    public override void Exit(){
        Player.Movement.SetCharacterDirection(Vector3.zero);
    }
}