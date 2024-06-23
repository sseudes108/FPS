using UnityEngine;

public class PlayerIdle : IdleState{
    public override void Enter(){
        Debug.Log("Enter Player Idle State");
    }

    public override void LogicUpdate(){
        if(Player.Input.Move.x != 0 || Player.Input.Move.y != 0){
            Player.ChangeState(Player.Move);
        }
        if(!Player.Movement.IsGrounded()){
            Player.ChangeState(Player.Jump);
        }
    }

    public override void Exit(){
        
    }
}